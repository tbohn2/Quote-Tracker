using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Quote_Tracker.Filters
{
    public class RequireAuthAttribute : Attribute, IAuthorizationFilter
    {
        public const string SessionUserIdKey = "UserId";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.Session.GetInt32(SessionUserIdKey);
            if (userId == null)
            {
                context.Result = new UnauthorizedObjectResult("You must be logged in.");
                return;
            }
            context.HttpContext.Items["UserId"] = userId;
        }
    }
}
