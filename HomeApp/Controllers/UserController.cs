using HomeApp.Common;
using HomeApp.Helpers;
using HomeApp.Interfaces;
using HomeApp.SqlModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeApp.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UserController : Controller, ICrudController<User>
    {
        private HomeDbContext _context;
        public UserController(HomeDbContext context)
        {
            _context = context;
        }
        [Authorize]

        [HttpPost]
        public User? CreateOne([FromBody] User entity)
        {
            return null;
        }
        [Authorize]

        [HttpDelete("{id}")]
        public bool DeleteOne(int id)
        {
            return false;

        }
        [Authorize]

        [HttpGet]
        public List<User>? GetAll()
        {

            try
            {
                return _context?.Users?.ToList()?.Select(u => new User() { ID = u.ID, Username = u.Username })?.ToList() ?? [];
            }
            catch (Exception)
            {

                return null;
            }
        }
        [Authorize]

        [HttpGet("{id}")]

        public User? GetById(int id)
        {
            return null;
        }

        public User? UpdateOne([FromBody] User entity)
        {
            return null;
        }
    }
}
