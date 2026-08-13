using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeApp.SqlModels
{
    [Table("album_db_file")]
    public class AlbumDbFile
    {
        [Column("album_id")]

        public long AlbumId { get; set; }
        [Column("db_file_id")]

        public long DbFileId { get; set; }

        public Album Album { get; set; } = null!;
        public DbFile DbFile { get; set; } = null!;
    }
}
