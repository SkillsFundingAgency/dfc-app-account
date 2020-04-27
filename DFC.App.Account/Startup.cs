using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using DFC.App.Account.Services.DSS.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DFC.App.Account.Helpers;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace DFC.App.Account
{
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
            services.Configure<DssSettings>(Configuration.GetSection(nameof(DssSettings)));

            services.AddMvc().AddMvcOptions(options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(
                    new HyphenControllerTransformer()));
            });
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
           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("yourDetails", appPath + "/your-details", new {controller = "yourDetails", action = "body"});
                endpoints.MapControllerRoute("closeAccount", appPath + "/close-your-account", new {controller = "closeAccount", action = "body"});
                endpoints.MapControllerRoute("editDetails", appPath + "/edit-your-details", new {controller = "editDetails", action = "body"});
                endpoints.MapControllerRoute("changePassword", appPath + "/change-password", new {controller = "changePassword", action = "body"});
                endpoints.MapControllers();
            });

        }
    }
}
