using APForums.Server.Controllers.Services.Data;
using APForums.Server.Data.DTO;
using APForums.Server.Models;
using System.Security.Claims;

namespace APForums.Server.Controllers.Services.Interfaces
{
    public interface IIdentityService
    {

        Task<AuthResponse> GenerateTokenForValidatedUser(int userId, string tpNumber);

        Task<AuthResponse> GetRefreshAsync(RefreshTokenRequest refreshTokenRequest);
        bool IsValidIdFromClaim(ClaimsPrincipal principal, out int userId);
        bool IsValidIdFromClaim(ClaimsPrincipal principal, out int userId, out string role);
        bool IsValidUserFromClaim(ClaimsPrincipal principal, out int userId);
        bool IsValidUserFromClaim(ClaimsPrincipal principal, out int userId, out string role);
        ClaimsPrincipal? ValidateAndGetPrincipalFromJwtToken(string accessToken);
    }
}
