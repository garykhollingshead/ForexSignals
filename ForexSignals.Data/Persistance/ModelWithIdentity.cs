using Marten.Schema;

namespace ForexSignals.Data.Persistance
{
    public class ModelWithIdentity
    {
        [Identity]
        public string Id { get; set; }
    }
}
