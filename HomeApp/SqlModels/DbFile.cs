using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HomeApp.SqlModels
{
    [Table("db_file")]
    public class DbFile
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long ID { get; set; }
        [Column("title")]

        public string? Title { get; set; } = "";
        [Column("description")]

        public string? Description { get; set; } = "";
        [Column("original_file_name")]

        public string? Original_file_name { get; set; } = "";
        [Column("stored_file_name")]

        public string? Stored_file_name { get; set; } = "";
        [Column("file_type")]

        public string? File_type { get; set; } = "";
        [Column("file_size_kb")]

        public string? File_size_kb { get; set; } = "";
        [Column("upload_date")]

        public DateTime Upload_date { get; set; }
        [JsonIgnore]
        public List<Album> Albums { get; set; } = new List<Album>();


    }
}
