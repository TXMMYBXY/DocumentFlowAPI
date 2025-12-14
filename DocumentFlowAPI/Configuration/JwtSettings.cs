namespace DocumentFlowAPI.Configuration;

public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public int ExpiresDays { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiresMinutes { get; set; }
}
