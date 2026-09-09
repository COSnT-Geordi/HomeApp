using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HomeApp.SqlModels
{
    [Table("article_category")]
    public class ArticleCategory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]

        public long? ID { get; set; }
        [Column("title")]

        public string Title { get; set; } = "";
        [Column("description")]
        public string Description { get; set; } = "";
        [JsonIgnore]
        public List<Article> Articles { get; set; } = new List<Article>();
       
    }
}
