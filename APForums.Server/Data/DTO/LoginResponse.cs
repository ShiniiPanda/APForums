namespace APForums.Server.Data.DTO
{
    public class LoginResponse
    {
        public string AccessToken { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;
    }
}
