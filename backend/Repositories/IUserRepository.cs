public interface IUserRepository
{
    Task<bool> EmailExists(string email);
    Task CreateAsync(User user);
}
