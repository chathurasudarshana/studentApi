namespace SCH.Core.ErrorHandling
{
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using SCH.Shared.Exceptions;
    using System.Net;

    public class AppExceptionHandler : IExceptionHandler
    {
        private readonly IConfiguration configuration;

        public AppExceptionHandler(IConfiguration configuration) 
        { 
            this.configuration = configuration; 
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            AppErrorContent appErrorContent;
            HttpStatusCode statusCode;

            if (exception is SCHException)
            {

                appErrorContent = new AppErrorContent
                {
                    Message = exception.Message
                };


                SCHException sCHException = (SCHException)exception;
                SCHExceptionTypes expType = sCHException.SCHExceptionType;

                statusCode = HttpStatusCode.InternalServerError;
                if (sCHException is SCHDomainException)
                {
                    statusCode =
                        expType == SCHExceptionTypes.Conflict ? HttpStatusCode.Conflict :
                        expType == SCHExceptionTypes.NotFound ? HttpStatusCode.NotFound :
                        expType == SCHExceptionTypes.BadRequest ? HttpStatusCode.BadRequest :
                        expType == SCHExceptionTypes.Forbidden ? HttpStatusCode.Forbidden :
                        HttpStatusCode.Conflict;
                }
                else if (sCHException is SCHApplicationException)
                {
                    statusCode =
                        expType == SCHExceptionTypes.InternalServerError 
                            ? HttpStatusCode.InternalServerError :
                        expType == SCHExceptionTypes.ServiceUnavailable 
                            ? HttpStatusCode.ServiceUnavailable :
                        HttpStatusCode.InternalServerError;
                }

            }
            else
            {

                statusCode = HttpStatusCode.InternalServerError;

                string? defaultLogLevel = configuration["AppSettings:HideResponseErrors"];
                if (defaultLogLevel == true.ToString())
                {
                    const string criticalErrorMessage = "CRITICAL ERROR(S) OCCURRED";

                    appErrorContent = new AppErrorContent 
                    { 
                        Message = criticalErrorMessage 
                    };
                }
                else
                {
                    appErrorContent = new AppErrorContent
                    {
                        Message = exception.Message,
                        Trace = exception.ToString()
                    };
                }
            }

            httpContext.Response.StatusCode = ((int)statusCode);
            await httpContext.Response
                .WriteAsJsonAsync(appErrorContent);

            return true;
        }
    }
}
