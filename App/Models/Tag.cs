using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public int PostId { get; set; }
        public string TagName { get; set; }

    }
}
