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
            
            // Add more service groups here as needed
            // services.AddUserServices();
            // services.AddTransactionServices();
            // services.AddCategoryServices();
            
            return services;
        }

        private static IServiceCollection AddWalletServices(this IServiceCollection services)
        {
            services.AddScoped<IWalletService, WalletService>();
            return services;
        }

        // Example of how to organize services by domain
        // private static IServiceCollection AddUserServices(this IServiceCollection services)
        // {
        //     services.AddScoped<IUserService, UserService>();
        //     services.AddScoped<IUserProfileService, UserProfileService>();
        //     return services;
        // }

        // private static IServiceCollection AddTransactionServices(this IServiceCollection services)
        // {
        //     services.AddScoped<ITransactionService, TransactionService>();
        //     services.AddScoped<ITransactionCategoryService, TransactionCategoryService>();
        //     return services;
        // }
    }
}
