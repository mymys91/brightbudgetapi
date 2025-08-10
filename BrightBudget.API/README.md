# BrightBudget API

A comprehensive financial management API built with ASP.NET Core, featuring JWT authentication, refresh tokens, and wallet management.

## Features

- **Authentication System**: JWT-based authentication with refresh tokens
- **User Management**: Registration, login, logout, and password management
- **Wallet Management**: CRUD operations for user wallets with multi-currency support
- **Security**: Role-based access control and user isolation
- **Database**: SQLite with Entity Framework Core
- **Documentation**: Comprehensive API documentation and testing files

## Quick Start

1. **Clone the repository**
2. **Navigate to the API project**: `cd BrightBudget.API`
3. **Update database**: `dotnet ef database update`
4. **Run the application**: `dotnet run`
5. **Test the API**: Use the provided `.http` files

## API Documentation

- **[Authentication API](README_AUTH.md)**: Complete authentication system documentation
- **[Wallet API](README_WALLET.md)**: Wallet management endpoints and usage

## Testing

The project includes comprehensive HTTP test files:
- `BrightBudget.API.http`: Complete API test suite
- `Auth_Refresh_Tests.http`: Authentication flow testing
- `Wallet_Tests.http`: Wallet API testing

## Architecture

- **Controllers**: RESTful API endpoints with automatic authorization
- **Services**: Business logic layer with organized registration
- **Models**: Entity Framework data models
- **DTOs**: Data transfer objects for API requests/responses
- **Middleware**: Custom authentication and error handling
- **Extensions**: Helper methods for common operations and service registration
- **Filters**: Custom authorization filters for automatic user validation

## Service Organization

Services are organized using extension methods in `Extensions/ServiceCollectionExtensions.cs`:

- **Domain-based grouping**: Services are grouped by business domain (e.g., `AddWalletServices()`)
- **Clean Program.cs**: Main startup file remains focused on configuration
- **Scalable structure**: Easy to add new service groups as the application grows
- **Maintainable**: All service registrations are centralized in one location

## Authorization & Security

The API uses a custom authorization filter system for enhanced security and code cleanliness:

- **Automatic User Validation**: `[Authorize]` + `[ServiceFilter(typeof(CurrentUserFilter))]` automatically validates JWT tokens
- **No Manual Checks**: Controllers no longer need to manually call `GetCurrentUser()` or check authentication
- **Centralized Security**: All protected endpoints automatically get the current user from the filter
- **Clean Controllers**: Controllers focus on business logic, not authentication boilerplate
- **Consistent Pattern**: All protected endpoints use the same authorization approach

## Dependencies

- ASP.NET Core 9.0
- Entity Framework Core
- ASP.NET Core Identity
- AutoMapper
- JWT Bearer Authentication
