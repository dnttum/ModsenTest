namespace ModsenTestEvent.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    
    Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    
    Task<bool> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken);
    
    Task<bool> IsContactsUniqueAsync(string contacts, CancellationToken cancellationToken);
    
    Task AddAsync(User user, CancellationToken cancellationToken);
    
    Task UpdateAsync(User user, CancellationToken cancellationToken);
}