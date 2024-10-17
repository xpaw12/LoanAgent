namespace LoanAgent.Application.Common.Security.Jwt.Options;

public class JwtTokenOptions
{
    public const string Key = "JwtToken";
    public string SecretKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int ExpiresIn { get; set; }
}
