using Insta_App.Models;
using Insta_App.Models.Dto;

namespace Insta_App.DataAccess.Repository.IRepository
{
    public interface IPostRepository
    {
        Task<ApiResponse> SavePost(CreatePostDTO createPost);
        Task<PostResponseDTO> GetPosts(int userId);
        IEnumerable<Posts> GetAllPosts();
    }
}   
