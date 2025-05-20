namespace Backend.API.Delegates
{
    public static class AppBuilderFunctions
    {
        public static Task AddSecurityHeaders(HttpContext context, Func<Task> next)
        {
            if (!context.Response.Headers.ContainsKey("X-Xss-Protection"))
            {
                context.Response.Headers.Append("X-Xss-Protection", "1; mode=block");
            }

            if (!context.Response.Headers.ContainsKey("X-Content-Type-Options"))
            {
                context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            }

            if (!context.Response.Headers.ContainsKey("X-Content-Type-Options"))
            {
                context.Response.Headers.Append("Referrer-Policy", "no-referrer");
            }

            return next.Invoke();
        }

        public static Task RejectForbiddenMethods(HttpContext context, Func<Task> next)
        {
            context.Response.StatusCode = context.Request.Method switch
            {
                "OPTIONS" => 405,
                "PUT" => 405,
                "DELETE" => 405,
                "PATCH" => 405,
                "HEAD" => 405,
                _ => context.Response.StatusCode
            };

            return next.Invoke();
        }
    }
}
