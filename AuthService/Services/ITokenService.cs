namespace AuthService.Services
{
    public interface ITokenService
    {
        string BuildToken(string username);
    }
}
