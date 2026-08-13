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
    [Table("album")]
    public class Album
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long ID { get; set; }
        [Column("title")]

        public string? Title { get; set; } = "";
        [Column("description")]

        public string? Description { get; set; } = "";        
       
        [Column("creation_date")]

        public DateTime Creation_date { get; set; }

        public List<DbFile> DbFiles { get; set; } = new List<DbFile>();
    }
}
