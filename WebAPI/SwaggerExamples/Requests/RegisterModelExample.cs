using Swashbuckle.AspNetCore.Filters;
using WebAPI.Models;

namespace WebAPI.SwaggerExamples.Requests
{
    /// <summary>
    /// zwraca przykladowe cialo żądania ktore zostanie wyswietlone w swaggerze
    /// </summary>
    public class RegisterModelExample : IExamplesProvider<RegisterModel>
    {
        public RegisterModel GetExamples()
        {
            var model = new RegisterModel()
            {
                UserName = "yourUniqueName",
                Email = "yourEmailAddrress@example.com",
                Password = "Pa$$w0rd123!"
            };

            return model;
        }
    }
}
