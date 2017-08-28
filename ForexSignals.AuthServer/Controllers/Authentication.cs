using System.Threading.Tasks;
using ForexSignals.Data.Controllers;
using ForexSignals.Data.Requests;
using ForexSignals.Data.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ForexSignals.AuthServer.Controllers
{
    public class Authentication : ApiController
    {
        [HttpPost, Route("Authenticate")]
        public async Task<JsonResult> Authenticate(UserLoginRequest login)
        {

            return new JsonResult(new UserLoginResponse());
        }

        [HttpPost, Route("AddNewUser")]
        public async Task<IActionResult> AddNewUser(NewUserRequest newUserRequest)
        {

            return new JsonResult(new NewUserResponse());
        }
    }
}
