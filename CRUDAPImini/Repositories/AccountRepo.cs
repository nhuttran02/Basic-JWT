using AutoMapper;
using CRUDAPImini.Data;
using CRUDAPImini.Models.DTOs;
using CRUDAPImini.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CRUDAPImini.Repositories
{
    public class AccountRepo(AppDBContext appDBContext, IMapper mapper, IConfiguration config) : IAccountRepo
    {
        public async Task<LoginResponse> Login(LoginDTO loginDTO)
        {
            var user = await FindUserByEmail(loginDTO.Email);
            if (user != null)
            {
                bool verifyPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password);
                if (!verifyPassword)
                    return new LoginResponse(false, null, "Invalid credentials");
                string token = GenerateToken(user);
                return new LoginResponse(true, token, null);
            }
            return new LoginResponse(false, null, "user does not exist");
        }

        private async Task<User> FindUserByEmail(string email)
        {
            email = email.ToLower();
            return await appDBContext.Users.FirstOrDefaultAsync(_ => _.Email.ToLower() == email);
        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim("Fullname", user.Name),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
            };
            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Response> Register(RegisterDTO registerDTO)
        {
            var user = await FindUserByEmail(registerDTO.Email);
            if (user != null)
                return new Response(false, "User adready registered");
            var addUser = mapper.Map<User>(registerDTO);
            addUser.Password = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);
            await appDBContext.SaveChangesAsync();
            return new Response(true, "Created");
        }
    }
}
