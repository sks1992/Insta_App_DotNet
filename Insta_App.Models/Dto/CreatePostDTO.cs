namespace Insta_App.Models.Dto
{
    public class CreatePostDTO
    {
        public int? UserId { get; set; }
        public string? PostImageUrl { get; set; }
        public string? PostDescription { get; set; }
    }
}
