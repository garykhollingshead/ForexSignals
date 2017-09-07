using System.Threading.Tasks;
using ForexSignals.Data.Models;
using ForexSignals.DataAccess.Adapters;

namespace ForexSignals.DataAccess.Repositories
{
    public class UserRepository
    {
        private readonly MartenAdapter _adapter;

        public UserRepository(MartenAdapter adapter)
        {
            _adapter = adapter;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _adapter.GetByIdAsync<User>(id);
        }

        public async Task<User> SaveUserAsync(User user)
        {
            return await _adapter.UpsertAsync(user);
        }
    }
}
