namespace DocumentFlowAPI.Configuration;

/// <summary>
/// Класс настроек для токенов JWT и токенов обновления
/// </summary>
public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public int ExpiresDays { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiresMinutes { get; set; }
}
