using CRUD_Operation.Models.Auth;

namespace CRUD_Operation.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse?> SignupAsync(SignupRequest request, CancellationToken cancellationToken = default);
        Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    }
}
