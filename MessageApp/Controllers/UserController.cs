using MessageApp.Data;
using MessageApp.DTO;
using MessageApp.Interfaces;
using MessageApp.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MessageApp.Controllers
{
    [ApiController]
    [Route("api/")]
    public class UserController : Controller
    {
        public readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly MessageAppDbContext _appDbContext;
        public readonly UserManager<ChatUsers> _userManager;
        public readonly SignInManager<ChatUsers> _signInManager;

        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor, MessageAppDbContext appDbContext, UserManager<ChatUsers> userManager, SignInManager<ChatUsers> signInManager)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _appDbContext = appDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterUser([FromBody]RegisterUserDTO registerUser) {
            if (ModelState.IsValid)
            {
                var isUniqueEmail = await _userManager.FindByEmailAsync(registerUser.Email);
                if (isUniqueEmail == null)
                {
                    var result = await _userService.RegisterUser(registerUser);
                    return Ok(result);
                } else
                {
                    return Ok("This email is already registered, please enter a new email! ");
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var errorMessage = string.Join("; ", errors);
                return null;
            }
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginUser(LoginUserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(userDTO.UserName, userDTO.Password, false, false);
                if (result.Succeeded) { }
                else { }
                /*if (result.Succeeded)
                {
                    return Ok("Login Successful");
                }
                else if (result.IsLockedOut)
                {
                    return View("Lockout");
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, "User is not allowed to sign in.");
                }
                else if (result.RequiresTwoFactor)
                {
                    return RedirectToAction("SendCode", new { ReturnUrl = "/", RememberMe = false });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }*/
            }
            return BadRequest();
        }

        //[Authorize]
        [HttpPost]
        [Route("UploadPhoto")]
        public async Task<IActionResult> Upload([FromForm] List<IFormFile> files)
        {
            var profile = await _userService.UploadPhoto(files);
            return Ok(profile);
        }
        //[Authorize]
        [HttpPut]
        [Route("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(string status)
        {
            string currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //string currentUserId = await _appDbContext.ChatUsers.Where(m => m.Id == Id).Select(u => u.Id).FirstOrDefaultAsync();

            var u = await _userService.UpdateStatus(currentUserId, status);
            return Ok(u);
        }
    }
}
