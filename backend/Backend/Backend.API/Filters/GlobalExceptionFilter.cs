using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Backend.Core.CustomEntities;

namespace Backend.API.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            List<Message> messages = [];

            var exception = context.Exception;

            var message = new Message("-1");
            message.SetText("Ocurrió un error al procesar la petición nuevo mensaje");
            messages.Add(message);

            Console.WriteLine("\n\nNo Controlado:");
            Console.WriteLine(exception);
            _logger.LogCritical("Error no controlado: {1}", exception.ToString());

            var result = Result.Fail("");
            result.Messages = messages;

            context.Result = new OkObjectResult(result);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            context.ExceptionHandled = true;
        }
    }
}
