using System.Threading.Tasks;
using ForexSignals.Data.Exceptions;
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

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _adapter.QueryFirstOrDefaultAsync<User>(u => u.Username == username);
        }

        public async Task<User> SaveUserAsync(User user)
        {
            var checkDuplicateUser = await _adapter.QueryFirstOrDefaultAsync<User>(u => u.Username == user.Username);
            if (checkDuplicateUser != null)
            {
                throw new DuplicateUserException($"Username '{user.Username}' has already been taken");
            }
            return await _adapter.UpsertAsync(user);
        }
    }
}
