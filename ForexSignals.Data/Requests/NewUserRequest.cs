using System.ComponentModel.DataAnnotations;

namespace ForexSignals.Data.Requests
{
    public class NewUserRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
