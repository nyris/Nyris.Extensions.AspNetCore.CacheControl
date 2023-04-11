using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

/// <summary>
///     Extension methods for <see cref="IApplicationBuilder"/> for registering <c>Cache-Control</c>
///     header interpretation..
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    ///     Registers the <see cref="UseRequestCacheControlMiddleware"/> with the application.
    /// </summary>
    /// <param name="app">The application to register the middleware in.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> for method chaining.</returns>
    public static IApplicationBuilder UseCacheControlMiddleware(this IApplicationBuilder app) =>
        app.UseMiddleware(typeof(UseRequestCacheControlMiddleware));
}
