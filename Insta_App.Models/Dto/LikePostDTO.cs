namespace Insta_App.Models.Dto
{
    public class LikePostDTO
    {
        public int LikeUserKey { get; set; }
        public int PostUserKey { get; set; }
        public int PostKey { get; set; }
        public bool IsLiked { get; set; } = false;
    }
}
