using System;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class YigimKeyAttribute : Attribute, IAsyncActionFilter
    {
        private const string APIKEYNAME = "x-api-yigim-key";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
            {
                throw new ApiException("Api key is not provided.",401);
            }
 
            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
 
            var apiKey = appSettings.GetValue<string>(APIKEYNAME);
 
            if (!apiKey.Equals(extractedApiKey))
            {
                throw new ApiException("Api key is not provided.",401);
            }
 
            await next();
        }
    }
}