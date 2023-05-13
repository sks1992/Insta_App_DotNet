using System.ComponentModel.DataAnnotations;

namespace Insta_App.Models.Dto
{
    public class CreateUserDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string UserPassword { get; set; }
        [Required]
        public string UserBio { get; set; }
        [Required]
        public string UserImage { get; set; }
    }
}
