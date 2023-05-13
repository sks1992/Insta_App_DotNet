using System.ComponentModel.DataAnnotations;

namespace Insta_App.Models
{
    public class Posts
    {
        [Key]
        public int PostId { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserImage { get; set; }
        public string? PostDescription { get; set; }
        public string? PostUrl { get; set; }
        public DateTime? PublishedDate { get; set; }
    }
}
