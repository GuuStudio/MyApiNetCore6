using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyApiNetCore6.Data;
using MyApiNetCore6.Helpers;
using MyApiNetCore6.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyApiNetCore6.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountRepository(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager) { 
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }
        public async Task<string> SignInAsync(SignInModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            bool passwordValid = await _userManager.CheckPasswordAsync(user!, model.Password);
            if (user == null || !passwordValid) {
                return string.Empty;
            }
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach ( var role in userRoles) { 
                authClaims.Add(new Claim(ClaimTypes.Role , role.ToString()));
            }
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey,
                    SecurityAlgorithms.HmacSha512Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
        {
            if (model == null) throw new ArgumentNullException("model");
            if (model.Password == null) throw new ArgumentNullException();
            if(model.Password == model.ConfirmPassword)
            {
                var user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email
                };
                var result=  await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if(!await _roleManager.RoleExistsAsync(AppRole.Customer))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(AppRole.Customer));
                    }
                    await _userManager.AddToRoleAsync(user, AppRole.Customer);
                }
                return result;
            } else
            {
                throw new Exception();
            }
        }
    }
}
