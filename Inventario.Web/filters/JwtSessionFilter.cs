using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Inventario.Web.filters;

public class JwtSessionFilter : IActionFilter
{
    public void OnActionExecuting (ActionExecutingContext context) 
    { 
        var token = context.HttpContext.Session.GetString("JWT");
        if (string.IsNullOrEmpty(token))
        {
            context.Result = new RedirectToRouteResult(
                "Login",
                "Auth",
                null
                );
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }


}

