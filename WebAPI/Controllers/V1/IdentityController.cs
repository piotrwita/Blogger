using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Wrappers;

namespace WebAPI.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        /// <summary>
        /// Pozwala zarządzać użytkownikami , wyszukiwać , dodawać
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Pozwala pobrać ocje konfiguracji z pliku appsettingss.json
        /// </summary>
        private readonly IConfiguration _configuration;

        public IdentityController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = 
                userManager ?? throw new ArgumentException(nameof(userManager));

            _configuration =
                configuration ?? throw new ArgumentException(nameof(configuration));
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterModel register)
        {
            var userExists = await _userManager.FindByNameAsync(register.UserName);

            if (userExists != null)
            {
                var responseError = new Response<bool>
                {
                    Succeeded = false,
                    Message = "User already exists"
                };

                return StatusCode(StatusCodes.Status500InternalServerError, responseError);
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = register.Email,
                //unikalny identyfikator gwarantuje nam, że za każdym razem, gdy zmieni się coś zwiąanego z bezpieczeństwem użytkownika
                //np hasło to automatycznie zostaną unieważnone wszystkie pliki cookie z logowania
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = register.UserName,
            };

            var result = await _userManager.CreateAsync(user, register.Password);

            if (!result.Succeeded)
            {
                var responseError = new Response<bool>
                {
                    Succeeded = false,
                    Message = "User creation failed exists. Please check user details and try again",
                    Errors = result.Errors.Select(x => x.Description)
                };

                return StatusCode(StatusCodes.Status500InternalServerError, responseError);
            }

            var response = new Response<bool>
            {
                Succeeded = true,
                Message = "User created successfully"
            };

            return Ok(response);
        }
    }
}
