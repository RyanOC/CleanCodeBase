using System.Threading;
using System.Threading.Tasks;
using System;
using System.Text;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Core.Abstractions;
using Core.Entities;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private IConfiguration _configuration;

        public IdentityService(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }
        
        public async Task<string> AuthenticateAsync(LoginCredentials loginCredentials)
        {
            await Task.Run(() =>
            {
                Thread.Sleep(1000); // just to simulate a http identity provider service call delay
            });

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, loginCredentials.UserName),
                new Claim(ClaimTypes.Role, "admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Auth:SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Auth:ValidIssuer"],
                audience: _configuration["Auth:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Auth:ExpireMinutes"])),
                signingCredentials: creds
            );
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}