using BrightBudget.API.Response;
using Microsoft.AspNetCore.Mvc;

namespace BrightBudget.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected IActionResult Success<T>(T data)
        {
            return base.Ok(ApiResponse<T>.Ok(data));
        }

        protected IActionResult Success()
        {
            return base.Ok(ApiResponse<string>.Ok("Success"));
        }

        protected IActionResult Fail(string errorMessage)
        {
            return BadRequest(ApiResponse<string>.Fail(errorMessage));
        }

        protected IActionResult Fail(IEnumerable<string> errors)
        {
            return BadRequest(ApiResponse<IEnumerable<string>>.Fail(errors));
        }
    }
}
