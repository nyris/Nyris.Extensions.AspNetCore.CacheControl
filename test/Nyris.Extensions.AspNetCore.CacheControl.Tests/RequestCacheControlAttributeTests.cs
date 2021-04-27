using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace Nyris.Extensions.AspNetCore.CacheControl.Tests
{
    public sealed class RequestCacheControlAttributeTests
    {
        private readonly ActionExecutingContext _context;
        private readonly ActionExecutionDelegate _next;

        public RequestCacheControlAttributeTests()
        {
            var controller = new Mock<Controller>().Object;
            var httpContext = new DefaultHttpContext();

            var actionContext = new ActionContext
            {
                HttpContext = httpContext,
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor(),
            };

            var metadata = new List<IFilterMetadata>();

            _context = new ActionExecutingContext(
                actionContext,
                metadata,
                new Dictionary<string, object>(),
                new Mock<Controller>().Object);

            _next = () => Task.FromResult(new ActionExecutedContext(actionContext, metadata, controller));
        }

        public static IEnumerable<object[]> TestData
        {
            get
            {
                yield return new object[]
                {
                    default(HeaderEntry),
                    new UseRequestCacheControlAttribute.RequestCacheControl()
                };

                yield return new object[]
                {
                    new HeaderEntry("cache-control", "no-store"),
                    new UseRequestCacheControlAttribute.RequestCacheControl
                    {
                        HeaderUsed = true,
                        NoStore = true,
                        Directives = { "no-store" }
                    }
                };

                yield return new object[]
                {
                    new HeaderEntry("cache-control", "no-cache,no-store,no-transform,only-if-cached"),
                    new UseRequestCacheControlAttribute.RequestCacheControl
                    {
                        HeaderUsed = true,
                        NoStore = true,
                        NoCache = true,
                        NoTransform = true,
                        OnlyIfCached = true,
                        Directives = { "no-cache", "no-store", "no-transform", "only-if-cached" }
                    }
                };

                yield return new object[]
                {
                    new HeaderEntry("cache-control", "max-age=0,min-fresh=60,max-stale=42"),
                    new UseRequestCacheControlAttribute.RequestCacheControl
                    {
                        HeaderUsed = true,
                        MaxAge = TimeSpan.Zero,
                        MinFresh = TimeSpan.FromSeconds(60),
                        MaxStale = TimeSpan.FromSeconds(42),
                        Directives = { "max-age=0", "min-fresh=60", "max-stale=42" }
                    }
                };

                yield return new object[]
                {
                    new HeaderEntry("cache-control", "max-age = 123"),
                    new UseRequestCacheControlAttribute.RequestCacheControl
                    {
                        HeaderUsed = true,
                        MaxAge = TimeSpan.FromSeconds(123),
                        Directives = { "max-age=123" }
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public async Task CacheHeaders_AreParsedCorrectly(HeaderEntry header, IRequestCacheControl expected)
        {
            // arrange
            if (!string.IsNullOrWhiteSpace(header.Name))
            {
                _context.HttpContext.Request.Headers.Add(header.Name, header.Value);
            }

            var attribute = new UseRequestCacheControlAttribute();

            // act
            await attribute.OnActionExecutionAsync(_context, _next);

            // assert
            var cacheControl = _context.HttpContext.GetRequestCacheControl();
            cacheControl.Should().NotBeNull("because we expect to either get an instance or crash");
            cacheControl.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task MultipleHeaders_AreParsedCorrectly()
        {
            // arrange
            _context.HttpContext.Request.Headers.Add("Pragma", "no-cache");
            _context.HttpContext.Request.Headers.Add("Cache-Control",
                new StringValues(new [] {"no-store", "max-age=0"}));

            var attribute = new UseRequestCacheControlAttribute();

            // act
            await attribute.OnActionExecutionAsync(_context, _next);

            // assert
            var cacheControl = _context.HttpContext.GetRequestCacheControl();
            cacheControl.Should().NotBeNull("because we expect to either get an instance or crash");

            cacheControl.HeaderUsed.Should().BeTrue();
            cacheControl.NoCache.Should().BeTrue();
            cacheControl.NoStore.Should().BeTrue();
            cacheControl.MaxAge.Should().Be(TimeSpan.Zero);

            cacheControl.OnlyIfCached.Should().BeFalse();
            cacheControl.MaxStale.Should().BeNull();
            cacheControl.MinFresh.Should().BeNull();
        }

        [Fact]
        public void GetCacheControl_WhenAttributeIsNotUsed_Throws()
        {
            // arrange
            var httpContext = new DefaultHttpContext();
            var context = new ActionExecutingContext(
                new ActionContext(
                    httpContext: httpContext,
                    routeData: new RouteData(),
                    actionDescriptor: new ActionDescriptor(),
                    modelState: new ModelStateDictionary()
                ),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object);

            // act
            Action getCacheControl = () => context.HttpContext.GetRequestCacheControl();

            // assert
            getCacheControl.Should().Throw<InvalidOperationException>("because the expected attribute was not registered");
        }

        public readonly struct HeaderEntry
        {
            public readonly string Name;
            public readonly string Value;

            public HeaderEntry(string name, string value)
            {
                Name = name;
                Value = value;
            }

            /// <inheritdoc />
            public override string ToString() => string.IsNullOrWhiteSpace(Name) ? "(none)" : $"{Name}: {Value}";
        }
    }
}
