namespace Insta_App.Models
{
    public class Likes
    {
        public int Id { get; set; }
        public int LikeUserKey { get; set; }
        public int PostUserKey { get; set; }
        public int PostKey { get; set; }
        public bool IsLiked { get; set; } = false;
    }
}
