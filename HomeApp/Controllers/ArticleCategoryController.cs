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
    [Route("/articlecategories")]
    public class ArticleCategoryController : Controller, ICrudController<ArticleCategory>
    {
        private HomeDbContext _context;
        public ArticleCategoryController(HomeDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ArticleCategory? CreateOne([FromBody] ArticleCategory entity)
        {
            var principal = ControllerHelper.GetTokenFromRequest(Request);
            try
            {
                entity.ID = null;
               
                var created = _context.ArticleCategories.Add(entity);
                _context.SaveChanges();
                return created.Entity;
            }
            catch (Exception)
            {

                return null;
            }
        }

        [HttpDelete("{id}")]
        public bool DeleteOne(int id)
        {
            try
            {
                var existing = GetById(id);//already searches
                existing.Articles.Clear();
                _context.ArticleCategories.Remove(existing);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        [HttpGet]
        public List<ArticleCategory>? GetAll()
        {
            var principal = ControllerHelper.GetTokenFromRequest(Request);

            try
            {
                return _context.ArticleCategories.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet("{id}")]

        public ArticleCategory? GetById(int id)
        {
            var principal = ControllerHelper.GetTokenFromRequest(Request);
            try
            {
                return _context.ArticleCategories.Include(e => e.Articles).SingleOrDefault(e => e.ID == id);
            }
            catch (Exception)
            {

                return null;
            }
        }
        [Authorize]

        [HttpPut]

        public ArticleCategory? UpdateOne([FromBody] ArticleCategory entity)
        {
           
            try
            {
                var existing = GetById((int)entity.ID);//already searches on owner!

                _context.ArticleCategories.Entry(existing).CurrentValues.SetValues(entity);
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
