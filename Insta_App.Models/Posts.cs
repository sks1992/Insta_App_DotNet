﻿using System.ComponentModel.DataAnnotations;

namespace Insta_App.Models
{
    public class Posts
    {
        [Key]
        public int PostId { get; set; }
        public int? UserId { get; set; }
        public string? PostDescription { get; set; }
        public string? PostImage { get; set; }
        public DateTime PublishedDate { get; set; } = DateTime.Now;
    }
}
