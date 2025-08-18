using BrightBudget.API.Dtos.Wallet;
using BrightBudget.API.Extensions;
using BrightBudget.API.Filters;
using BrightBudget.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using BrightBudget.API.Data;

namespace BrightBudget.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [ServiceFilter(typeof(CurrentUserFilter))]
    public class WalletController : BaseApiController
    {
        private readonly IWalletService _walletService;
        private readonly AppDbContext _context;
        public WalletController(IWalletService walletService, AppDbContext context)
        {
            _walletService = walletService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyWallets()
        {
            try
            {
                var user = HttpContext.GetCurrentUser()!; // User is guaranteed to exist due to filter
                var walletDtos = await _walletService.GetByUserIdAsync(user.Id);
                return Success(walletDtos);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Failed to retrieve wallets");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWallet(int id)
        {
            try
            {
                var user = HttpContext.GetCurrentUser()!; // User is guaranteed to exist due to filter
                var walletDto = await _walletService.GetByIdAsync(id, user.Id);
                if (walletDto == null)
                    return NotFound(new { message = "Wallet not found" });

                return Success(walletDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to retrieve wallet", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateWallet(WalletCreateDto dto)
        {
            try
            {
                var user = HttpContext.GetCurrentUser()!; // User is guaranteed to exist due to filter
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var walletDto = await _walletService.CreateAsync(dto, user.Id);
                return Success(walletDto);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Failed to create wallet");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWallet(int id, WalletUpdateDto dto)
        {
            try
            {
                var user = HttpContext.GetCurrentUser()!; // User is guaranteed to exist due to filter
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var success = await _walletService.UpdateAsync(id, dto, user.Id);
                if (!success)
                    return NotFound(new { message = "Wallet not found or access denied" });

                return Success(new { message = "Wallet updated successfully" });
            }
            catch (Exception ex)
            {
                 return HandleException(ex, "Failed to update wallet");
            } 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWallet(int id)
        {
            try
            {
                var user = HttpContext.GetCurrentUser()!; // User is guaranteed to exist due to filter
                var success = await _walletService.DeleteAsync(id, user.Id);
                if (!success)
                    return NotFound(new { message = "Wallet not found or access denied" });

                return Success(new { message = "Wallet deleted successfully" });
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Failed to delete wallet");
            }
        }
    }
}
