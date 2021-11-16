using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebStorageSystem.Filters
{
    public class ValidateModelFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
            await next();
        }
    }

    public class ValidateModelFilterAttribute : TypeFilterAttribute
    {
        public ValidateModelFilterAttribute() : base(typeof(ValidateModelFilter)) {}
    }
}
