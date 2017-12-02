using System;
using System.ComponentModel.DataAnnotations;

namespace ForexSignals.Data.Requests
{
    public class NewUserRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public DateTime TermsAccepted { get; set; }
    }
}
