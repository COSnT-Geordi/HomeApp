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
    [Table("page")]
    public class Page
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long ID { get; set; }
        [Column("title")]
        public string? Title { get; set; } = "";
        [Column("description")]
        public string? Description { get; set; } = "";
        [Column("page_type")]
        public string? PageType { get; set; } = "";
    
    }
}
