# REST API Plugin for MoBro

Integrate custom metrics into MoBro using a simple REST API.  
This plugin allows you to integrate your own custom metrics and data into MoBro to integrate them into your dashboards.

## Features

* Metric Management
    * Freely register up to 100 new custom metrics
    * Update the value of a registered metric at any time
    * Fetch registered metrics and check their current value
    * Unregister metrics when they are no longer needed
* **API Documentation**: Built-in Swagger documentation for easy API exploration
* **Persistence:** Registered metrics can be persisted across plugin restarts

---

# Getting Started

1. Install the REST API plugin in MoBro.
2. Configure the plugin settings (Port, Swagger support, Persistence).
3. Verify that the plugin is running by navigating to http://localhost:8080 (use the configured port) in your browser.
4. Register and manage your custom metrics via the provided REST API.

## API Endpoints

The plugin exposes the following REST API endpoints:

* `POST /api/v1/metrics`: Register a new metric.
* `GET /api/v1/metrics`: Get all registered metrics.
* `GET /api/v1/metrics/{id}`: Get a specific metric by ID.
* `DELETE /api/v1/metrics/{id}`: Unregister a metric by ID.
* `PUT /api/v1/metrics/{id}/value`: Update the value of a metric by ID.

Detailed API documentation is available via Swagger if enabled in the plugin settings.

---

# Settings

The plugin offers the following configurable settings:

| Setting         | Default | Explanation                                                                        |
|-----------------|---------|------------------------------------------------------------------------------------|
| Port            | 8080    | The port to use for the Rest API                                                   |
| Swagger support | enabled | Whether to enable swagger documentation (http://localhost:8080/swagger)            |
| Persistence     | enabled | Whether to persist registered metrics to make them available across plugin reboots |
