using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeApp.SqlModels
{
    [Table("article")]
    public class Article
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]

        public int? ID { get; set; }
        [Column("title")]

        public string Title { get; set; } = "";
        [Column("description")]

        public string Description { get; set; } = "";
        [Column("creation_date")]
        public DateTime? Creation_date { get; set; } = DateTime.MinValue;
        [Column("last_updated")]
        public DateTime? Last_updated { get; set; } = DateTime.MinValue;

        [Column("user_id")]

        public long? UserId { get; set; }
        public User? User { get; set; }
    }
}
