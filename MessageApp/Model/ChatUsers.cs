using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MessageApp.Model
{
    public class ChatUsers : IdentityUser
    {
        public string? UserStatus { get; set; }
        public string? ProfilePhoto { get; set; }
    }
}
