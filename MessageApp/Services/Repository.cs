using MessageApp.Data;
using MessageApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace MessageApp.Services
{
    public class Repository : IRepository
    {
        private readonly MessageAppDbContext _appContext;
        public Repository() { }
        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return await _appContext.ChatUsers.AllAsync(u => u.Email != email);
        }
    }
}
