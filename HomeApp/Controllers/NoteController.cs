using HomeApp.Common;
using HomeApp.Interfaces;
using HomeApp.SqlModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeApp.Controllers
{
    [ApiController]
    [Route("/notes")]
    public class NoteController : Controller, ICrudController<Note>
    {
        private HomeDbContext _context;
        public NoteController(HomeDbContext context)
        {
            _context = context;
        }
        [Authorize]

        [HttpPost]
        public Note? CreateOne([FromBody] Note entity)
        {
            try
            {
                entity.ID = null;
                entity.Creation_date = DateTime.Now;
                var created = _context.Notes.Add(entity);
                _context.SaveChanges();
                return created.Entity;
            }
            catch (Exception)
            {

                return null;
            }
        }
        [Authorize]

        [HttpDelete]
        public bool DeleteOne(int id)
        {
            try
            {
                var existing = GetById(id);

                _context.Notes.Remove(existing);
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
        public List<Note>? GetAll()
        {
            try
            {
                return _context.Notes.ToList();
            }
            catch (Exception)
            {

                return null;
            }
        }
        [HttpGet("{id}")]
        [Authorize]

        public Note? GetById(int id)
        {
            try
            {
                return _context.Notes.SingleOrDefault(e => e.ID == id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut]
        [Authorize]

        public Note? UpdateOne([FromBody] Note entity)
        {
            try
            {
                var existing = GetById(entity.ID ?? 0);
                entity.Last_updated = DateTime.Now;

                _context.Notes.Entry(existing).CurrentValues.SetValues(entity);
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
