using BrightBudget.API.Extensions;
using BrightBudget.API.Filters;
using BrightBudget.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BrightBudget.API.Dtos.TransactionCategory;

namespace BrightBudget.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [ServiceFilter(typeof(CurrentUserFilter))]
    public class TransactionCategoriesController : BaseApiController
    {
        private readonly ITransactionCategoryService _service;

        public TransactionCategoriesController(ITransactionCategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = HttpContext.GetCurrentUser()!;
            return Success(await _service.GetAllAsync(user.Id));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = HttpContext.GetCurrentUser()!;
            var result = await _service.GetByIdAsync(id, user.Id);
            return result == null ? Fail("Category not found") : Success(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TransactionCategoryCreateDto dto)
        {
            var user = HttpContext.GetCurrentUser()!;
            var result = await _service.CreateAsync(dto, user.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = HttpContext.GetCurrentUser()!; // User is guaranteed to exist due to filter
                var success = await _service.DeleteAsync(id, user.Id);
                if (!success)
                    return NotFound(new { message = "Category not found or access denied" });

                return Success(new { message = "Category deleted successfully" });
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Failed to delete Category");
            }          
        }
    }

}