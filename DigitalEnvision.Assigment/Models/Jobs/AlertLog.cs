using DigitalEnvision.Assigment.Helpers.Enums;
using System;

namespace DigitalEnvision.Assigment.Models.Jobs
{
    public class AlertLog
    {
        public long Id { get; set; }
        public User User { get; set; }
        public long UserId { get; set; }
        public AlertStatus Status { get; set; }
        public int RetryCount { get; set; }
        public DateTime? LastExecution { get; set; }
        public string ErrorLog { get; set; }
    }
}
