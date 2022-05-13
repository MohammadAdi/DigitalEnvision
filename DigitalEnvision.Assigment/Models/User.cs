using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DigitalEnvision.Assigment.Models
{
    public class User
    {
        public long Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime BirtdayDate { get; set; }
        public long LocationId { get; set; }
        public Location Location { get; set; }

        public List<Models.Jobs.AlertLog> Alerts { get; set; }
    }
}
