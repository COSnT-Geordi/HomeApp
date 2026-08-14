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
    [Route("/albums")]
    public class AlbumController : Controller, ICrudController<Album>
    {
        private HomeDbContext _context;
        public AlbumController(HomeDbContext context)
        {
            _context = context;
        }
        [Authorize]

        [HttpPost]
        public Album? CreateOne([FromBody] Album entity)
        {
            var principal = ControllerHelper.GetTokenFromRequest(Request);
            var owner = _context.Users.SingleOrDefault(e => e.ID == principal.UserID);

            try
            {
                entity.ID = 0;
                entity.DbFiles.Clear();
                entity.Creation_date = DateTime.Now.ToUniversalTime();
                entity.User = owner;
                var created = _context.Albums.Add(entity);
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
        public List<Album>? GetAll()
        {
            var principal = ControllerHelper.GetTokenFromRequest(Request);
            var owner = _context.Users.Include(e => e.Albums).SingleOrDefault(e => e.ID == principal.UserID);

            if (owner == null) return new List<Album>();
            try
            {
                return owner.Albums;
            }
            catch (Exception)
            {

                return null;
            }
        }
        [Authorize]

        [HttpGet("{id}")]

        public Album? GetById(int id)
        {
            var principal = ControllerHelper.GetTokenFromRequest(Request);
            var owner = _context.Users
                .Include(e => e.Albums)
                .ThenInclude(e=>e.DbFiles).SingleOrDefault(e => e.ID == principal.UserID);
            try
            {
                return owner.Albums
                    .SingleOrDefault(e => e.ID == id);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]

        [HttpPut]

        public Album? UpdateOne([FromBody] Album entity)
        {
            try
            {
                var existing = GetById((int)entity.ID);

                _context.Albums.Entry(existing).CurrentValues.SetValues(entity);
                foreach (var item in entity.DbFiles)
                {
                    item.Albums = null;
                }
                ContextTool.UpdateRelations<DbFile>(existing.DbFiles, entity.DbFiles);
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
