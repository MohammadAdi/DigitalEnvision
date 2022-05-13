using DigitalEnvision.Assigment.Helpers;
using DigitalEnvision.Assigment.Helpers.Enums;
using DigitalEnvision.Assigment.Infrastructures;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DigitalEnvision.Assigment.Jobs
{
    public interface ISendAlert
    {
        Task RunSendAlert();
    }
    [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    public class SendAlert : ISendAlert
    {
        private readonly IApplicationDbContext _context;
        private readonly Hookbin _config;
        const int _maxRetryCount = 3;
        public SendAlert(IApplicationDbContext context, IOptions<Hookbin> config)
        {
            _context = context;
            _config = config.Value;
        }
        public async Task RunSendAlert()
        {
            var queueLists = await _context.AlertLogs.Include(x => x.User).ThenInclude(x => x.Location).Where(x => x.Status == AlertStatus.New || x.Status == AlertStatus.Process).ToListAsync();
            if (!queueLists.Any()) return;

            foreach (var alert in queueLists)
            {
                var addHoursTimezone = alert.User.Location.TimeZone;
                if (DateTime.UtcNow.AddHours(addHoursTimezone).Hour < _config.Starting_Send)
                    continue;

                try
                {
                    alert.Status = AlertStatus.Process;

                    var alertName = alert.User.FirstName + " " + alert.User.LastName;
                    var hookBinUrl = _config.Url;
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(hookBinUrl);

                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        var message = "Hey " + alertName + " it's your birthday";
                        var json = JsonConvert.SerializeObject(message);

                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                    }

                    //When status response is OK
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        alert.Status = AlertStatus.Success;
                        alert.LastExecution = System.DateTime.UtcNow;
                        alert.ErrorLog=string.Empty
                    }
                    else
                    {
                        alert.RetryCount += 1;
                        alert.LastExecution = System.DateTime.UtcNow;
                        alert.Status = alert.RetryCount >= _maxRetryCount ? AlertStatus.Failed : alert.Status;
                        alert.ErrorLog = httpResponse.StatusCode.ToString();
                    }

                    _context.AlertLogs.Update(alert);
                }
                catch (System.Exception ex)
                {
                    alert.RetryCount += 1;
                    alert.LastExecution = System.DateTime.UtcNow;
                    alert.Status = alert.RetryCount >= _maxRetryCount ? AlertStatus.Failed : alert.Status;
                    alert.ErrorLog = ex.Message;
                    _context.AlertLogs.Update(alert);
                }
                await _context.SaveChanges();
            }
        }
    }
}
