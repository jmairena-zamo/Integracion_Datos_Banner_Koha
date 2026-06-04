using Integracion_Datos_Banner_Koha.Constant;
using Integracion_Datos_Banner_Koha.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Integracion_Datos_Banner_Koha.Filters.Action
{
    public class ValidationModelAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                List<string> errors = context.ModelState.Values.Where(v => v.Errors.Count > 0).SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToList();
                context.Result = new BadRequestObjectResult(new ResponseModel(StatusCodes.Status400BadRequest, ReplyMessages.RequiredFields, string.Join(" ; ", errors)));
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
