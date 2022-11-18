# WorkoutGlobal authentication and authorization microservice

## Structure

Current microservice consists of 3 projects:
- WorkoutGlobal.AuthorizationServiceApi (authorization & authentication & user API)
- WorkoutGlobal.AuthorizationServiceApi.UnitTests (unit tests for API)
- WorkoutGlobal.AuthorizationServiceApi.IntegrationTests (integration tests for API)

## Technologies

Current API using such technologies and concepts:
- Auto mapping (library `AutoMapper`)
- Auto validation (`FluentValidation`, `FluentValidation.AspNetCore`)
- Swagger
- Entity Framework Core with Identity (MS SQL)
- Authentication via JWT tokens (access / refresh tokens)
- `FluentAssertions`, `AutoFixture`, `Moq` and `IAsyncLifetime` pattern for unit and integration tests

## How run API & Tests?

You should use your MS SQL server or other cloud solution via change database connection string in `appsettings.json` file. You can run tests via Test Explorer.
