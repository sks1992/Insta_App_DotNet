using Insta_App.DataAccess.Repository.IRepository;
using Insta_App.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insta_App_API.Controllers
{
    [Route("api")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        [HttpPost("addPosts")]
        [Authorize]
        public async Task<IActionResult> AddPost([FromBody] CreatePostDTO createPost)
        {
            var createPostResponse = await _postRepository.SavePost(createPost);
            return Ok(createPostResponse);
        }

        [HttpGet("posts/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetPost(int userId)
        {
            var posts = await _postRepository.GetPosts(userId);
            return Ok(posts);
        }
        [HttpGet("allPosts")]
        [Authorize]
        public IActionResult GetAllPost()
        {
            var posts = _postRepository.GetAllPosts();
            return Ok(posts);
        }
    }
}
