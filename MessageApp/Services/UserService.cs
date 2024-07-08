using MessageApp.Data;
using MessageApp.DTO;
using MessageApp.Interfaces;
using MessageApp.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using System.Text;

namespace MessageApp.Services
{
    public class UserService : IUserService
    {
        private readonly MessageAppDbContext _dbContext;
        private readonly IRepository _rep;
        private readonly UserManager<ChatUsers> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserService(MessageAppDbContext dbContext, IRepository rep, UserManager<ChatUsers> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _rep = rep;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;   
        }

        public async Task<RegisteredUserResponseDTO> RegisterUser(RegisterUserDTO registerUser)
        {
           var user = new ChatUsers
                {
                    UserName = registerUser.UserName,
                    Email = registerUser.Email
                };
                var result = await _userManager.CreateAsync(user, registerUser.Password);

                if (result.Succeeded)
                {
                    var response = new RegisteredUserResponseDTO
                    {
                        UserName = registerUser.UserName,
                        Email = registerUser.Email
                    };
                    return response;
                }
                else
                {
                    return null;
                }
        }

        public async Task<IActionResult> UploadPhoto(List<IFormFile> files)
        {
            //var Id = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //string currentUserId = await _appContext.ChatUsers.Where(m => m.Id == Id).Select(u => u.Id).FirstOrDefaultAsync();

            if (files.Count == 0)
                return new OkObjectResult("No file was uploaded");

            string directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "ProfilePhotos");
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            foreach (var file in files)
            {
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return new OkObjectResult("Invalid file type.");
                }
                string filePath = Path.Combine(directoryPath, file.FileName + "_" + "baad0ce3-7f72-47eb-ae7f-59db19143f22");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                var user = await _dbContext.ChatUsers.FindAsync("baad0ce3-7f72-47eb-ae7f-59db19143f22");
                user.ProfilePhoto = Path.Combine(directoryPath, file.FileName);
                await _dbContext.SaveChangesAsync();
            }
            return new OkObjectResult("Upload Successful");
        }

        public async Task<IActionResult> UpdateStatus(string Id, string status)
        {
            if (Id != null)
            {
                var user = await _dbContext.ChatUsers.FindAsync(Id);
                user.UserStatus = status;
                await _dbContext.SaveChangesAsync();
                return new OkObjectResult("Status updated to: " + status);
            }
            else
            {
                return new OkObjectResult("Could not find user");
            }
        }
        public async Task<IActionResult> AuthenticateUser(LoginUserDTO userDTO)
        {
            return null;
        }
    }
}
