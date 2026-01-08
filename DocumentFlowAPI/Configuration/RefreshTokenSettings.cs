namespace DocumentFlowAPI.Configuration;

/// <summary>
/// Класс настроек для рефреш токенов
/// </summary>
public class RefreshTokenSettings
{
    public string SecretKey { get; set; }
    public int ExpiresDays { get; set; }
}
