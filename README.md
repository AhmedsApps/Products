# Products Evaluation Task

This repository is an evaluation project demonstrating a simple product management service implemented with:

- Backend: ASP.NET Core (targeting .NET 10) Web API
  - JWT authentication (Bearer tokens)
  - EF Core In-Memory database (for demo/testing)
  - Swagger/OpenAPI for API exploration
- Models project: shared DTO/entity types
- Client: React application (development SPA)

Purpose
- Provide a compact example for assessing backend and frontend integration, authentication, API documentation, and basic CRUD flows.
- Support quick local verification of API behavior and client integration.

How to run (development)
1. Open the solution in Visual Studio or run from the command line.
2. Start the `Products.Server` project (use the `https` profile).
   - HTTPS: `https://localhost:7266`
   - HTTP: `http://localhost:5087`
3. Run the React client (`products.client`) if desired, or use the server-hosted Swagger UI at the server root.

Key endpoints
- POST `api/Auth/Login` — returns JWT for demo credentials (username `admin`, password `password`).
- GET `api/Products/GetProducts` — requires Authorization header `Bearer <token>`; optional `colour` query parameter.
- POST `api/Products/AddProduct` — requires Authorization header.

Notes for evaluators
- The server uses an in-memory database for simplicity and tests.
- Swagger UI is configured to serve at the application root for convenience in this evaluation build.

Project layout
- `Products.Server` — ASP.NET Core Web API and controllers.
- `Products.Models` — shared models (Product, LoginRequestModel).
- `products.client` — React front-end used for manual verification.
- `Products.Test` — unit tests targeting controller behavior.

Testing
- The `Products.Test` project contains unit tests for controller behavior. Run tests with your IDE or `dotnet test`.

Architecture (simple)

Below is a brief description and a simple ASCII diagram showing how this Products service could integrate into a distributed or event-driven microservices architecture.

Principles
- Each service owns its data and exposes a small HTTP API.
- Services communicate asynchronously through an event bus for cross-service integration (e.g. Orders service subscribes to Product events).
- An API Gateway or BFF can aggregate calls for the UI.

ASCII architecture diagram

```
    +-----------+                +-------------+
    |   Client  | <------------> | API Gateway |
    +-----------+                +-------------+
           |                        |
           |                        |
           |                        |
     HTTPS |                        |HTTPS
           |                        v
           |   +-------------------------------+
           |   |         Products Service      |   <-- exposes CRUD + publishes events
           |   |  - ASP.NET Core Web API       |
           |   |  - JWT Auth                   |
           |   |  - EF Core (own DB)           |
           |   +-------------------------------+
           |                 |
           |                 |
           v                 |
        +-----------------+  |  +----------------+
        |   Orders Service|  |  | Payments       |
        |   (microservice)|  |  | Service        |
        +-----------------+  |  +----------------+
                    ^        |        ^
                    |        |        |
                    |        |        |
                    |        |        |
                    v        v        v
               +-------------------------------+
               |         Event Bus             |   (e.g. RabbitMQ, Kafka, Azure Service Bus)
               +-------------------------------+

```

Example interactions
- When a product is created/updated in Products Service, it can publish an event like `Product.Created` or `Product.Updated` to the Event Bus.
- Orders Service subscribes to product events (e.g. to cache product details or validate prices when creating orders).
- Payments Service handles payment-specific events triggered by Orders Service.
- API Gateway routes UI requests to the appropriate service and can handle authentication.

Extending this evaluation
- Replace the in-memory store with a real database (e.g., SQL Server, Oracle, PostgreSQL) and add migrations.
- Add message broker integration and publish/subscribe example events.
- Harden authentication to use a real identity provider.

- This repository is provided for evaluation purposes.
