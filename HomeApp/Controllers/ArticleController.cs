using HomeApp.Common;
using HomeApp.Interfaces;
using HomeApp.SqlModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            try
            {
                entity.ID = null;
                entity.Creation_date = DateTime.Now.ToUniversalTime();
                entity.Last_updated = DateTime.Now.ToUniversalTime();
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
                var existing = GetById(id);

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
            try
            {
                return _context.Articles.ToList();
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
            try
            {
                return _context.Articles.SingleOrDefault(e => e.ID == id);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]

        [HttpPut]

        public Article? UpdateOne([FromBody] Article entity)
        {
            try
            {
                var existing = GetById((int)entity.ID);
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
