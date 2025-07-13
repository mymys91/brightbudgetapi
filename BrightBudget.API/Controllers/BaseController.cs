using BrightBudget.API.Common;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BrightBudget.API.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILogger<BaseController> Logger;

        protected BaseController(ILogger<BaseController> logger)
        {
            Logger = logger;
        }

        // Standardized success response
        protected IActionResult Success<T>(T data, string? message = null)
        {
            return Ok(ApiResponse<T>.Ok(data, message));
        }

        // Standardized error response
        protected IActionResult Error(string message)
        {
            return BadRequest(ApiResponse<string>.Fail(message));
        }

        protected IActionResult InvalidModelState()
        {
            var errors = ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .ToDictionary(
                    e => e.Key,
                    e => e.Value!.Errors.Select(err => err.ErrorMessage).ToArray()
                );

            var response = new ValidationErrorResponse
            {
                Errors = errors
            };

            return BadRequest(response);
        }
    }
}