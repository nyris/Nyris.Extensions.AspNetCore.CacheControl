namespace Nyris.Extensions.AspNetCore.CacheControl;

/// <summary>
///     Interface for cache-control instructions.
/// </summary>
public interface ICacheControl
{
    /// <summary>
    ///     Gets a value indicating whether the <c>Cache-Control</c> or cache-related
    ///     <c>Pragma</c> request header was present.
    /// </summary>
    /// <remarks>
    ///     If this property evaluates to <see langword="false"/>, all properties of this
    ///     instance will have their default value.
    /// </remarks>
    bool HeaderUsed { get; }

    /// <summary>
    ///     Gets a value indicating whether the request is forbidden to use the cache.
    /// </summary>
    bool NoCache { get; }

    /// <summary>
    ///     Gets a value indicating whether the request is allowed to use but forbidden to update the cache.
    /// </summary>
    bool NoStore { get; }

    /// <summary>
    ///     Gets a value indicating whether the request is forbidden to proceed if no cached entry existed.
    /// </summary>
    bool OnlyIfCached { get; }

    /// <summary>
    ///     Gets a value indicating whether the request is forbidden to transform upstream
    ///     data, e.g. transparently converting between image file formats.
    /// </summary>
    public bool NoTransform { get; }

    /// <summary>
    ///     Gets a value indicating the maximum age of a resource relative to the time
    ///     of the request.
    /// </summary>
    /// <returns><see langword="null"/> if unset; the maximum age of the resource otherwise.</returns>
    TimeSpan? MaxAge { get; }

    /// <summary>
    ///     Gets a value indicating the maximum duration a resource is allowed to be expired.
    /// </summary>
    /// <returns><see langword="null"/> if unset; the maximum duration a resource is allowed to be expired.</returns>
    TimeSpan? MaxStale { get; }

    /// <summary>
    ///     Gets a value indicating the minimum duration a resource is required to be fresh (i.e. not expired).
    /// </summary>
    /// <returns><see langword="null"/> if unset; the maximum duration a resource is still fresh (i.e. not expired).</returns>
    TimeSpan? MinFresh { get; }

    /// <summary>
    ///     Gets all provided cache-control directives.
    /// </summary>
    IReadOnlyCollection<string> Directives { get; }
}
