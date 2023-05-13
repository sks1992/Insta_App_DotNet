using Insta_App.Models;
using Insta_App.Models.Dto;

namespace Insta_App.DataAccess.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<ApiResponse> RegisterUser(CreateUserDTO createUserDTO);
        Task<LoginResponseDTO> Login(LoginUserDTO loginUser);
    }
}
