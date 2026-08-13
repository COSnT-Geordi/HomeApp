using HomeApp.Common;
using HomeApp.Interfaces;
using HomeApp.SqlModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeApp.Controllers
{
    [ApiController]
    [Route("/notelists")]
    public class NoteListController : Controller, ICrudController<NoteList>
    {
        private HomeDbContext _context;
        public NoteListController(HomeDbContext context)
        {
            _context = context;
        }
        [Authorize]

        [HttpPost]
        public NoteList? CreateOne([FromBody] NoteList entity)
        {
            try
            {
                entity.ID = null;
                entity.Creation_date = DateTime.Now;

                var created = _context.NoteLists.Add(entity);
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
        public List<NoteList>? GetAll()
        {
            try
            {
                return _context.NoteLists.ToList();
            }
            catch (Exception)
            {

                return null;
            }
        }
        [HttpGet("{id}")]
        [Authorize]

        public NoteList? GetById(int id)
        {
            try
            {
                return _context.NoteLists.Include(e=>e.Notes).SingleOrDefault(e => e.ID == id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut]
        [Authorize]

        public NoteList? UpdateOne([FromBody] NoteList entity)
        {
            try
            {
                var existing = GetById(entity.ID ?? 0);
                entity.Last_updated = DateTime.Now;

                _context.NoteLists.Entry(existing).CurrentValues.SetValues(entity);
                ContextTool.UpdateRelations<Note>(existing.Notes, entity.Notes);
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
