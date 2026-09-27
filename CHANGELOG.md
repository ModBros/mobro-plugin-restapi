# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## 1.1.1 - Unreleased

### Added

- German translation of the plugin settings

### Changed

- Requests are only accepted if addressed to `localhost`, `127.0.0.1` or `[::1]`, which protects against DNS rebinding attacks from websites
- Creating a metric with an id that already exists returns `409 Conflict` instead of `400 Bad Request`

### Fixed

- Metrics and their values are persisted shortly after every change instead of only on shutdown, so they are no longer lost after a crash or power loss
- `GET /api/v1/metrics/{id}` now includes `valueUpdated` like the other endpoints
- `PUT /api/v1/metrics/{id}/value` validates the value like metric creation: arrays and objects are rejected with `400 Bad Request` instead of silently becoming `null`, and strings are limited to 128 characters
- The API server no longer picks up configuration files, environment variables or console logging of the MoBro service; its logs go to the plugin log
- Conflict and some bad request error responses use camelCase property names like all other responses
- Example metric ids in the API documentation are consistent (`t1`)
- Swagger setting description no longer refers to a fixed port and Swagger defaults to enabled, as declared in the plugin settings

## 1.1.0 - 2026-02-16

### Changed

- Updated to .NET 10
- Updated to SDK v1.1.0
- Updated to FastEndpoints 7

## 1.0.0 - 2025-06-09

### Added

- Initial release
