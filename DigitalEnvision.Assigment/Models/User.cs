using System;

namespace DigitalEnvision.Assigment.Models
{
    public class User
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirtdayDate { get; set; }
        public string Location { get; set; }
    }
}
