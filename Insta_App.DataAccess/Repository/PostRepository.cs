using Insta_App.DataAccess.Data;
using Insta_App.DataAccess.Repository.IRepository;
using Insta_App.Models;
using Insta_App.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Insta_App.DataAccess.Repository
{
    public class PostRepository : IPostRepository
    {
        private ApplicationDbContext _db;
        protected ApiResponse _response;
        public PostRepository(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }
        public async Task<ApiResponse> SavePost(CreatePostDTO createPost)
        {
            if (createPost.UserId == null || createPost.PostDescription.IsNullOrEmpty() || createPost.PostImageUrl.IsNullOrEmpty())
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = "Please Fill All values";
                return _response;
            }

            var post = _db.Posts.AsEnumerable();
            var uploadedFilePath = "";
            if (post == null)
            {
                uploadedFilePath = Path.Combine(@"D:\Images\Insta\posts", $"{1}{".jpg"}");
            }
            else
            {
                uploadedFilePath = Path.Combine(@"D:\Images\Insta\posts", $"{post.Count() + 1}{".jpg"}");
            }


            if (createPost.PostImageUrl != null)
            {
                byte[] fileByteArray = Convert.FromBase64String(createPost.PostImageUrl);
                File.WriteAllBytes(uploadedFilePath, fileByteArray);
            }

            var user = _db.User.Where(u => u.UserId == createPost.UserId).FirstOrDefault();
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = "User Not Exist. Please Enter Correct UserId";
                return _response;
            }

            var posts = new Posts()
            {
                PostDescription = createPost.PostDescription,
                PostImage = uploadedFilePath,
                UserId = createPost.UserId,
                UserName = user.UserName,
                UserImageUrl = user.UserImage,
            };

            await _db.Posts.AddAsync(posts);
            await _db.SaveChangesAsync();

            _response.IsSuccess = true;
            return _response;
        }

        public async Task<PostResponseDTO> GetPostsById(int userId)
        {

            byte[] imageBytes;
            string base64String;
            string base64;

            if (userId <= 0)
            {
                var post = new PostResponseDTO()
                {
                    IsSuccess = false,
                    ErrorMessage = "Please Enter Correct User Id"
                };
                return post;
            }

            var user = await _db.User.Where(u => u.UserId == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                var post = new PostResponseDTO()
                {
                    IsSuccess = false,
                    ErrorMessage = "User Not  Exists"
                };
                return post;
            }
            imageBytes = File.ReadAllBytes(user.UserImage!);
            base64String = Convert.ToBase64String(imageBytes);
            base64 = "data:image/jpg;base64," + base64String;
            user.UserImage = base64;

            var postList = await _db.Posts.Where(u => u.UserId == userId).ToListAsync();

            foreach (var post in postList)
            {
                imageBytes = File.ReadAllBytes(post.PostImage!);
                base64String = Convert.ToBase64String(imageBytes);
                base64 = "data:image/jpg;base64," + base64String;
                post.PostImage = base64;
            }

            var posts = new PostResponseDTO()
            {
                IsSuccess = true,
                Posts = postList,
            };
            return posts;
        }

        public IEnumerable<Posts> GetAllPosts()
        {
            byte[] imageBytes;
            string base64String;
            string base64;

            var postList = _db.Posts.AsEnumerable();

            foreach (var post in postList)
            {
                imageBytes = File.ReadAllBytes(post.PostImage!);
                base64String = Convert.ToBase64String(imageBytes);
                base64 = "data:image/jpg;base64," + base64String;
                post.PostImage = base64;

                imageBytes = File.ReadAllBytes(post.UserImageUrl!);
                base64String = Convert.ToBase64String(imageBytes);
                base64 = "data:image/jpg;base64," + base64String;
                post.UserImageUrl = base64;
            }
            return postList;
        }

        public async Task<LikePostResponseDTO> LikePost(LikePostDTO likePost)
        {
            if (likePost.LikeUserKey == 0 || likePost.PostKey == 0 || likePost.PostUserKey ==0)
            {
                var response = new LikePostResponseDTO()
                {
                    IsSuccess = false,
                    ErrorMessage = "Please Fill Correct values, Either LikeUserId or PostUserId or PostId Is incorrect",
                    likePost = likePost.IsLiked
                };

                return response;
            }

            var likeData = await _db.Likes.Where(u => u.LikeUserKey == likePost.LikeUserKey && u.PostKey == likePost.PostKey).FirstOrDefaultAsync();

            if (likeData == null)
            {
                var like = new Likes()
                {
                    PostKey = likePost.PostKey,
                    LikeUserKey = likePost.LikeUserKey,
                    IsLiked = likePost.IsLiked,
                    PostUserKey = likePost.PostUserKey,
                };

                await _db.Likes.AddAsync(like);
                await _db.SaveChangesAsync();
            }
            else
            {
                likeData.LikeUserKey = likePost.LikeUserKey;
                likeData.PostKey = likePost.PostKey;
                likeData.IsLiked = likePost.IsLiked;
                likeData.PostUserKey = likePost.PostUserKey;

                _db.Likes.Update(likeData);
                await _db.SaveChangesAsync();
            }

            var post = await _db.Posts.Where(u => u.PostId == likePost.PostKey).FirstOrDefaultAsync();
            if (post == null)
            {
                var response = new LikePostResponseDTO()
                {
                    IsSuccess = false,
                    ErrorMessage = "No Post Found,Please Enter Correct PostID",
                    likePost = likePost.IsLiked
                };
                return response;
            }

            var likeCount = await _db.Likes.Where(u => u.PostKey == likePost.PostKey && likePost.IsLiked == true).ToListAsync();

            post.PostLikes = likeCount.Count();
            await _db.SaveChangesAsync();

            var response1 = new LikePostResponseDTO()
            {
                IsSuccess = true,
                ErrorMessage = "",
                likePost = likePost.IsLiked
            };
            return response1;
        }
    }
}
