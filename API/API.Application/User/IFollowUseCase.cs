namespace API.Application.User;

public interface IFollowUseCase
{
    Task UnfollowUserAsync(long followerId, long followeeId, CancellationToken ct = default);
    Task FollowUserAsync(long followerId, long followeeId, CancellationToken ct = default);
}