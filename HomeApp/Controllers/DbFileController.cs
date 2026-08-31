
using HomeApp.Common;
using HomeApp.RestModels;
using HomeApp.SqlModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using Image = SixLabors.ImageSharp.Image;

namespace HomeApp.Controllers
{
    [ApiController]
    [Route("/files")]
    public class DbFileController : Controller
    {
        private HomeDbContext _context;
        private IConfiguration _configuration;

        public DbFileController(IConfiguration configuration, HomeDbContext context)
        {
            _context = context;
            _configuration = configuration;
        }
        [Authorize]


        [HttpGet(Name = "GetFiles")]
        public IEnumerable<DbFile>? GetAll()
        {

            return _context.DbFiles.ToList();
        }
        [Authorize]

        [HttpGet("/type={type}", Name = "GetFilesByType")]
        public IEnumerable<DbFile>? GetAll(string type)
        {

            return _context.DbFiles.Where(e => e.File_type == type);
        }
        [Authorize]

        [HttpGet("{id}", Name = "GetByFileById")]
        public DbFile? GetById(int id)
        {
            return _context.DbFiles
                .Include(e => e.Albums)
                .SingleOrDefault(e => e.ID == id);
        }
        [Authorize]

        [HttpGet("download/{id}", Name = "DownloadsFileByID")]
        public IActionResult? DownloadFileByID(int id)
        {
            FileDetail? fileDetail = GetFileByID(id);
            if (fileDetail == null) return null;
            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileDetail.FilePath, out var contentType))
                contentType = "application/octet-stream";

            var stream = new FileStream(fileDetail.FilePath, FileMode.Open, FileAccess.Read);

            return File(stream, contentType, fileDetail.FileName, true);
        }
        [Authorize]

        [HttpDelete("{id}", Name = "DeleteFileById")]
        public bool DeleteById(int id)
        {
            var destinationFolder = _configuration.GetValue<string>("Paths:FileUploadLocation");
            var destinationFolderOptimized = _configuration.GetValue<string>("Paths:FileUploadLocationOptimized");

            var existing = GetById(id);

            string filePath = Path.Combine(destinationFolder, existing.Stored_file_name);
            string filePathOptimized = Path.Combine(destinationFolderOptimized, existing.Stored_file_name);

            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    System.IO.File.Delete(filePath);
                }
                catch (Exception)
                {

                    return false;
                }

            }
            if (System.IO.File.Exists(filePathOptimized))
            {
                try
                {
                    System.IO.File.Delete(filePathOptimized);
                }
                catch (Exception)
                {

                    return false;
                }
            }


            try
            {
                var entityToDelete = GetById(id);
                _context.SaveChanges();
                _context.DbFiles.Remove(entityToDelete);
                _context.SaveChanges();

                return true;

            }
            catch { return false; }


        }

        /// <summary>
        /// Used to upload files to a single directory structure
        /// </summary>
        /// <param name="file"></param>
        /// <param name="department"></param>
        /// <returns></returns>
        [RequestSizeLimit(500 * 1024 * 1024)]
        [Authorize]

        [HttpPost]
        public async Task<DbFile> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var fileSaved = await SaveFile(file);
            return fileSaved;
        }
        [Authorize]

        [HttpGet("/filedetail={id}")]
        public FileDetail? GetFileByID(int id)
        {
            var destinationFolder = _configuration.GetValue<string>("Paths:FileUploadLocation");

            var entity = GetById(id);

            if (entity == null) return null;
            string filePath = Path.Combine(destinationFolder, entity.Stored_file_name);
            if (System.IO.File.Exists(filePath))
            {
                return new FileDetail
                {
                    FilePath = filePath,
                };
            }
            else
            {
                return null;
            }

        }

        [HttpGet("/videos/{id}")]
        public IActionResult GetVideo(int id)
        {
            var fileDb = GetById(id);

            if (fileDb == null) return NotFound();
            string? destinationOptimized = _configuration.GetValue<string>("Paths:FileUploadLocationOptimized");

            if(!Directory.Exists(destinationOptimized)) return NotFound();

            string fileDestination = Path.Combine(destinationOptimized, fileDb?.Stored_file_name??"");

            if (!System.IO.File.Exists(fileDestination))
                return NotFound();

            var stream = new FileStream(
                fileDestination,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read);

            return File(
                stream,
                "video/mp4",
                enableRangeProcessing: true);
        }

        private async Task<DbFile?> SaveFile(IFormFile file)
        {
            try
            {
                var destinationFolder = _configuration.GetValue<string>("Paths:FileUploadLocation");
                var destinationFolderOptimized = _configuration.GetValue<string>("Paths:FileUploadLocationOptimized");
                var now = DateTime.Now.ToUniversalTime();
                string randomFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{now.ToString("dd-MM-yyyy-H-m-s-fff")}{Path.GetExtension(file.FileName)}";
                if (string.IsNullOrEmpty(destinationFolder)) return null;
                if (string.IsNullOrEmpty(destinationFolderOptimized)) return null;
                if (!Directory.Exists(destinationFolder))
                {
                    try
                    {
                        Directory.CreateDirectory(destinationFolder);
                    }
                    catch (Exception)
                    {

                        return null;
                    }

                }
                if (!Directory.Exists(destinationFolderOptimized))
                {
                    try
                    {
                        Directory.CreateDirectory(destinationFolderOptimized);
                    }
                    catch (Exception)
                    {

                        return null;
                    }

                }
                DbFile dbFile = new DbFile()
                {
                    Description = "",
                    File_size_kb = ((double)file.Length / 1000).ToString("F2"),
                    File_type = Path.GetExtension(file.FileName).ToLower().Replace(".", ""),
                    Original_file_name = file.FileName,
                    Stored_file_name = randomFileName,
                    Upload_date = now,
                    ID = 0,
                };
                var result = _context.Add(dbFile);
                _context.SaveChanges();

                if (result != null)
                {
                    string destination = Path.Combine(destinationFolder, randomFileName);
                    string destinationOptimized = Path.Combine(destinationFolderOptimized, randomFileName);
                    using (var stream = new FileStream(destination, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);// original file size!
                        stream.Close();
                    }
                    string extension = Path.GetExtension(file.FileName).ToLower();

                    if (extension.Contains(".png") || extension.Contains(".jpg") || extension.Contains(".jpeg"))
                    {
                        using var image = await Image.LoadAsync(destination);

                        image.Mutate(x =>
                        {
                            x.AutoOrient();

                            x.Resize(new ResizeOptions
                            {
                                Size = new Size(600, 600),
                                Mode = ResizeMode.Max
                            });
                        });
                        var outPutStream = new FileStream(destinationOptimized, FileMode.Create);
                        await image.SaveAsync(outPutStream, new WebpEncoder
                        {
                            Quality = 80
                        });
                    }

                    return dbFile;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
