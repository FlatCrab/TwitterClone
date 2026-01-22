
namespace API.Core.User.interfaces;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(long id,CancellationToken ct = default);
    Task<User> GetUserByAccountNameAsync(string accountname, CancellationToken ct = default);
    Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken ct = default);
    Task<IEnumerable<User>> SearchUsersByUsernameAsync(string searchTerm, CancellationToken ct = default);
    Task<IEnumerable<User>> GetFriendRecommendationsAsync(long userId, int count,int depth, CancellationToken ct = default);
    Task<IEnumerable<User>> GetFriendsOfUserAsync(long userId, CancellationToken ct = default);
    Task<IEnumerable<User>> GetMutualFriendsAsync(long userId1, long userId2, CancellationToken ct = default);
    Task<IEnumerable<User>> GetUsersWithBirthdayTodayAsync(CancellationToken ct = default);
    Task<IEnumerable<User>> GetFollowersAsync(long userId, CancellationToken ct = default);
    Task<IEnumerable<User>> GetFollowingAsync(long userId, CancellationToken ct = default);
    Task<User> CreateUserAsync(User user, CancellationToken ct = default);
    Task<User> UpdateUserAsync(User user, CancellationToken ct = default);
    Task DeleteUserAsync(long id, CancellationToken ct = default);
}