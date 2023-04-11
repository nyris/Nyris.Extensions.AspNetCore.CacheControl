using System;
using Microsoft.AspNetCore.Mvc;
using Nyris.Extensions.AspNetCore.CacheControl;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Http;

/// <summary>
/// Extension methods for <see cref="HttpContext"/>.
/// </summary>
public static class CacheControlHttpContextExtensions
{
    /// <summary>
    /// Gets the <see cref="ICacheControl"/> feature from the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/>.</param>
    /// <returns>An <see cref="ICacheControl"/> instance.</returns>
    /// <exception cref="InvalidOperationException">The <see cref="ICacheControl"/> feature was not registered on the <see cref="HttpContext"/>.</exception>
    public static ICacheControl GetRequestCacheControl(this HttpContext context)
    {
        var control = context.Features.Get<ICacheControl?>();
        if (control == null)
        {
            throw new InvalidOperationException(
                $"The {nameof(ICacheControl)} feature was not registered. " +
                $"Make sure the {nameof(UseRequestCacheControlMiddleware)} is added to the request pipeline.");
        }
        return control;
    }
}
