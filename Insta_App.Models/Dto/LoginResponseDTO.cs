namespace Insta_App.Models.Dto
{
    public class LoginResponseDTO :ApiResponse
    {
        public string? Token { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserImage { get; set; }
    }
}
