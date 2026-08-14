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
    [Table("user")]
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long ID { get; set; }
        [Column("username")]

        public string? Username { get; set; } = "";
        [Column("password")]

        public string? Password { get; set; } = "";

        [Column("creation_date")]

        public DateTime Creation_date { get; set; }


        public List<Article> Articles { get; set; } = [];

        public List<Album> Albums { get; set; } = [];

    }
}
