using HomeApp.Common;
using HomeApp.Interfaces;
using HomeApp.Models;
using HomeApp.Services;
using HomeApp.SqlModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeApp.Helpers;
namespace HomeApp.Controllers
{
    [ApiController]
    [Route("/auth")]
    public class AuthController : Controller
    {
        private HomeDbContext _context;

        private readonly IAuthenticationService _authService;
        private LoginService _loginService;

        public AuthController(HomeDbContext context, LoginService loginService, IAuthenticationService authenticationService)
        {
            _context = context;
            _authService = authenticationService;
            _loginService = loginService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = _loginService.Login(request);

            if (string.IsNullOrEmpty(result))
                return Unauthorized();


            return Ok(new { token = result });
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(SignUpRequest request)
        {
            var result = _loginService.SignUp(request);

            if (result == null)
                return Unauthorized();

            return Ok(result);
        }
        [Authorize]
        [HttpGet("isauthenticated")]
        public async Task<IActionResult> IsAuthenticated()
        {

            var principal = ControllerHelper.GetTokenFromRequest(Request);

            if (principal.UserID == 0) return Unauthorized();
            // Console.WriteLine(token);

            return Ok();

        }


    }
}
