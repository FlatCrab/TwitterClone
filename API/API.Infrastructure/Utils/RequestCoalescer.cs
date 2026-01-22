using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace API.Infrastructure.Utils;

public class RequestCoalescer<TKey, TResult> where TKey : notnull
{
    private readonly ConcurrentDictionary<TKey, Lazy<Task<TResult>>> _inflight = new();

    public Task<TResult> ExecuteAsync(TKey key, Func<CancellationToken, Task<TResult>> factory, CancellationToken ct = default)
    {
        var lazy = _inflight.GetOrAdd(key, _ => new Lazy<Task<TResult>>(() => RunAndRemove(key, factory, ct)));
        return lazy.Value;
    }

    private async Task<TResult> RunAndRemove(TKey key, Func<CancellationToken, Task<TResult>> factory, CancellationToken ct)
    {
        try
        {
            return await factory(ct).ConfigureAwait(false);
        }
        finally
        {
            _inflight.TryRemove(key, out _);
        }
    }
}