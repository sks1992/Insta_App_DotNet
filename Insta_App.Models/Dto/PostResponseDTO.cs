namespace Insta_App.Models.Dto
{
    public class PostResponseDTO : ApiResponse
    {
        public IEnumerable<Posts> Posts { get; set; } = Enumerable.Empty<Posts>();
    }
}
