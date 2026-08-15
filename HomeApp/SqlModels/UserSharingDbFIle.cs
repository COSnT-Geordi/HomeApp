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
    [Table("user_sharing_db_file")]
    public class UserSharingDbFile
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long ID { get; set; }
        [Column("user_sharing_id")]
        public long? UserSharingID { get; set; }
        [Column("db_file_id")]
        public long? DbFileID { get; set; }

        public DbFile? DbFile { get; set; }
        [JsonIgnore]
        public UserSharing? UserSharing {  get; set; }
    }
}
