using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("タイトル")]
        public string Title { get; set; }
        [DisplayName("本文")]
        public string Description { get; set; }
        [DisplayName("投稿日")]
        public DateTime PublishedDate { get; set; }
        public string OwnerId { get; set; }
        [DisplayName("下書き")]
        public bool IsDraft { get; set; }
    }
}
