using Insta_App.DataAccess.Data;
using Insta_App.DataAccess.Repository.IRepository;
using Insta_App.Models;
using Insta_App.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Insta_App.DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext _db;
        protected ApiResponse _response;
        private string secrateKey;
        public UserRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _response = new ApiResponse();
            secrateKey = configuration.GetValue<string>("ApiSettings:Secret");
        }
        public async Task<ApiResponse> RegisterUser(CreateUserDTO createUserDTO)
        {
            var user = await _db.User.FirstOrDefaultAsync(x => x.UserName == createUserDTO.UserName);

            if (user != null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = "UserName Already Exist";
                return _response;
            }
            if (createUserDTO.UserEmail == "" || createUserDTO.UserName == "" || createUserDTO.UserPassword == "" || createUserDTO.UserImage == "" || createUserDTO.UserBio == "") {
                _response.IsSuccess = false;
                _response.ErrorMessage = "Please Fill all values";
                return _response;
            }

            string uploadedFilePath = Path.Combine(@"D:\Images\Insta", $"{createUserDTO.UserName.ToLower()}{".jpg"}");
            if (createUserDTO.UserImage != null)
            {
                byte[] fileByteArray = System.Convert.FromBase64String(createUserDTO.UserImage);
                System.IO.File.WriteAllBytes(uploadedFilePath, fileByteArray);
            }

            User userData = new User()
            {
                UserName = createUserDTO.UserName.ToLower(),
                UserEmail = createUserDTO.UserEmail,
                UserPassword = createUserDTO.UserPassword,
                UserBio = createUserDTO.UserBio,
                UserImage = uploadedFilePath,
            };

            await _db.User.AddAsync(userData);
            await _db.SaveChangesAsync();
            _response.IsSuccess = true;
            return _response;
        }

        public async Task<LoginResponseDTO> Login(LoginUserDTO loginUser)
        {
            var response = new LoginResponseDTO();
            //check if login user is fill all value
            if (loginUser.UserName == "" || loginUser.Password == "")
            {
                response = new()
                {
                    IsSuccess = false,
                    ErrorMessage = "Please Fill All Fields"
                };
                return response;
            }
            //get user from db
            var user = await _db.User.FirstOrDefaultAsync(x => x.UserName == loginUser.UserName.ToLower() && x.UserPassword == loginUser.Password);
            // if user is null ?
            if (user == null)
            {
                response = new()
                {
                    IsSuccess = false,
                    ErrorMessage = "UserName Not Exist. Please register first!"
                };
                return response;
            }

            byte[] imageBytes = File.ReadAllBytes(user.UserImage!);
            string base64String = Convert.ToBase64String(imageBytes);
            string base64 = "data:image/jpg;base64," + base64String;


            //if user was found generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            //this will convert secretKey in ByteArray and store in key variable
            var key = Encoding.ASCII.GetBytes(secrateKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.UserId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);

            user.Token = tokenHandler.WriteToken(token);
            _db.User.Update(user);
            await _db.SaveChangesAsync();

            response = new()
            {
                UserId = user.UserId,
                Token = tokenHandler.WriteToken(token),
                UserImage = base64,
                IsSuccess = true,
                UserName = user.UserName,
                UserBio = user.UserBio
            };
            return response;
        }
    }
}
