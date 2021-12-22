using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    /// <summary>
    /// Służy do uzyskania informacji o użytkowniku w klasie contextu
    /// Wstrzykuje do klasy contextu, context http
    /// </summary>
    public class UserResolverService
    {
        private readonly IHttpContextAccessor _context;

        public UserResolverService(IHttpContextAccessor context)
        {
            _context = 
                context ?? throw new ArgumentNullException(nameof(context));
        }

        public string GetUser()
        {
            return _context.HttpContext.User?.Identity?.Name;
        }

    }
}
