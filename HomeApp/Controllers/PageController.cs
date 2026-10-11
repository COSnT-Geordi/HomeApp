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
    [Route("/pages")]
    public class PageController : Controller, ICrudController<Page>
    {
        private HomeDbContext _context;
        public PageController(HomeDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles ="ADMIN")]
        [HttpPost]
        public Page? CreateOne([FromBody] Page entity)
        {
          
            try
            {
                entity.ID = 0;
         
                var created = _context.Pages.Add(entity);
                _context.SaveChanges();
                return created.Entity;
            }
            catch (Exception)
            {

                return null;
            }
        }
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public bool DeleteOne(int id)
        {
            try
            {
                var existing = GetById(id);

                _context.Pages.Remove(existing);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        [HttpGet]
        public List<Page>? GetAll()
        {        
            try
            {
                return _context.Pages.ToList();
            }
            catch (Exception)
            {

                return null;
            }
        }
        [HttpGet("{id}")]
        public Page? GetById(int id)
        {
            
            try
            {
                return _context.Pages.SingleOrDefault(e=> e.ID == id);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPut]

        public Page? UpdateOne([FromBody] Page entity)
        {
            try
            {
                var existing = GetById((int)entity.ID);

                _context.Pages.Entry(existing).CurrentValues.SetValues(entity);
               
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
