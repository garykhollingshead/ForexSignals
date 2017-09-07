using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForexSignals.Data.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ForexSignals.AuthServer.Attributes
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            switch (exception)
            {
                case DuplicateUserException duplicateUserException:
                    context.Result = new BadRequestObjectResult(duplicateUserException.Message);
                    break;
                default:
                    context.Result = new StatusCodeResult(500);
                    break;
            }
        }
    }
}
