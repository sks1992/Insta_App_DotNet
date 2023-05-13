namespace Insta_App.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPassword { get; set; }
        public string? UserBio { get; set; }
        public string? UserImage { get; set; }
        public DateTime UserCreatedAt { get; set; } = DateTime.Now;
        public string? Token { get; set; }
    }
}
