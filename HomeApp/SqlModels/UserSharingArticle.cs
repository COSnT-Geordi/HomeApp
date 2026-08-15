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
    [Table("user_sharing_article")]
    public class UserSharingArticle
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long ID { get; set; }
        [Column("user_sharing_id")]
        public long? UserSharingID { get; set; }
        [Column("article_id")]
        public long? ArticleID { get; set; }

        public Article? Article { get; set; }
        [JsonIgnore]
        public UserSharing? UserSharing { get; set; }
    }
}
