using Microsoft.AspNetCore.Http;

namespace CRUD_Operation.Middleware
{
    /// <summary>
    /// Middleware that checks for a valid Bearer token on protected API routes.
    /// Returns 401 if the request targets a protected path and the user is not authenticated.
    /// </summary>
    public class JwtTokenCheckMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Path prefixes that do not require authentication.
        /// </summary>
        private static readonly PathString[] AnonymousPaths =
        {
            new PathString("/api/auth"),
            new PathString("/api/health"),
            new PathString("/swagger"),
            new PathString("/favicon.ico")
        };

        public JwtTokenCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (IsAnonymousPath(context.Request.Path))
            {
                await _next(context);
                return;
            }

            // Only check for API routes
            if (!context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    message = "Unauthorized. Valid Bearer token is required.",
                    statusCode = 401
                });
                return;
            }

            await _next(context);
        }

        private static bool IsAnonymousPath(PathString path)
        {
            foreach (var prefix in AnonymousPaths)
            {
                if (path.StartsWithSegments(prefix, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
