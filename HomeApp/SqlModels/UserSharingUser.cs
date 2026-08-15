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
    [Table("user_sharing_user")]
    
    public class UserSharingUser
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long ID { get; set; }

        [Column("user_sharing_id")]
        public long? UserSharingID { get; set; }
        [Column("user_id")]
        public long? UserID { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
        [JsonIgnore]
        public UserSharing? UserSharing {  get; set; }
    }
}
