using System;
using ForexSignals.Data.Enums;
using ForexSignals.Data.Models;

namespace ForexSignals.Data.Responses
{
    public class UserResponse
    {
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public DateTime TermsAccepted { get; set; }
        public UserType UserType { get; set; }

        public UserResponse() { }

        public UserResponse(User user)
        {
            Username = user.Username;
            Firstname = user.Firstname;
            Lastname = user.Lastname;
            Email = user.Email;
            TermsAccepted = user.TermsAccepted;
            UserType = user.UserType;
        }
    }
}
