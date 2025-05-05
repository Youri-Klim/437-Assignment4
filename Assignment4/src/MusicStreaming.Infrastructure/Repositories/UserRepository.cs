using Microsoft.EntityFrameworkCore;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
using MusicStreaming.Core.Entities;
using MusicStreaming.Infrastructure.Data;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStreaming.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MusicStreamingDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(MusicStreamingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto> GetByIdAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetByUsernameAsync(string username)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<IReadOnlyList<UserDto>> ListAllAsync()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<IReadOnlyList<UserDto>>(users);
        }

        public async Task<string> CreateAsync(string username, string email, string password)
        {
            var id = Guid.NewGuid().ToString();
            var user = new User(id, username, email, DateTime.Now);
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return id;
        }

        public async Task UpdateAsync(UserDto userDto)
        {
            var user = await _context.Users.FindAsync(userDto.Id);
            if (user != null)
            {
                // Using reflection since User has private setters
                var entityType = _context.Entry(user).Entity.GetType();
                var usernameProperty = entityType.GetProperty("Username");
                var emailProperty = entityType.GetProperty("Email");
                
                usernameProperty?.SetValue(user, userDto.Username);
                emailProperty?.SetValue(user, userDto.Email);
                
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}