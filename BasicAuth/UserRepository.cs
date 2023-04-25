namespace rmsbe.BasicAuth;

public class UserRepository : IUserRepository
{
    private readonly IList<User> _users = new List<User>
    {
        new()
        {
            Username = Configs.Users.AdminUser,
            Password = Configs.Users.AdminPassword
        }
    };

    public async Task<bool> Authenticate(string username, string password)
    {
        return await Task.FromResult(_users.SingleOrDefault(x => x.Username == username && x.Password == password)) != null;
    }

    public async Task<IList<string>> GetUserNames()
    {
        return _users.Select(x => x.Username).ToList();
    }
}