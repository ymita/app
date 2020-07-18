using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    [Table("Posts_Tags_XREF")]
    public class PostTagCrossReference
    {
        [Key]
        public int Id { get; set; }
        public int PostId { get; set; }
        public int TagId { get; set; }
    }
}
