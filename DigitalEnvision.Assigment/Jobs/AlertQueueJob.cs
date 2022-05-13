using DigitalEnvision.Assigment.Infrastructures;
using DigitalEnvision.Assigment.Models.Jobs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalEnvision.Assigment.Jobs
{
    public interface IAlertQueueJob
    {
        Task RunAlertQueJob();
    }

    public class AlertQueueJob : IAlertQueueJob
    {
        private readonly IApplicationDbContext _context;

        public AlertQueueJob(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task RunAlertQueJob()
        {

            var birthdayUserIds = await _context.Users.Include(x => x.Alerts)
                                    .Where(x => !x.Alerts.Any(x => x.LastExecution.HasValue && x.LastExecution.Value.Year <= DateTime.UtcNow.Year))
                                    .Select(x => x.Id).ToArrayAsync();

            var userIds = await _context.Users.Include(x => x.Alerts)
                    .Where(x => !x.Alerts.Any(y => y.LastExecution.HasValue && y.LastExecution.Value.Year <= DateTime.UtcNow.Year) && !x.Alerts.Any(x => birthdayUserIds.Contains(x.Id)))
                    .Select(x => x.Id).Distinct().ToArrayAsync();

            var alertLogs = new List<AlertLog>();

            foreach (var item in userIds)
            {
                var queue = new AlertLog()
                {
                    UserId = item,
                    RetryCount = 0,
                    Status = Helpers.Enums.AlertStatus.New,
                    LastExecution = null
                };
                alertLogs.Add(queue);
            }
            _context.AlertLogs.AddRange(alertLogs);
            await _context.SaveChanges();

        }
    }
}
