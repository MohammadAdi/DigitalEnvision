using DigitalEnvision.Assigment.Infrastructures;
using Microsoft.EntityFrameworkCore;
using System;
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
            //Get all users whose birthday in today 
            var userIds = await _context.Users.Include(x => x.LocationId).Include(x => x.Alerts)
                                    .Where(x => x.BirtdayDate.Date == DateTime.UtcNow.AddHours(x.Location.TimeZone)
                                       && !x.Alerts.Any(x => x.LastExecution.Year != DateTime.UtcNow.Year))
                                    .Select(x => x.Id).ToArrayAsync();


            var exitingUserIds = await _context.Users.Include(x => x.Alerts)
                                    .Where(x => !x.Alerts.Any(x => x.LastExecution.Year <= DateTime.UtcNow.Year))
                                    .Select(x => x.Id).Distinct().ToArrayAsync();
        }
    }
}
