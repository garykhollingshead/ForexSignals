using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ForexSignals.AuthServer.Attributes;
using ForexSignals.AuthServer.BusinessLogic;
using ForexSignals.Data.Controllers;
using ForexSignals.Data.Exceptions;
using ForexSignals.Data.Models;
using ForexSignals.Data.Requests;
using ForexSignals.Data.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace ForexSignals.AuthServer.Controllers
{
    [Route("Auth")]
    [CustomExceptionFilter, ValidateModel]
    public class AuthenticationController : Controller
    {
        private readonly UserActions _userActions;

        public AuthenticationController(UserActions userActions)
        {
            _userActions = userActions;
        }

        [HttpPost, Route("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]UserLoginRequest login)
        {
            if (!await _userActions.AuthenticateUserPasswordAsync(login))
            {
                return Unauthorized();
            }

            var token = _userActions.GetToken(login.Username);

            return new OkObjectResult(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        [HttpGet("Authenticate"), Authorize]
        public IActionResult Authenticate()
        {
            var token = _userActions.GetToken(User.Claims.First().Value);

            return new OkObjectResult(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        [HttpPost("AddNewUser", Name = "AddNewUser")]
        public async Task<IActionResult> AddNewUser([FromBody]NewUserRequest newUserRequest)
        {
            var user = await _userActions.AddUserAsync(newUserRequest);

            var token = _userActions.GetToken(user.Username);

            return new OkObjectResult(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        [HttpGet("GetUser"), Authorize]
        public async Task<IActionResult> GetUser()
        {
            return new JsonResult(await _userActions.GetUserByNameAsync(User.Claims.First().Value));
        }
    }
}
