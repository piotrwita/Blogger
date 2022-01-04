using Swashbuckle.AspNetCore.Filters;
using WebAPI.Wrappers;

namespace WebAPI.SwaggerExamples.Responses
{
    public class RegisterResponseStatus200Example : IExamplesProvider<RegisterResponseStatus200>
    {
        public RegisterResponseStatus200 GetExamples()
        {
            var response = new RegisterResponseStatus200
            {
                Succeeded = true,
                Message = "User created successfully"
            };

            return response;
        }
    }

    public class RegisterResponseStatus200 : Response
    {
    }
}
