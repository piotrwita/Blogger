using Application.Interfaces;
using Domain.Enums;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Models;
using WebAPI.Wrappers;

namespace WebAPI.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        /// <summary>
        /// Pozwala zarządzać użytkownikami , wyszukiwać , dodawać
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Pozwala zarządzać rolami użytkowników   
        /// </summary>
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Pozwala pobrać opcje konfiguracji z pliku appsettingss.json
        /// </summary>
        private readonly IConfiguration _configuration;

        private readonly IEmailSenderService _emailSenderService;

        public IdentityController(UserManager<ApplicationUser> userManager,
                                RoleManager<IdentityRole> roleManager,
                                IConfiguration configuration,
                                IEmailSenderService emailSenderService)
        {
            _userManager =
                userManager ?? throw new ArgumentException(nameof(userManager));

            _roleManager =
                roleManager ?? throw new ArgumentException(nameof(roleManager));

            _configuration =
                configuration ?? throw new ArgumentException(nameof(configuration));

            _emailSenderService =
                emailSenderService ?? throw new ArgumentException(nameof(emailSenderService));
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterModel register)
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

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                var role = new IdentityRole(UserRoles.User);
                await _roleManager.CreateAsync(role);
            }

            await _userManager.AddToRoleAsync(user, UserRoles.User);

            await _emailSenderService.Send(user.Email, "Registration confirmation", EmailTemplate.WelcomeMessage, user);

            var response = new Response<bool>
            {
                Succeeded = true,
                Message = "User created successfully"
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdminAsync(RegisterModel register)
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

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                var role = new IdentityRole(UserRoles.Admin);
                await _roleManager.CreateAsync(role);
            }

            await _userManager.AddToRoleAsync(user, UserRoles.Admin);

            var response = new Response<bool>
            {
                Succeeded = true,
                Message = "User created successfully"
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("RegisterSuperUser")]
        public async Task<IActionResult> RegisterSuperUserAsync(RegisterModel register)
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

            if (!await _roleManager.RoleExistsAsync(UserRoles.SuperUser))
            {
                var role = new IdentityRole(UserRoles.SuperUser);
                await _roleManager.CreateAsync(role);
            }

            await _userManager.AddToRoleAsync(user, UserRoles.SuperUser);

            var response = new Response<bool>
            {
                Succeeded = true,
                Message = "User created successfully"
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync(LoginModel login)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
            {
                //generowanie tokena
                //Tworzenie oswiadczenia (info o uzytkowniku. rolach, uprawnieniach)
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    //poswiadczenie - nazwa użytkownika
                    new Claim(ClaimTypes.Name, user.UserName),
                    //identyfikator JWT - zapewnia unikalny id dla tokena JWT
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                //pobieranie wszystkich roli uzytkownika
                var userRoles = await _userManager.GetRolesAsync(user);
                //dla każej roli tworzymy nowe oświadczenie
                foreach (var userRole in userRoles)
                {
                    var roleClaim = new Claim(ClaimTypes.Role, userRole);
                    authClaims.Add(roleClaim);
                };

                //klucz podpisu autoryzacji
                var authSignigKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                //poświaczenia podpisu
                var signCredentials = new SigningCredentials(authSignigKey, SecurityAlgorithms.HmacSha256);

                //tworzymy unikalny token
                var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddHours(2),
                    claims: authClaims,
                    signingCredentials: signCredentials
                    );

                //token z informacją o dacie wygaśnięcia
                var extendedToken = new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expitarion = token.ValidTo
                };

                return Ok(extendedToken);
            }
            return Unauthorized();
        }
    }
}
