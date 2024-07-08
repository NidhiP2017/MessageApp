using MessageApp.DTO;
using Microsoft.AspNetCore.Mvc;

namespace MessageApp.Interfaces
{
    public interface IUserService 
    {
        Task<RegisteredUserResponseDTO> RegisterUser(RegisterUserDTO registerUser);

        Task<IActionResult> UploadPhoto(List<IFormFile> files);

        Task<IActionResult> UpdateStatus(string Id, string status);

        Task<IActionResult> AuthenticateUser(LoginUserDTO usersDTO);

    }
}
