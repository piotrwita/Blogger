using Swashbuckle.AspNetCore.Filters;
using WebAPI.Wrappers;

namespace WebAPI.SwaggerExamples.Responses
{
    public class RegisterResponseStatus500Example : IExamplesProvider<RegisterResponseStatus500>
    {
        public RegisterResponseStatus500 GetExamples()
        {
            var response = new RegisterResponseStatus500
            {
                Succeeded = false,
                Message = "User already exists"
            };

            return response;
        }
    }

    public class RegisterResponseStatus500 : Response
    {
    }
}
