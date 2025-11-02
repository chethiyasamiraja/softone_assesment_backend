using AuthBackend.Modal;
using Microsoft.AspNetCore.Mvc;

namespace AuthBackend.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }
    }
}
