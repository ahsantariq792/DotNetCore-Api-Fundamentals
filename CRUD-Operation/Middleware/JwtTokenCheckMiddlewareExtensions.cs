namespace CRUD_Operation.Middleware
{
    public static class JwtTokenCheckMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtTokenCheck(this IApplicationBuilder app)
        {
            return app.UseMiddleware<JwtTokenCheckMiddleware>();
        }
    }
}
