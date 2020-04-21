using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DFC.App.Account.Services
{
    public static class ErrorService
    {
        public static async Task LogException(HttpContext context, ILogger logger)
        {
            var exception =
                context.Features.Get<IExceptionHandlerPathFeature>();

            logger.Log(LogLevel.Error, $"Accounts Error: {exception.Error.Message} \r\n" +
                                       $"Path: {exception.Path} \r\n)");
        }
    }
}
