using BrightBudget.API.Services;
using BrightBudget.API.Services.Interfaces;

namespace BrightBudget.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register all application services here
            services.AddWalletServices();
            services.AddTransactionServices();
           
            return services;
        }

        private static IServiceCollection AddWalletServices(this IServiceCollection services)
        {
            services.AddScoped<IWalletService, WalletService>();
            return services;
        }
        
        private static IServiceCollection AddTransactionServices(this IServiceCollection services)
        {
            services.AddScoped<ITransactionService, TransactionService>();
            return services;
        }      
    }
}
