# Changelog

All notable changes to this project will be documented in this file.
This project uses [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## 0.4.2 - 2023-04-11

### Internal

- Fixed the missing deployment of the `Abstractions` package.

## 0.4.1 - 2023-04-11

### Added

- Added the `UseCacheControlMiddleware` extension for `IApplicationBuilder`.

## 0.4.0 - 2023-04-11

### Changed

- The `IRequestCacheControl` interface was renamed to `ICacheControl`.
- Replaced the `UseRequestCacheControlAttribute` with a `UseRequestCacheControl` middleware.

### Added

- The `AllowCacheUse` and `AllowCacheUpdate` extension methods were added
  to simplify the otherwise required negations of `NoCache` and `NoStore`.

## 0.3.0 - 2022-11-09

### Added

- Added support for .NET 7.0.

## 0.2.0 - 2021-04-27

### Added

- Added support for .NET 6.0.

## 0.1.0 - 2021-04-27

### Added

- Initial release. 🎉
