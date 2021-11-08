using Filter;
using Core.Component.Library.WebTools;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Helper
{
    [SwaggerExclude]
    public class JwtWeb
    {
        public string JwtKey;
        public string JwtIssuer;
        public Dictionary<string, object> GenerateJSONWebToken(Dictionary<string,object> userInfo, IConfiguration configuration)
        {
            SetConfiguration(configuration);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var TrasactionDate = DateTime.Now;
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.GetString("name")),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.GetString("email")),
                new Claim("TransactionDate", TrasactionDate.ToString("yyyy-MM-dd")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var reqToken = new JwtSecurityToken(JwtIssuer,
                JwtIssuer,
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(reqToken);
            userInfo.Add("token", token);
            userInfo.Add("transactionDate", TrasactionDate.ToString("yyyy-MM-dd"));
            return userInfo;
        }

        public void SetConfiguration(IConfiguration configuration)
        {
            JwtKey = configuration["Settings:JwtSettings:Key"];
            JwtIssuer = configuration["Settings:JwtSettings:Issuer"];
        }
    }
}
