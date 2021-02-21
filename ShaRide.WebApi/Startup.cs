using System.Collections.Generic;
using System.Globalization;
using AutoWrapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShaRide.Application;
using ShaRide.Application.Localize;
using ShaRide.Application.Services.Interface;
using ShaRide.WebApi.Extensions;
using ShaRide.WebApi.Services;

namespace ShaRide.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApplicationLayer(Configuration);
            services.AddAuthorization();
            services.AddSwaggerExtension();
            services.AddHealthChecks();
            services.AddTransient<IAuthenticatedUserService, AuthenticatedUserService>();
            services.AddLocalization();
            services.AddCors();
            services.AddMvc()
                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(Resource));
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCors(builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
            app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions
            {
                ShowStatusCode = true,
                ShowIsErrorFlagForSuccessfulResponse = true,
                EnableResponseLogging = true,
                LogRequestDataOnException = true,
                IgnoreNullValue = true,
                IsApiOnly = false
            });
            var cultures = new List<CultureInfo>
            {
                new CultureInfo("en"),
                new CultureInfo("az")
            };
            app.UseRequestLocalization(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwaggerExtension();
            app.UseErrorHandlingMiddleware();
            app.UseHealthChecks("/health");
            app.UseEndpoints(endpoints =>
            {
                var pipeline = endpoints.CreateApplicationBuilder().Build();
            
                endpoints.Map("/swagger", pipeline).RequireAuthorization(new AuthorizeAttribute {AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme});
                endpoints.Map("/swagger/index.html", pipeline).RequireAuthorization(new AuthorizeAttribute {AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme});
                endpoints.Map("/swagger/v1/swagger.json", pipeline).RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme });
                endpoints.Map("/swagger/{documentName}/swagger.json", pipeline).RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme });
                endpoints.MapControllers();
            });
        }
    }
}