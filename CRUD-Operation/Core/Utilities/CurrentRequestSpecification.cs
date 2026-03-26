using CRUD_Operation.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CRUD_Operation.Core.Utilities
{
    public class CurrentRequestSpecification : ICurrentRequestSpecification
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string? _token;

        public CurrentRequestSpecification(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Token
        {
            get => _token ?? GetTokenFromHeaders();
            set => _token = value;
        }

        private string GetTokenFromHeaders()
        {
            var authHeader = _httpContextAccessor.HttpContext?.Request?.Headers.Authorization.FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return string.Empty;

            return authHeader["Bearer ".Length..].Trim();
        }
    }
}
