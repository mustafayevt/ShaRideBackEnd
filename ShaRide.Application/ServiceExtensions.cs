using System;
using System.Reflection;
using System.Text;
using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Request;
using ShaRide.Application.Helpers;
using ShaRide.Application.Managers;
using ShaRide.Application.Services.Concrete;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Settings;

namespace ShaRide.Application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("InMemory"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }
            #region Services
            services.AddHttpClient("HttpClient");
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<UserManager>();
            services.AddTransient<IAccountService, AccountService>();
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.Configure<TheTexting>(configuration.GetSection("TheTexting"));
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IVerificationCodeService, VerificationCodeService>();
            services.AddTransient<IVerificationCodeService, VerificationCodeService>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IRestrictionService, RestrictionService>();
            services.AddTransient<IBanTypeService, BanTypeService>();
            services.AddTransient<ICarBrandService, CarBrandService>();
            services.AddTransient<ICarModelService, CarModelService>();
            #endregion
            
            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));

            #region Authentication

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JWTSettings:Issuer"],
                        ValidAudience = configuration["JWTSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                    };
                    o.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.Response.StatusCode = 401;
                            c.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new ApiError("Your token has expired"));
                            return c.Response.WriteAsync(result);
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new ApiError("You are not Authorized"));
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new ApiError("You are not authorized to access this resource"));
                            return context.Response.WriteAsync(result);
                        }
                    };
                });
            
            #endregion
        }
    }
}
