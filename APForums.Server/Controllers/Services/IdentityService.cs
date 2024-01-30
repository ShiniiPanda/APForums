using APForums.Server.Controllers.Services.Data;
using APForums.Server.Controllers.Services.Interfaces;
using APForums.Server.Data;
using APForums.Server.Data.DTO;
using APForums.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APForums.Server.Controllers.Services
{
    public class IdentityService : IIdentityService
    {

        public static readonly string AdminRoleClaimName = "admin";

        public static readonly string MemberRoleClaimName = "member";

        public static readonly string RoleClaim = "role";

        private List<string> AdminTPs = new List<string>() { "TP000111", "TP000222" };

        private readonly TokenValidationParameters _tokenValidationParameters;

        private readonly IConfiguration _config;

        private readonly ForumsDbContext _context;

        public IdentityService(IConfiguration config, TokenValidationParameters tokenValidationParameters, ForumsDbContext context)
        {
            _config = config;
            _tokenValidationParameters = tokenValidationParameters;
            _context = context;
        }

        public async Task<AuthResponse> GenerateTokenForValidatedUser(int userId, string tpNumber)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            string role = MemberRoleClaimName;
            if (_config == null)
            {
                Console.WriteLine("Sike");
                return new AuthResponse
                {
                    Status = AuthStatus.MissingContext
                };
            }
            if (AdminTPs.Contains(tpNumber))
            {
                role = AdminRoleClaimName;
            }

            var key = Encoding.ASCII.GetBytes(_config["JWTSettings:JWTSecret"]!);
            var signingKey = new SymmetricSecurityKey(key);
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.NameId, userId.ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName, tpNumber),
                        new Claim("role", role)
                    }
                ),
                Audience = _config["JWTSettings:JWTAudience"]!,
                Issuer = _config["JWTSettings:JWTIssuer"]!,
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)

            };

            var token = tokenHandler.CreateJwtSecurityToken(descriptor);

            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                JwtId = token.Id,
                UserId = userId,
                Created = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(3)
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            var response = new AuthResponse
            {
                AccessToken = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token,
                Status = AuthStatus.Successful
            };

            return response;
        }

        public async Task<AuthResponse> GetRefreshAsync(RefreshTokenRequest refreshTokenRequest) 
        {
            Console.WriteLine("Received Access Token " + refreshTokenRequest.AccessToken + "\n");
            Console.WriteLine("Received Refresh Token " + refreshTokenRequest.RefreshToken + "\n");
            var principal = ValidateAndGetPrincipalFromJwtToken(refreshTokenRequest.AccessToken);
            if (principal == null)
            {
                Console.WriteLine("Principal Is Null!!");
                return new AuthResponse
                {
                    Status = AuthStatus.InvalidPrincipal
                };
            }
            var expiryDateSeconds = long.Parse(principal.Claims.Single(t => t.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateSeconds);

            if (expiryDateUtc >  DateTime.UtcNow)
            {
                Console.WriteLine("Token has not expired yet!!");
                return new AuthResponse
                {
                    Status = AuthStatus.NotExpired
                };
            }

            var jti = principal.Claims.Single(t => t.Type == JwtRegisteredClaimNames.Jti).Value;
            var storedToken = await _context.RefreshTokens.SingleOrDefaultAsync(rt => rt.Token == refreshTokenRequest.RefreshToken);

            if (storedToken == null)
            {
                return new AuthResponse
                {
                    Status = AuthStatus.NotFound
                };
            }

            if (DateTime.UtcNow > storedToken.ExpiryDate)
            {
                return new AuthResponse
                {
                    Status = AuthStatus.Expired
                };
            }

            if (storedToken.Invalid)
            {
                return new AuthResponse
                {
                    Status = AuthStatus.AccessRevoked
                };
            }

            if (storedToken.Used)
            {
                return new AuthResponse
                {
                    Status = AuthStatus.Used
                };
            }

            if (storedToken.JwtId != jti)
            {
                return new AuthResponse
                {
                    Status = AuthStatus.NotMatched
                };
            }

            storedToken.Used = true;
            _context.RefreshTokens.Update(storedToken);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(int.Parse(principal.Claims.Single(t => t.Type == ClaimTypes.NameIdentifier).Value));

            if (user == null)
            {
                return new AuthResponse
                {
                    Status = AuthStatus.NotFound
                };
            }

            return await GenerateTokenForValidatedUser(user.Id, user.TPNumber);

        }

        public bool IsValidIdFromClaim(ClaimsPrincipal principal, out int userId)
        {
            userId = -1;
            if (principal == null)
            {
                return false;
            }
            var claimId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(claimId))
            {
                return false;
            }
            try
            {
                userId = int.Parse(claimId);
                return true;
            } catch
            {
                return false;
            }
        }

        public bool IsValidIdFromClaim(ClaimsPrincipal principal, out int userId, out string role)
        {
            userId = -1;
            role = "member";
            if (principal == null)
            {
                return false;
            }
            var claimId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(claimId))
            {
                return false;
            }
            try
            {
                userId = int.Parse(claimId);
                var roleClaim = principal.FindFirst(c => c.Type == RoleClaim)?.Value;
                if (roleClaim != null)
                {
                    role = roleClaim;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsValidAdminClaim(ClaimsPrincipal principal, out int adminId)
        {
            adminId = -1;
            if (principal == null)
            {
                return false;
            }
            var role = principal.FindFirst(c => c.Type == RoleClaim)?.Value;
            if (string.IsNullOrEmpty(role))
            {
                return false;
            }
            if (!role.Equals(AdminRoleClaimName))
            {
                return false;
            }
            var id = principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }
            try
            {
                adminId = int.Parse(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsValidUserFromClaim(ClaimsPrincipal principal, out int userId)
        {
            userId = -1;
            if (!IsValidIdFromClaim(principal, out int id))
            {
                return false;
            }
            userId = id;
            return _context.Users.Where(u => u.Id == id).Any();
        }

        public bool IsValidUserFromClaim(ClaimsPrincipal principal, out int userId, out string role)
        {
            role = "member";
            userId = -1;
            if (!IsValidIdFromClaim(principal, out int id))
            {
                return false;
            }
            userId = id;
            var roleClaim = principal.FindFirst(c => c.Type == RoleClaim)?.Value;
            if (roleClaim != null)
            {
                role = roleClaim;
            }
            return _context.Users.Where(u => u.Id == id).Any();
        }

        public ClaimsPrincipal? ValidateAndGetPrincipalFromJwtToken(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var validationParameters = _tokenValidationParameters;
                validationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(accessToken, validationParameters, out var validToken);
                if ((validToken is JwtSecurityToken jwtToken) && (jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))) {
                    return principal;
                } else
                {
                    return null;
                }
            } catch (Exception ex) 
            {
                Console.WriteLine("ERROR RECEIVED: " + ex.Message);
                return null;
            }
        }


        
    }
}
