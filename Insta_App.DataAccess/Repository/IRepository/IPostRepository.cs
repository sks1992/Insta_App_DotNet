using Insta_App.Models;
using Insta_App.Models.Dto;

namespace Insta_App.DataAccess.Repository.IRepository
{
    public interface IPostRepository
    {
        Task<ApiResponse> SavePost(CreatePostDTO createPost);
        Task<PostResponseDTO> GetPostsById(int userId); 
        IEnumerable<Posts> GetAllPosts();
        Task<LikePostResponseDTO> LikePost(LikePostDTO likePost);
    }
}   
        