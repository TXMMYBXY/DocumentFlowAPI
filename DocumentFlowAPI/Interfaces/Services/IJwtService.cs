namespace DocumentFlowAPI.Interfaces.Services;

public interface IJwtService
{
    string GenerateToken(Models.User user);
    bool ValidateToken(string token);
    int GetUserIdFromToken(string token);
    string GetUserEmailFromToken(string token);
}
