namespace Nyris.Extensions.AspNetCore.CacheControl;

/// <summary>
///     Extension methods for <see cref="ICacheControl"/>.
/// </summary>
public static class CacheControlExtensions
{
    /// <summary>
    ///     Gets a value indicating whether the use of a cache for retrieving
    ///     values is allowed.
    /// </summary>
    /// <param name="control">The <see cref="ICacheControl"/> instance.</param>
    /// <returns><see langword="true">true</see> if the use of a cache for retrieving values is allowed; <see langword="true">false</see> otherwise.</returns>
    /// <seealso cref="ICacheControl.NoCache"/>
    public static bool AllowCacheUse(this ICacheControl control) => !control.NoCache;

    /// <summary>
    ///     Gets a value indicating whether any cache involved may be updated
    ///     with newer information.
    /// </summary>
    /// <param name="control">The <see cref="ICacheControl"/> instance.</param>
    /// <returns><see langword="true">true</see> if updating the cache with newer values is allowed; <see langword="true">false</see> otherwise.</returns>
    /// <seealso cref="ICacheControl.NoStore"/>
    public static bool AllowCacheUpdate(this ICacheControl control) => !control.NoStore;
}
