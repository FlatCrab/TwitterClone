
namespace API.Core.User.interfaces;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(long id);
    Task<User> GetUserByAccountNameAsync(string accountname);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<IEnumerable<User>> SearchUsersByUsernameAsync(string searchTerm);
    Task<IEnumerable<User>> GetFriendRecommendationsAsync(long userId, int count,int depth);
    Task<IEnumerable<User>> GetFriendsOfUserAsync(long userId);
    Task<IEnumerable<User>> GetMutualFriendsAsync(long userId1, long userId2);
    Task<IEnumerable<User>> GetUsersWithBirthdayTodayAsync();
    Task<IEnumerable<User>> GetFollowersAsync(long userId);
    Task<IEnumerable<User>> GetFollowingAsync(long userId);
    Task FollowUserAsync(long followerId, long followeeId);
    Task UnfollowUserAsync(long followerId, long followeeId);
    Task<User> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task DeleteUserAsync(long id);
}