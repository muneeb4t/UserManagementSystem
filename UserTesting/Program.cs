
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using UserManagementSystem.Data;
using UserManagementSystem.Models;
using UserManagementSystem.Rrepositories.AuthRepository;
using UserManagementSystem.Rrepositories.UserRepository;
using UserManagementSystem.Services.AuthService;
using UserManagementSystem.Services.EmailService;
using UserManagementSystem.Services.UserServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UserManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(Constants.DefaultConnectionDB)));

            builder.Services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedEmail = true)
                .AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders()
                .AddSignInManager();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration[Constants.JwtIssuer],
                    ValidAudience = builder.Configuration[Constants.JwtAudience],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[Constants.JwtKey]!))
                };
            });

            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = ActionContext =>
                {
                    var modelState = ActionContext.ModelState.Values;
                    List<string> errors = modelState.SelectMany(state => state.Errors.Select(error => error.ErrorMessage)).ToList();
                    return new BadRequestObjectResult(new ServiceResponse<List<string> , string>
                    {
                        Status = (int)HttpStatusCode.BadRequest,
                        Message = errors,
                        Body = null
                    });
                };
            });
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<customUnathorizedResponseMiddleware>();
            //app.UseMiddleware<ResponseFormattingMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

    }
}
