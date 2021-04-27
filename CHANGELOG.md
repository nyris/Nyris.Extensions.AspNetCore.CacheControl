# Changelog

All notable changes to this project will be documented in this file.
This project uses [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## 0.3.0 - 2021-04-23

### Added

- Added support for the `Cache-Control` request directives `no-cache`, `no-store`
  and `only-if-cached`.

## 0.2.1 - 2021-04-23

### Internal

- The service now uses HTTP/2 to communicate with the Find API on port `51339`
  by default.
- The `cloud.google.com/app-protocols` and `service.alpha.kubernetes.io/app-protocols`
  were added to the Kubernetes integration service.

## 0.2.0 - 2021-04-23

### Fixed

- Fixed an issue where the use of `SendAsync` on the HTTP client
  would discard the configured default HTTP version.

### Internal

- Changed the default span propagation format to B3.
- The ASP.NET Core `TraceIdentifier` is now reported as the `aspnetcore.trace-identifier` tag.
- Added the `http.request_content_length`, `http.response_content_length`,
  `http.user_agent` and `http.status_code_family` trace tags.

## 0.1.3 - 2021-04-22

### Added

- The `application/step`, `model/stl`, `model/obj`, `model/gltf` and
  `model/gltf-binary` content types are now reported as accepted.

### Fixed

- Fixes an issue with stream length validation when the file was
  received via `application/octet-stream`.

## 0.1.2 - 2021-04-22

### Changed

- The controllers now resolve as `/cad/find/vX.Y` and `/cad/render/vX.Y`.

## 0.1.1 - 2021-04-22

### Changed

- The `/api` prefix was changed to `/cad` to allow the routes to be
  told apart by the Ingress controller.

## 0.1.0 - 2021-04-22

### Added

- Initial release. 🎉
