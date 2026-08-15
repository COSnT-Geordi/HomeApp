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
    [Table("user_sharing")]
    public class UserSharing
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long ID { get; set; }
        [Column("title")]

        public string? Title { get; set; } = "";
        [Column("description")]

        public string? Description { get; set; } = "";        
       
        [Column("everyone_can_see")]

        public bool EveryoneCanSee { get; set; }
        [Column("user_id")]

        public long? UserId { get; set; }      


        public List<UserSharingAlbum> UserSharingAlbums { get; set; } = [];
        public List<UserSharingArticle> UserSharingArticles { get; set; } = [];
        public List<UserSharingDbFile> UserSharingDbFiles { get; set; } = [];
        public List<UserSharingUser> UserSharingUsers { get; set; } = [];

        [JsonIgnore]
        public User? User { get; set; }
    }
}
