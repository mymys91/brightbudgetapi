using BrightBudget.Infrastructure.Data;
using BrightBudget.Infrastructure.Repositories.Implementations;
using BrightBudget.Infrastructure.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BrightBudget.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
               options.UseSqlite(config.GetConnectionString("DefaultConnection")));

            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IExpenseCategoryRepository, ExpenseCategoryRepository>();
            services.AddScoped<ISavingGoalRepository, SavingGoalRepository>();
            services.AddScoped<ISavingTransactionRepository, SavingTransactionRepository>();
            return services;
        }
    }
}