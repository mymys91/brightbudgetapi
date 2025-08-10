# Authentication API Documentation

## Overview
The BrightBudget API now supports JWT-based authentication with refresh tokens for enhanced security and user experience.

## Architecture

### CurrentUserMiddleware
A custom middleware that automatically extracts the current user from JWT tokens and makes them available to controllers, eliminating redundant authentication code.

**Features:**
- Automatically extracts user from JWT tokens
- Populates `HttpContext.Items["CurrentUser"]` with the current user
- Handles token validation and user lookup
- Gracefully handles invalid/expired tokens
- No performance impact on unauthenticated requests

**Usage in Controllers:**
```csharp
using BrightBudget.API.Extensions;

[HttpGet("profile")]
public IActionResult GetProfile()
{
    var user = HttpContext.GetCurrentUser();
    if (user == null)
        return Unauthorized(new { message = "User not authenticated" });

    return Success(new { 
        id = user.Id,
        email = user.Email,
        userName = user.UserName
    });
}
```

**Extension Methods:**
- `HttpContext.GetCurrentUser()` - Returns the full `ApplicationUser` object from valid tokens
- `HttpContext.GetCurrentUserId()` - Returns just the user ID string
- `HttpContext.GetCurrentUserFromExpiredToken()` - Returns user from expired tokens (for refresh endpoint)

## Endpoints

### 1. Register User
- **POST** `/api/auth/register`
- **Body:**
  ```json
  {
    "email": "user@example.com",
    "password": "SecurePassword123!"
  }
  ```
- **Response:** Success message or validation errors

### 2. Login
- **POST** `/api/auth/login`
- **Body:**
  ```json
  {
    "email": "user@example.com",
    "password": "SecurePassword123!"
  }
  ```
- **Response:**
  ```json
  {
    "success": true,
    "data": {
      "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
      "refreshToken": "base64_encoded_refresh_token",
      "expiresIn": 900
    }
  }
  ```

### 3. Get Current User
- **GET** `/api/auth/me`
- **Headers:** `Authorization: Bearer {accessToken}`
- **Response:** Current user information
- **Use Case:** Check authentication status and get user details

### 4. Refresh Token
- **POST** `/api/auth/refresh`
- **Body:**
  ```json
  {
    "accessToken": "expired_or_valid_access_token",
    "refreshToken": "valid_refresh_token"
  }
  ```
- **Response:** New access token and refresh token
- **Use Case:** When the access token expires (default: 15 minutes)

### 5. Change Password
- **POST** `/api/auth/change-password`
- **Headers:** `Authorization: Bearer {accessToken}`
- **Body:**
  ```json
  {
    "currentPassword": "CurrentPassword123!",
    "newPassword": "NewSecurePassword456!"
  }
  ```
- **Response:** Success message or validation errors
- **Security:** Requires current password verification and invalidates all refresh tokens

### 6. Logout
- **POST** `/api/auth/logout`
- **Headers:** `Authorization: Bearer {accessToken}`
- **Response:** Success message
- **Effect:** Invalidates the refresh token

## Token Configuration

### Access Token
- **Expiry:** 15 minutes (configurable in `appsettings.json`)
- **Purpose:** Short-lived token for API requests
- **Storage:** Client-side (memory, secure storage)

### Refresh Token
- **Expiry:** 7 days (configurable in `appsettings.json`)
- **Purpose:** Long-lived token to obtain new access tokens
- **Storage:** Database (linked to user account)
- **Security:** Cryptographically secure random bytes

## Configuration

Update your `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "your-secret-key-here",
    "Issuer": "BrightBudget",
    "Audience": "BrightBudgetUsers",
    "AccessTokenExpiryMinutes": 15,
    "RefreshTokenExpiryDays": 7
  }
}
```

## Security Features

1. **Token Rotation:** Refresh tokens are rotated on each use
2. **Secure Storage:** Refresh tokens stored in database with expiry
3. **Automatic Invalidation:** Logout immediately invalidates refresh tokens
4. **Cryptographic Security:** Refresh tokens use cryptographically secure random generation
5. **Password Security:** Change password requires current password verification
6. **Token Invalidation:** Password changes invalidate all refresh tokens for security
7. **Authentication Check:** "Me" endpoint validates current authentication status
8. **Middleware Security:** CurrentUserMiddleware handles token validation securely

## Usage Flow

1. **Initial Login:** User logs in and receives both tokens
2. **API Requests:** Use access token for authenticated requests
3. **Status Check:** Use "me" endpoint to verify authentication status
4. **Token Expiry:** When access token expires, use refresh token to get new one
5. **Seamless Experience:** User continues without re-authentication
6. **Password Change:** User can change password with current password verification
7. **Security Reset:** Password changes require re-authentication
8. **Logout:** Refresh token is invalidated, requiring new login

## Error Handling

- **401 Unauthorized:** Invalid or expired tokens, user not authenticated
- **400 Bad Request:** Missing or invalid request data, validation errors
- **404 Not Found:** User not found
- **500 Internal Server Error:** Server-side issues

## Testing

Use the provided HTTP test files:
- `BrightBudget.API.http` - Main test file
- `Auth_Refresh_Tests.http` - Dedicated auth tests

## Best Practices

1. **Store Access Tokens:** In memory or secure client storage
2. **Store Refresh Tokens:** Securely on client side
3. **Automatic Refresh:** Implement automatic token refresh before expiry
4. **Error Handling:** Handle 401 responses by attempting token refresh
5. **Secure Storage:** Never store tokens in localStorage (use httpOnly cookies or secure storage)
6. **Password Security:** Enforce strong password policies
7. **Session Management:** Implement proper session handling for password changes
8. **Authentication Status:** Use "me" endpoint to check current user status
9. **Middleware Usage:** Use `HttpContext.GetCurrentUser()` instead of manual JWT parsing
10. **Code Reuse:** Leverage CurrentUserMiddleware to eliminate redundant authentication code
