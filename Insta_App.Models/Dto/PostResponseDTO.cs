namespace Insta_App.Models.Dto
{
    public class PostResponseDTO :ApiResponse
    {
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserProfileImage { get; set;}
        public IEnumerable<Posts> Posts { get; set; } = Enumerable.Empty<Posts>();
    }
}
