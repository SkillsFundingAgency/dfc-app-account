using DFC.App.Account.Helpers;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using DFC.App.Account.Services.DSS.Services;
using DFC.App.Account.Services.SHC.Interfaces;
using DFC.App.Account.Services.SHC.Models;
using DFC.App.Account.Services.SHC.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using DFC.App.Account.Models.AddressSearch;
using DFC.App.Account.Services.Interfaces;
using DFC.App.Account.Application.Common.Services;
using DFC.App.Account.Services.AzureB2CAuth.Interfaces;
using DFC.App.Account.Services.AzureB2CAuth;
using DFC.App.Account.Services.Auth.Models;

namespace DFC.App.Account
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
           
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();

            services.AddControllersWithViews();
            services.AddScoped<IDssReader, DssService>();
            services.AddScoped<IDssWriter, DssService>();
            services.AddScoped<IAddressSearchService, GetAddressIoSearchService>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISkillsHealthCheckService, SkillsHealthCheckService>();
            services.AddScoped<IHttpWebRequestFactory, HttpWebRequestFactory>();
            services.Configure<DssSettings>(Configuration.GetSection(nameof(DssSettings)));
            services.Configure<CompositeSettings>(Configuration.GetSection(nameof(CompositeSettings)));
            services.Configure<AddressSearchServiceSettings>(
                Configuration.GetSection(nameof(AddressSearchServiceSettings)));
            services.Configure<ShcSettings>(Configuration.GetSection(nameof(ShcSettings)));

            services.AddScoped<IOpenIDConnectClient, AzureB2CAuthClient>();
            services.Configure<OpenIDConnectSettings>(Configuration.GetSection("OIDCSettings"));

            //    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(cfg =>
            //{
            //    cfg.TokenValidationParameters =
            //            new TokenValidationParameters
            //            {
            //                ValidateIssuer = true,
            //                ValidateAudience = true,
            //                ValidateIssuerSigningKey = true,

            //                  /*
            //                   * if ValidateLifetime is set to true and the jwt is expired according to to both the ClockSkew and also the expiry on the jwt,then token is invalid
            //                   * This will mark the User.IsAuthenticated as false
            //                  */
            //                ValidateLifetime = true,
            //                ClockSkew = TimeSpan.FromMinutes(1),

            //                ValidIssuer = Configuration["TokenProviderOptions:Issuer"],
            //                ValidAudience = Configuration["TokenProviderOptions:ClientId"],
            //                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["TokenProviderOptions:ClientSecret"])),
            //            };
            //});
            services.AddMvc().AddMvcOptions(options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(
                    new HyphenControllerTransformer()));
            });
            //services.AddRazorPages()
            //    .AddViewOptions(options =>
            //    {
            //        options.HtmlHelperOptions.ClientValidationEnabled = false;
            //    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var appPath = Configuration.GetSection("CompositeSettings:Path").Value;
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseExceptionHandler(errorApp =>
                errorApp.Run(async context =>
                {
                    await ErrorService.LogException(context, logger);
                    context.Response.Redirect(appPath + "/Error");
                }));



            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("yourDetails", appPath + "/your-details", new {controller = "yourDetails", action = "body"});
                
                endpoints.MapControllerRoute("closeAccount", appPath + "/close-your-account", new {controller = "closeYourAccount", action = "body"});
                endpoints.MapControllerRoute("closeAccountBody", "/body/close-your-account", new {controller = "closeYourAccount", action = "body"});
                
                
                endpoints.MapControllerRoute("editDetails", appPath + "/edit-your-details", new {controller = "editYourDetails", action = "body"});
                endpoints.MapControllerRoute("changePassword", appPath + "/change-password", new {controller = "changePassword", action = "body"});
                
                endpoints.MapControllerRoute("deleteAccount", appPath + "/delete-account", new {controller = "deleteAccount", action = "body"});
                endpoints.MapControllerRoute("deleteAccountBody",  "/body/delete-account", new {controller = "deleteAccount", action = "body"});

                endpoints.MapControllerRoute("confirmDelete", appPath + "/confirm-delete", new { controller = "confirmDelete", action = "body" });
                endpoints.MapControllerRoute("confirmDeleteBody", "/body/confirm-delete", new { controller = "confirmDelete", action = "body" });

                endpoints.MapControllerRoute("shcDeleted", appPath + "/shc-deleted", new { controller = "shcDeleted", action = "body" });
                endpoints.MapControllerRoute("shcDeletedBody", "/body/shc-deleted", new { controller = "shcDeleted", action = "body" });

                endpoints.MapControllers();
            });

        }
    }
}
