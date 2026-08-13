using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HomeApp.SqlModels
{
    [Table("note")]
    public class Note
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]

        public int? ID { get; set; }        
        [Column("description")]

        public string Description { get; set; } = "";
        [Column("creation_date")]
        public DateTime Creation_date { get; set; } = DateTime.MinValue;
        [Column("last_updated")]
        public DateTime Last_updated { get; set; } = DateTime.MinValue;

        [Column("note_listid")]
        public int? NoteListId { get; set; }
        [JsonIgnore]
        public NoteList? NoteList { get; set; }
    }
}
