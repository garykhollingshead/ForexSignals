using System.Threading.Tasks;
using ForexSignals.AuthServer.BusinessLogic;
using ForexSignals.Data.Controllers;
using ForexSignals.Data.Requests;
using ForexSignals.Data.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ForexSignals.AuthServer.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly UserActions _userActions;

        public AuthenticationController(UserActions userActions)
        {
            _userActions = userActions;
        }

        [HttpPost, Route("Authenticate")]
        public async Task<JsonResult> Authenticate(UserLoginRequest login)
        {

            return new JsonResult(new UserLoginResponse());
        }

        [HttpPost("AddNewUser", Name = "AddNewUser")]
        public async Task<IActionResult> AddNewUser(NewUserRequest newUserRequest)
        {
            return new JsonResult(await _userActions.AddUserAsync(newUserRequest));
        }
    }
}
