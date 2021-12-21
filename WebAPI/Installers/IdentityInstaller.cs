using Infrastructure.Data;
using Infrastructure.Data.Dapper;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebAPI.Installers
{
    public class IdentityInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                //dzięki temu, że wskazaliśmy entity framework jako magazyn, w którym będziemy przechowywać dane to
                //dodając teraz migracje framework utworzy za nas wszystkie tabele potrzebne do obslugi procesu uwierzytelniania i autoryzacji
                .AddEntityFrameworkStores<BloggerContext>()
                .AddDefaultTokenProviders();

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            var tokenValidationParameters = new TokenValidationParameters()
            {
                //ustawiamy wartości logiczne kontrolujące, czy dostawcy i odbiorcy zostaną zweryfikowani podczas walidacji tokena
                ValidateIssuer = false,
                ValidateAudience = false,
                //256 bitwy sekretny klucz wykorzystywany do sprawdzania poprawności podpisu
                //zwrócony w postaci tablicy bajtów zakodowanych w utf8
                IssuerSigningKey = signingKey
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //wyodrebneinie i walidację tokena jwt z nagłówka autoryzacji
            .AddJwtBearer(option =>
            {
                //zestaw parametrów służących do sprawdzania poprawności tokena
                option.TokenValidationParameters = tokenValidationParameters;
            });
        }
    }
}
