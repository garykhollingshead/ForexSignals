using System.Threading.Tasks;
using ForexSignals.Data.Configuration;
using ForexSignals.Data.Models;
using ForexSignals.Data.Requests;
using ForexSignals.Data.Responses;
using ForexSignals.DataAccess.Adapters;
using ForexSignals.DataAccess.Repositories;
using Microsoft.Extensions.Options;

namespace ForexSignals.AuthServer.BusinessLogic
{
    public class UserActions
    {
        private readonly UserRepository _userRepository;

        public UserActions(IOptions<AppSettings> config)
        {
            var postgresConfig = config.Value.PostgresSettings;
            _userRepository = new UserRepository(new MartenAdapter(postgresConfig));
        }

        public async Task<NewUserResponse> AddUserAsync(NewUserRequest newUser)
        {
            var user = new User
            {
                Username = newUser.Username
            };
            user = await _userRepository.SaveUserAsync(user);
            return new NewUserResponse
            {
                Id = user.Id,
                Username = user.Username
            };
        }
    }
}
