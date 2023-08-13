# ASP.NET Core Caching Extensions

Provides the `HttpContext.GetRequestCacheControl()` extension method for accessing 
the HTTP/1.1 `Cache-Control` and HTTP/1.0 `Pragma` header directives.

## Usage example

Register the Cache-Control middleware on your `IApplicationBuilder`:

```csharp
app.UseCacheControlMiddleware();
```

From within your action, use the `GetRequestCacheControl` extension
method on the HTTP context to get the Cache-Control header information:

```csharp
ICacheControl cacheControl = HttpContext.GetRequestCacheControl();
```
