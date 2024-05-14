namespace UserManagementSystem.Services.AuthService
{
    using AutoMapper;
    using Azure.Core;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using UserManagementSystem.DTOs.Auth;
    using UserManagementSystem.Models;
    using UserManagementSystem.Rrepositories.AuthRepository;
    using UserManagementSystem.Services.EmailService;

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IAuthRepository _authRepository;
        private readonly IEmailService _emailService;

        public AuthService(IConfiguration configuration, IMapper mapper, IAuthRepository authRepository, IEmailService emailService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _authRepository = authRepository;
            _emailService = emailService;
        }

        public async Task<string> Register(RegisterDTO userDTO)
        {
            if (userDTO is null)
                throw new Exception("Invalid Data");
            User user = _mapper.Map<User>(userDTO);
            var existingUser = await _authRepository.FindUserByEmailAsync(user.Email!);
            if (existingUser != null)
                throw new Exception("User already exist with this email");
            var isCreated = await _authRepository.CreateAsync(user, userDTO.Password!);
            if (!isCreated.Succeeded)
            {
                var errorMessages = string.Join(Environment.NewLine, isCreated.Errors.Select(e => e.Description));
                throw new Exception($"{Environment.NewLine}{errorMessages}");
            }

            var token = await _authRepository.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);

            var verificationLink = $"{_configuration[Constants.AppUrl]}/Auth/verify?userId={user.Id}&token={encodedToken}";

            // Constructing the HTML hyperlink
            var hyperlink = $"<a href=\"{verificationLink}\">Click here to verify your account</a>";

            _emailService.SendAccountVerificationEmail(user.Email, "Account Verification Email", hyperlink , true);

            return ("Registraion Successfull, Please Verify Your Email");
        }

        public async Task<string> Login(LoginDTO userDTO)
        {
            if (userDTO == null)
                throw new ArgumentNullException(nameof(userDTO), "Login container is empty");

            User getUser = await _authRepository.FindUserByEmailAsync(userDTO.Email);
            if (getUser == null)
                throw new Exception("Invalid email/password");
            if (!await _authRepository.IsEmailConfirmedAsync(getUser))
            {
                throw new Exception("Email not confirmed. Please confirm your email.");
            }

            bool checkUserPasswords = await _authRepository.CheckPasswordAsync(getUser, userDTO.Password);
            if (!checkUserPasswords)
                throw new Exception("Invalid email/password");

            var token = GenerateToken(getUser);

            return (token);
        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[Constants.JwtKey]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.Name,  user.Name!),
                new Claim(ClaimTypes.Email, user.Email!),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration[Constants.JwtIssuer],
                audience: _configuration[Constants.JwtAudience],
                claims: userClaims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> ConfirmEmail(string userId, string token)
        {
            User user = await _authRepository.FindUserByIdAsync(userId);
            IdentityResult result = await _authRepository.ConfirmEmail(user, token);
            if(!result.Succeeded)
            {
                throw new Exception("Email Verfication Error, Please Try Again Later");
            }
            return "Account Verified";
        }
    }

}
