using BrightBudget.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BrightBudget.API.Filters
{
    public class CurrentUserFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.GetCurrentUser();
            if (user == null)
            {
                context.Result = new UnauthorizedObjectResult(new { message = "User not authenticated" });
                return;
            }

            // Store the user in HttpContext.Items for easy access in controllers
            context.HttpContext.Items["CurrentUser"] = user;
        }
    }
}
