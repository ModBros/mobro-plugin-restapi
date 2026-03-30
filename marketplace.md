# MoBro REST API

Connect virtually anything to your dashboard.  
This plugin turns MoBro into a data hub, allowing you to push custom metrics from your own scripts, home automation, or
third-party apps via a simple RESTful interface.

---

## Features

- **Metric Management**: Register up to 100 custom metrics and update them on the fly.
- **Persistent Data**: Optionally keep your registered metrics alive even after restarting MoBro.
- **Built-in Documentation**: Interactive **Swagger UI** makes testing and integration seamless.
- **Flexible Integration**: Perfect for Python scripts, Node.js, PowerShell, or even `curl` commands.

---

## Getting Started

1. **Install** the plugin via the MoBro Marketplace.
2. **Configure** your preferred **Port** (default: `8080`).
3. **Confirm**: Open `http://localhost:8080` in your browser to confirm the API is live.
4. **Explore**: Visit the Swagger UI at `http://localhost:8080/swagger` to see all available endpoints.
5. **Push Data**: Start sending `POST` and `PUT` requests to populate your dashboard.

---

## API Reference

| Endpoint                     | Method   | Action                                         |
|:-----------------------------|:---------|:-----------------------------------------------|
| `/api/v1/metrics`            | `POST`   | Register a new custom metric.                  |
| `/api/v1/metrics`            | `GET`    | List all currently registered metrics.         |
| `/api/v1/metrics/{id}`       | `GET`    | Get a specific registired metric.              |
| `/api/v1/metrics/{id}/value` | `PUT`    | Update the current value of a specific metric. |
| `/api/v1/metrics/{id}`       | `DELETE` | Remove a metric from the registry.             |

---

## Settings

| Setting             | Default   | Description                                                     |
|:--------------------|:----------|:----------------------------------------------------------------|
| **Port**            | `8080`    | The local port the API server listens on.                       |
| **Swagger Support** | `Enabled` | Enables the interactive `/swagger` documentation page.          |
| **Persistence**     | `Enabled` | Saves your registered metrics to disk so they survive a reboot. |
