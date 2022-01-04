using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPI.Wrappers;

namespace WebAPI.Attributes
{
    /// <summary>
    /// odpowiednie sformatowanie odpowiedzi wynikającej z walidacji z wykorzystaniem klasy Response
    /// </summary>
    public class ValidateFilterAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);

            if(!context.ModelState.IsValid)
            {
                var entry = context.ModelState.Values.FirstOrDefault();

                var responseError = new Response<bool>
                {
                    Succeeded = false,
                    Message = "Something went wrong",
                    Errors = entry.Errors.Select(x => x.ErrorMessage)
                };

                context.Result = new BadRequestObjectResult(responseError);
            }
        }
    }
}
