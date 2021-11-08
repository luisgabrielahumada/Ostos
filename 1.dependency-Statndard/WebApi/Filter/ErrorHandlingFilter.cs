using Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using System.Security.Authentication;

namespace Filter
{
    [SwaggerExclude]
    public class ErrorHandlingFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            HandleExceptionAsync(context);
            context.ExceptionHandled = true;
        }

        private static void HandleExceptionAsync(ExceptionContext context)
        {
            var exception = context.Exception;
            if (exception is NotImplementedException)
                SetExceptionResult(context, exception, HttpStatusCode.NotImplemented);
            else if (exception is AuthenticationException)
                SetExceptionResult(context, exception, HttpStatusCode.Unauthorized);
            else if (exception is Exception)
                SetExceptionResult(context, exception, HttpStatusCode.BadRequest);
            else
                SetExceptionResult(context, exception, HttpStatusCode.InternalServerError);
        }

        private static void SetExceptionResult(
            ExceptionContext context,
            Exception exception,
            HttpStatusCode code)
        {
            context.Result = new JsonResult(new HttpMessageError<Exception>() { IsSuccess = false, Stack = exception.ToString(), Message= exception.Message, StatusCode = HttpStatusCode.BadRequest })
            {
                StatusCode = (int)code
            };
        }
    }
}
