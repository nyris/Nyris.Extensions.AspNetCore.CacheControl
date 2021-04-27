using System;
using Microsoft.AspNetCore.Mvc;
using Nyris.Extensions.AspNetCore.CacheControl;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Http
{
    /// <summary>
    /// Extension methods for <see cref="HttpContext"/>.
    /// </summary>
    public static class CacheControlHttpContextExtensions
    {
        /// <summary>
        /// Gets the <see cref="IRequestCacheControl"/> feature from the <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/>.</param>
        /// <returns>An <see cref="IRequestCacheControl"/> instance.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="IRequestCacheControl"/> feature was not registered on the <see cref="HttpContext"/>.</exception>
        public static IRequestCacheControl GetRequestCacheControl(this HttpContext context)
        {
            var control = context.Features.Get<IRequestCacheControl?>();
            if (control == null)
            {
                throw new InvalidOperationException(
                    $"The {nameof(IRequestCacheControl)} feature was not registered. " +
                    $"Make sure the {nameof(UseRequestCacheControlAttribute)} is added to the actions or controllers.");
            }
            return control;
        }
    }
}
