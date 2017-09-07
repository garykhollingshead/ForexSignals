using ForexSignals.Data.Persistance;

namespace ForexSignals.Data.Models
{
    public class User : ModelWithIdentity
    {
        public string Username { get; set; }
    }
}
