using System;
using ForexSignals.Data.Enums;
using ForexSignals.Data.Persistance;

namespace ForexSignals.Data.Models
{
    public class User : ModelWithIdentity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public DateTime TermsAccepted { get; set; }
        public UserType UserType { get; set; }
    }
}
