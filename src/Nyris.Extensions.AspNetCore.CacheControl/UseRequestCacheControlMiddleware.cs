using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Nyris.Extensions.AspNetCore.CacheControl;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Mvc;

/// <summary>
///     Enables interpretation of the <c>Cache-Control</c> request header.
/// </summary>
public sealed class UseRequestCacheControlMiddleware
{
    private static readonly Regex Whitespace = new(@"\s+", RegexOptions.Compiled);

    private static readonly Regex Durations = new(
        @"^\s*(?<name>max-age|max-stale|min-fresh)\s*=\s*(?<seconds>\d+)\s*$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline |
        RegexOptions.ExplicitCapture);

    private readonly RequestDelegate _next;

    public UseRequestCacheControlMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task InvokeAsync(HttpContext context)
    {
        // TODO: Investigate whether HTTP/2 trailers need to be evaluated as well.
        var headers = context.Request.Headers;

        var control = new RequestCacheControl();
        context.Features.Set((ICacheControl) control);

        if (TryExtractDirectives("pragma", headers, out var directives))
        {
            control.HeaderUsed = true;
            foreach (var directive in directives)
            {
                control.Directives.Add(directive);
                TryParsePragmaDirective(directive, control);
            }
        }

        if (TryExtractDirectives("cache-control", headers, out directives))
        {
            control.HeaderUsed = true;
            foreach (var directive in directives)
            {
                control.Directives.Add(directive);
                TryParseControlDirective(directive, control);
                TryParseDurationsDirective(directive, control);
            }
        }

        return _next(context);
    }

    private static void TryParsePragmaDirective(string directive, RequestCacheControl control)
    {
        control.NoCache |= string.Equals("no-cache", directive, StringComparison.OrdinalIgnoreCase);
    }

    private static void TryParseControlDirective(string directive, RequestCacheControl control)
    {
        control.NoCache |= string.Equals("no-cache", directive, StringComparison.OrdinalIgnoreCase);
        control.NoStore |= string.Equals("no-store", directive, StringComparison.OrdinalIgnoreCase);
        control.OnlyIfCached |= string.Equals("only-if-cached", directive, StringComparison.OrdinalIgnoreCase);
        control.NoTransform |= string.Equals("no-transform", directive, StringComparison.OrdinalIgnoreCase);
    }

    private static void TryParseDurationsDirective(string directive, RequestCacheControl control)
    {
        var durationsMatch = Durations.Match(directive);
        if (!durationsMatch.Success || !int.TryParse(durationsMatch.Groups["seconds"].Value, out var seconds))
        {
            return;
        }

        var name = durationsMatch.Groups["name"].Value;
        if (string.Equals("max-age", name, StringComparison.OrdinalIgnoreCase))
        {
            control.MaxAge = TimeSpan.FromSeconds(seconds);
        }
        else if (string.Equals("max-stale", name, StringComparison.OrdinalIgnoreCase))
        {
            control.MaxStale = TimeSpan.FromSeconds(seconds);
        }
        else if (string.Equals("min-fresh", name, StringComparison.OrdinalIgnoreCase))
        {
            control.MinFresh = TimeSpan.FromSeconds(seconds);
        }
    }

    private static bool TryExtractDirectives(string headerName, IHeaderDictionary? headers,
        [NotNullWhen(true)] out HashSet<string>? directives)
    {
        directives = null;
        if (headers == null)
        {
            return false;
        }

        return headers.TryGetValue(headerName, out var values) && TryExtractDirectives(values, out directives);
    }

    private static bool TryExtractDirectives(StringValues? values, [NotNullWhen(true)] out HashSet<string>? directives)
    {
        if (!values.HasValue)
        {
            directives = null;
            return false;
        }

        directives = values.Value
            .SelectMany(static value => value?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? ArraySegment<string>.Empty)
            .Where(static value => !string.IsNullOrWhiteSpace(value))
            .Select(static directive =>
                Whitespace.Replace(directive, string.Empty))
            .ToHashSet();
        return true;
    }

    // ReSharper disable once MemberCanBePrivate.Global
    internal sealed class RequestCacheControl : ICacheControl
    {
        /// <inheritdoc />
        public bool HeaderUsed { get; set; }

        /// <inheritdoc />
        public bool NoCache { get; set; }

        /// <inheritdoc />
        public bool NoStore { get; set; }

        /// <inheritdoc />
        public bool OnlyIfCached { get; set; }

        /// <inheritdoc />
        public bool NoTransform { get; set; }

        /// <inheritdoc />
        public TimeSpan? MaxAge { get; set; }

        /// <inheritdoc />
        public TimeSpan? MaxStale { get; set; }

        /// <inheritdoc />
        public TimeSpan? MinFresh { get; set; }

        /// <summary>
        /// Gets or sets all provided cache-control directives.
        /// </summary>
        internal HashSet<string> Directives { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <inheritdoc />
        IReadOnlyCollection<string> ICacheControl.Directives => Directives;
    }
}
