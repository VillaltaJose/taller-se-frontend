using Backend.Core.App;

namespace Backend.API.Middlewares
{
    public class MetaDataMiddleware : IMiddleware
    {
        private readonly MetaData _metaData;

        public MetaDataMiddleware(
            MetaData metaData
        )
        {
            _metaData = metaData;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var ipAddress = context.Connection.RemoteIpAddress;

            _metaData.IpAcceso = ipAddress.ToString();

            await next(context);
        }
    }
}
