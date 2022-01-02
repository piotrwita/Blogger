using WebAPI.Wrappers;

namespace WebAPI.Middlewares
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;

                var succeeded = false;
                var message = ex.Message;

                var response = new Response(succeeded, message);
                await context.Response.WriteAsJsonAsync(response);
            }
            
        }
    }
}
