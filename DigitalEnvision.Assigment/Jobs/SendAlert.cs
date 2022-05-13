using DigitalEnvision.Assigment.Infrastructures;
using System.Threading.Tasks;

namespace DigitalEnvision.Assigment.Jobs
{
    public interface ISendAlert
    {
        Task RunSendAlert();
    }
    public class SendAlert : ISendAlert
    {
        private readonly IApplicationDbContext _context;

        public SendAlert(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task RunSendAlert()
        {
        }
    }
}
