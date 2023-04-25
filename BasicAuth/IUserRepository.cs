namespace rmsbe.BasicAuth;

public interface IUserRepository
{
    Task<bool> Authenticate(string username, string password);
    Task<IList<string>> GetUserNames();
}