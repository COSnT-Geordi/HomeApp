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
    [Route("/articles")]
    public class ArticleController : Controller, ICrudController<Article>
    {
        private HomeDbContext _context;
        public ArticleController(HomeDbContext context)
        {
            _context = context;
        }
        [Authorize]

        [HttpPost]
        public Article? CreateOne([FromBody] Article entity)
        {
            var principal = ControllerHelper.GetTokenFromRequest(Request);
            var owner = _context.Users.Include(e => e.Articles).SingleOrDefault(e => e.ID == principal.UserID);
            try
            {
                entity.ID = null;
                entity.Creation_date = DateTime.Now.ToUniversalTime();
                entity.Last_updated = DateTime.Now.ToUniversalTime();
                entity.User = owner;
                var created = _context.Articles.Add(entity);
                _context.SaveChanges();
                return created.Entity;
            }
            catch (Exception)
            {

                return null;
            }
        }
        [Authorize]

        [HttpDelete("{id}")]
        public bool DeleteOne(int id)
        {
            try
            {
                var existing = GetById(id);//already searches

                _context.Remove(existing);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        [Authorize]

        [HttpGet]
        public List<Article>? GetAll()
        {
            var principal = ControllerHelper.GetTokenFromRequest(Request);
            var owner = _context.Users.Include(e => e.Articles).SingleOrDefault(e => e.ID == principal.UserID);

            if (owner == null) return new List<Article>();
            try
            {
                return owner.Articles;
            }
            catch (Exception)
            {
                return null;
            }
        }
        [Authorize]

        [HttpGet("{id}")]

        public Article? GetById(int id)
        {
            var principal = ControllerHelper.GetTokenFromRequest(Request);
            var owner = _context.Users.Include(e => e.Articles).SingleOrDefault(e => e.ID == principal.UserID);
            try
            {
                return owner?.Articles.SingleOrDefault(e => e.ID == id);
            }
            catch (Exception)
            {

                return null;
            }
        }
        [Authorize]

        [HttpPut]

        public Article? UpdateOne([FromBody] Article entity)
        {
           
            try
            {
                var existing = GetById((int)entity.ID);//already searches on owner!
                entity.Last_updated = DateTime.Now.ToUniversalTime();

                _context.Articles.Entry(existing).CurrentValues.SetValues(entity);
                _context.SaveChanges();
                return existing;
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
