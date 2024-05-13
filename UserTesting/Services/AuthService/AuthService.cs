namespace UserManagementSystem.Services.AuthService
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using UserManagementSystem.DTOs.Auth;
    using UserManagementSystem.Models;

    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(UserManager<User> userManager, IConfiguration configuration, IMapper mapper)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<string> Register(RegisterDTO userDTO)
        {
            if (userDTO is null)
                throw new Exception("Invalid Data");
            User user = _mapper.Map<User>(userDTO);
            var existingUser = await _userManager.FindByEmailAsync(user.Email!);
            if (existingUser != null)
                throw new Exception("User already exist with this email");
            var isCreated = await _userManager.CreateAsync(user, userDTO.Password!);
            if (!isCreated.Succeeded)
            {
                var errorMessages = string.Join(Environment.NewLine, isCreated.Errors.Select(e => e.Description));
                throw new Exception($"Something went wrong: {Environment.NewLine}{errorMessages}");
            }

            return ("Registraion Successfull");
        }

        public async Task<string> Login(LoginDTO userDTO)
        {
            if (userDTO == null)
                throw new ArgumentNullException(nameof(userDTO), "Login container is empty");

            var getUser = await _userManager.FindByEmailAsync(userDTO.Email);
            if (getUser == null)
                throw new Exception("Invalid email/password");

            var checkUserPasswords = await _userManager.CheckPasswordAsync(getUser, userDTO.Password);
            if (!checkUserPasswords)
                throw new Exception("Invalid email/password");

            var token = GenerateToken(getUser);

            return (token);
        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.Name,  user.Name!),
                new Claim(ClaimTypes.Email, user.Email!),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
