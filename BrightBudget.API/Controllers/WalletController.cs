using BrightBudget.API.Dtos.Wallet;
using BrightBudget.API.Extensions;
using BrightBudget.API.Filters;
using BrightBudget.API.Models;
using BrightBudget.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrightBudget.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [ServiceFilter(typeof(CurrentUserFilter))]
    public class WalletController : BaseApiController
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyWallets()
        {
            try
            {
                var user = HttpContext.GetCurrentUser()!; // User is guaranteed to exist due to filter
                var wallets = await _walletService.GetByUserIdAsync(user.Id);
                var walletDtos = wallets.Select(w => new WalletReadDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    InitialBalance = w.InitialBalance,
                    Currency = w.Currency
                });

                return Success(walletDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to retrieve wallets", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWallet(int id)
        {
            try
            {
                var user = HttpContext.GetCurrentUser()!; // User is guaranteed to exist due to filter
                var wallet = await _walletService.GetByIdAsync(id);
                if (wallet == null)
                    return NotFound(new { message = "Wallet not found" });

                // Ensure user can only access their own wallets
                if (wallet.UserId != user.Id)
                    return Forbid();

                var walletDto = new WalletReadDto
                {
                    Id = wallet.Id,
                    Name = wallet.Name,
                    InitialBalance = wallet.InitialBalance,
                    Currency = wallet.Currency
                };

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

                var wallet = await _walletService.CreateAsync(dto, user.Id);
                var walletDto = new WalletReadDto
                {
                    Id = wallet.Id,
                    Name = wallet.Name,
                    InitialBalance = wallet.InitialBalance,
                    Currency = wallet.Currency
                };

                return CreatedAtAction(nameof(GetWallet), new { id = wallet.Id }, Success(walletDto));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to create wallet", error = ex.Message });
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
                return BadRequest(new { message = "Failed to update wallet", error = ex.Message });
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
                return BadRequest(new { message = "Failed to delete wallet", error = ex.Message });
            }
        }
    }
}
