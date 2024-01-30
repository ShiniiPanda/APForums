namespace APForums.Server.Controllers.Services.Data
{
    public class AuthResponse
    {

        public string AccessToken { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;

        public AuthStatus Status { get; set; }

    }

    public enum AuthStatus
    {
        Successful,
        InvalidPrincipal,
        Expired,
        NotExpired,
        NotFound,
        AccessRevoked,
        NotMatched,
        Used,
        MissingContext
    }
}
