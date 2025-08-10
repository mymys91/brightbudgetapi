# BrightBudget Wallet API

This document describes the Wallet API endpoints for managing user wallets in the BrightBudget application.

## Overview

The Wallet API provides CRUD operations for managing user wallets. Each user can create multiple wallets with different currencies and initial balances. All operations require authentication via JWT tokens.

## Authentication

All wallet endpoints require a valid JWT access token in the Authorization header:
```
Authorization: Bearer <access_token>
```

## Endpoints

### 1. Get My Wallets
**GET** `/api/wallet`

Retrieves all wallets belonging to the authenticated user.

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "name": "Main Wallet",
      "initialBalance": 1000.00,
      "currency": "VND"
    },
    {
      "id": 2,
      "name": "Savings Wallet",
      "initialBalance": 5000.00,
      "currency": "USD"
    }
  ]
}
```

### 2. Get Specific Wallet
**GET** `/api/wallet/{id}`

Retrieves a specific wallet by ID. Users can only access their own wallets.

**Parameters:**
- `id` (int): Wallet ID

**Response:**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "name": "Main Wallet",
    "initialBalance": 1000.00,
    "currency": "VND"
  }
}
```

**Error Responses:**
- `404 Not Found`: Wallet not found
- `403 Forbidden`: User doesn't own this wallet

### 3. Create Wallet
**POST** `/api/wallet`

Creates a new wallet for the authenticated user.

**Request Body:**
```json
{
  "name": "New Wallet",
  "initialBalance": 1000.00,
  "currency": "VND"
}
```

**Validation Rules:**
- `name`: Required, string
- `initialBalance`: Optional, decimal (defaults to 0)
- `currency`: Optional, string (defaults to "VND")

**Response:**
```json
{
  "success": true,
  "data": {
    "id": 3,
    "name": "New Wallet",
    "initialBalance": 1000.00,
    "currency": "VND"
  }
}
```

### 4. Update Wallet
**PUT** `/api/wallet/{id}`

Updates an existing wallet. Users can only update their own wallets.

**Parameters:**
- `id` (int): Wallet ID

**Request Body:**
```json
{
  "name": "Updated Wallet Name",
  "currency": "USD"
}
```

**Validation Rules:**
- `name`: Required, string
- `currency`: Required, string

**Response:**
```json
{
  "success": true,
  "data": {
    "message": "Wallet updated successfully"
  }
}
```

**Error Responses:**
- `404 Not Found`: Wallet not found or access denied

### 5. Delete Wallet
**DELETE** `/api/wallet/{id}`

Deletes a wallet. Users can only delete their own wallets.

**Parameters:**
- `id` (int): Wallet ID

**Response:**
```json
{
  "success": true,
  "data": {
    "message": "Wallet deleted successfully"
  }
}
```

**Error Responses:**
- `404 Not Found`: Wallet not found or access denied

## Data Models

### Wallet Model
```csharp
public class Wallet
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal InitialBalance { get; set; }
    public string Currency { get; set; }
    public string UserId { get; set; }
    public ApplicationUser? User { get; set; }
}
```

### DTOs

#### WalletCreateDto
```csharp
public class WalletCreateDto
{
    [Required(ErrorMessage = "Wallet name is required")]
    public string Name { get; set; }
    public decimal InitialBalance { get; set; }
    public string Currency { get; set; } = "VND";
}
```

#### WalletReadDto
```csharp
public class WalletReadDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal InitialBalance { get; set; }
    public string Currency { get; set; }
}
```

#### WalletUpdateDto
```csharp
public class WalletUpdateDto
{
    public string Name { get; set; }
    public string Currency { get; set; } = "VND";
}
```

## Security Features

1. **Authentication Required**: All endpoints require valid JWT tokens
2. **User Isolation**: Users can only access, modify, or delete their own wallets
3. **Input Validation**: All inputs are validated using data annotations
4. **Error Handling**: Comprehensive error handling with meaningful messages

## Usage Examples

### Complete Workflow

1. **Login to get access token:**
```http
POST /api/auth/login
{
  "email": "user@example.com",
  "password": "password123"
}
```

2. **Create a wallet:**
```http
POST /api/wallet
Authorization: Bearer <access_token>
{
  "name": "My First Wallet",
  "initialBalance": 1000.00,
  "currency": "VND"
}
```

3. **Get all wallets:**
```http
GET /api/wallet
Authorization: Bearer <access_token>
```

4. **Update wallet:**
```http
PUT /api/wallet/1
Authorization: Bearer <access_token>
{
  "name": "Updated Wallet Name",
  "currency": "USD"
}
```

5. **Delete wallet:**
```http
DELETE /api/wallet/1
Authorization: Bearer <access_token>
```

## Error Handling

The API returns consistent error responses:

```json
{
  "success": false,
  "message": "Error description",
  "errors": ["Detailed error 1", "Detailed error 2"]
}
```

Common HTTP status codes:
- `200 OK`: Successful operation
- `201 Created`: Resource created successfully
- `400 Bad Request`: Invalid input or validation error
- `401 Unauthorized`: Missing or invalid authentication
- `403 Forbidden`: Access denied
- `404 Not Found`: Resource not found
- `500 Internal Server Error`: Server error

## Testing

Use the provided HTTP test files:
- `Wallet_Tests.http`: Dedicated wallet API tests
- `BrightBudget.API.http`: Complete API test suite

## Dependencies

- **Entity Framework Core**: Database operations
- **AutoMapper**: Object mapping between models and DTOs
- **ASP.NET Core Identity**: User authentication and management
- **CurrentUserMiddleware**: Automatic user extraction from JWT tokens

## Notes

- Wallet balances are stored as `decimal` for precision
- Currency defaults to "VND" (Vietnamese Dong)
- Users can have multiple wallets with different currencies
- All operations are scoped to the authenticated user
- The API follows RESTful conventions
