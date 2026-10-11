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
    [Route("/sharings")]
    public class UserSharingController : Controller, ICrudController<UserSharing>
    {
        private HomeDbContext _context;
        public UserSharingController(HomeDbContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpPost]
        public UserSharing? CreateOne([FromBody] UserSharing entity)
        {
            var principal = ControllerHelper.GetTokenFromRequest(Request);
            var owner = _context.Users.SingleOrDefault(e => e.ID == principal.UserID);
            if (owner == null) return null;

            try
            {
                entity.ID = 0;
                entity.UserSharingAlbums.Clear();
                entity.EveryoneCanSee = false;
                entity.User = owner;
                var created = _context.UserSharings.Add(entity);
                _context.SaveChanges();
                return created.Entity;
            }
            catch (Exception)
            {

                return null;
            }
        }
        [HttpGet("shared")]
        public List<UserSharing> GetSharedWithMe()
        {

            var principal = ControllerHelper.GetTokenFromRequest(Request);
            if (principal == null) return null;
            try
            {

                return _context.UserSharings
                     .Include(e => e.UserSharingUsers)// Includes the users shared to
                     .Where(e => e.UserSharingUsers.Any(usu => usu.UserID == principal.UserID)) // Get only the users where the requested user id is in the user sharing user list
                     .ToList();
            }
            catch (Exception)
            {

                return new List<UserSharing>();
            }
        }
        [HttpGet("shared/{id}")]
        public UserSharing GetShareWithMeDetailed(long id)
        {

            var principal = ControllerHelper.GetTokenFromRequest(Request);
            if (principal == null) return null;
            try
            {

                return _context.UserSharings
                     .Include(e => e.UserSharingUsers)
                     .Include(e => e.UserSharingAlbums).ThenInclude(e => e.Album)
                     .Include(e => e.UserSharingArticles).ThenInclude(e => e.Article)
                     .Include(e => e.UserSharingDbFiles).ThenInclude(e => e.DbFile)
                     .Where(e => e.UserSharingUsers.Any(usu => usu.UserID == principal.UserID))
                     .Single(e => e.ID == id);
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

                _context.UserSharings.Remove(existing);
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
        public List<UserSharing>? GetAll()
        {
            var principal = ControllerHelper.GetTokenFromRequest(Request);
            var owner = _context.Users
                .Include(e => e.UserSharings)
                .SingleOrDefault(e => e.ID == principal.UserID);

            if (owner == null) return new List<UserSharing>();
            try
            {
                return owner.UserSharings;
            }
            catch (Exception)
            {

                return null;
            }
        }
        [Authorize]

        [HttpGet("{id}")]

        public UserSharing? GetById(int id)
        {
            var principal = ControllerHelper.GetTokenFromRequest(Request);
            var owner = _context.Users
                .Include(e => e.UserSharings)
                .ThenInclude(e => e.UserSharingAlbums)
                .Include(e => e.UserSharings)
                .ThenInclude(e => e.UserSharingArticles)
                .Include(e => e.UserSharings)
                .ThenInclude(e => e.UserSharingDbFiles)
                .Include(e => e.UserSharings)
                .ThenInclude(e => e.UserSharingUsers)
                .SingleOrDefault(e => e.ID == principal.UserID);

            if (owner == null) return null;

            try
            {
                return owner.UserSharings
                    .SingleOrDefault(e => e.ID == id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut]
        [Authorize]

        public UserSharing? UpdateOne([FromBody] UserSharing entity)
        {
            var Principal = ControllerHelper.GetTokenFromRequest(Request);
            if (Principal == null) return null;

            try
            {
                var existing = GetById((int)entity.ID);
                _context.UserSharings.Entry(existing).CurrentValues.SetValues(entity);





                ContextTool.UpdateRelations<UserSharingDbFile>(existing.UserSharingDbFiles, entity.UserSharingDbFiles);
                ContextTool.UpdateRelations<UserSharingAlbum>(existing.UserSharingAlbums, entity.UserSharingAlbums);
                ContextTool.UpdateRelations<UserSharingArticle>(existing.UserSharingArticles, entity.UserSharingArticles);
                ContextTool.UpdateRelations<UserSharingUser>(existing.UserSharingUsers, entity.UserSharingUsers);

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
