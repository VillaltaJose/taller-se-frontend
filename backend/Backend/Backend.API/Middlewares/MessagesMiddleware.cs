using Backend.Core.CustomEntities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Backend.API.Middlewares
{
    public class MessagesMiddleware : IMiddleware
    {
        private readonly ILogger<MessagesMiddleware> _logger;

        public MessagesMiddleware(
            ILogger<MessagesMiddleware> logger
        )
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            using var responseBodyStream = new MemoryStream();
            var bodyStream = context.Response.Body;

            try
            {
                context.Response.Body = responseBodyStream;

                await next(context);

                responseBodyStream.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();

                if (context.Response.ContentType != null &&
                    context.Response.ContentType.Contains("application/json",
                        StringComparison.CurrentCultureIgnoreCase) && IsResultType(responseBody))
                {
                    var modifiedResponse = await ModifyResultResponse(responseBody);
                    responseBody = modifiedResponse;
                }

                using var newStream = new MemoryStream();
                var sw = new StreamWriter(newStream);
                await sw.WriteAsync(responseBody);
                await sw.FlushAsync();
                context.Response.ContentLength = newStream.Length;
                newStream.Seek(0, SeekOrigin.Begin);

                await newStream.CopyToAsync(bodyStream);
            }
            finally
            {
                context.Response.Body = bodyStream;
            }
        }

        private static bool IsResultType(string responseBodyText)
        {
            try
            {
                return JsonConvert.DeserializeObject<JObject>(responseBodyText.ToLower())!
                    .ContainsKey("success");
            }
            catch
            {
                return false;
            }
        }

        private async Task<string> ModifyResultResponse(string responseBodyText)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<Result<dynamic>>(responseBodyText);

                if (result?.Messages is not { Count: > 0 }) return JsonConvert.SerializeObject(result);

                foreach (var message in result.Messages)
                {
                    if (string.IsNullOrWhiteSpace(message.Code)) continue;

                    message.SetText(message.Code);
                }

                return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Error al obtener mensaje: {Exception}", ex);
                return responseBodyText;
            }
        }
    }
}
