namespace UTP_Web_API.Services.IServices
{
    public interface IJwtService
    {
        string GetJwtToken(int userId, string role);
    }
}
