using AutoMapper;
using DFC.App.Account.Application.Common.Services;
using DFC.App.Account.HostedServices;
using DFC.App.Account.Models;
using DFC.App.Account.Models.AddressSearch;
using DFC.App.Account.Services;
using DFC.App.Account.Services.Auth;
using DFC.App.Account.Services.Auth.Interfaces;
using DFC.App.Account.Services.Auth.Models;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using DFC.App.Account.Services.DSS.Services;
using DFC.App.Account.Services.SHC.Interfaces;
using DFC.App.Account.Services.SHC.Models;
using DFC.App.Account.Services.SHC.Services;
using DFC.APP.Account.CacheContentService;
using DFC.APP.Account.Data.Contracts;
using DFC.APP.Account.Data.Models;
using DFC.Compui.Cosmos;
using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Subscriptions.Pkg.Netstandard.Extensions;
using DFC.Compui.Telemetry;
using DFC.Content.Pkg.Netcore.Data.Models.ClientOptions;
using DFC.Content.Pkg.Netcore.Data.Models.PollyOptions;
using DFC.Content.Pkg.Netcore.Extensions;
using DFC.Personalisation.Common.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

namespace DFC.App.Account
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private const string CosmosDbContentPagesConfigAppSettings = "CosmosDbConnections:Account";
        private readonly IWebHostEnvironment env;
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.Configuration = configuration;
            this.env = env;

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var cosmosDbConnectionContentPages = Configuration.GetSection(CosmosDbContentPagesConfigAppSettings).Get<CosmosDbConnection>();
            var cosmosRetryOptions = new RetryOptions { MaxRetryAttemptsOnThrottledRequests = 20, MaxRetryWaitTimeInSeconds = 60 };
            services.AddDocumentServices<CmsApiSharedContentModel>(cosmosDbConnectionContentPages, env.IsDevelopment());
            services.AddContentPageServices<ContentPageModel>(cosmosDbConnectionContentPages, env.IsDevelopment(), cosmosRetryOptions);

            services.AddTransient<IEventMessageService<CmsApiSharedContentModel>, EventMessageService<CmsApiSharedContentModel>>();
            services.AddTransient<ICacheReloadService, CacheReloadService>();

            services.AddApplicationInsightsTelemetry();

            services.AddControllersWithViews();
            services.AddScoped<IDssReader, DssService>();
            services.AddScoped<IDssWriter, DssService>();
            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddTransient<IWebhooksService, WebhooksService>();
            services.AddTransient<IWebhookContentProcessor, WebhookContentProcessor>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISkillsHealthCheckService, SkillsHealthCheckService>();
            services.AddScoped<IHttpWebRequestFactory, HttpWebRequestFactory>();
            services.Configure<DssSettings>(Configuration.GetSection(nameof(DssSettings)));
            services.Configure<CompositeSettings>(Configuration.GetSection(nameof(CompositeSettings)));
            services.Configure<ActionPlansSettings>(Configuration.GetSection(nameof(ActionPlansSettings)));
            services.Configure<AddressSearchServiceSettings>(
                Configuration.GetSection(nameof(AddressSearchServiceSettings)));
            services.Configure<ShcSettings>(Configuration.GetSection(nameof(ShcSettings)));
            services.Configure<AuthSettings>(Configuration.GetSection("AuthSettings"));
            services.AddScoped<IOpenIDConnectClient, AzureB2CAuthClient>();
            services.Configure<OpenIDConnectSettings>(Configuration.GetSection("OIDCSettings"));
            services.AddSingleton(Configuration.GetSection(nameof(CmsApiClientOptions)).Get<CmsApiClientOptions>() ?? new CmsApiClientOptions());
            services.AddHostedServiceTelemetryWrapper();
            services.AddSubscriptionBackgroundService(Configuration);
            services.AddHostedService<CacheReloadBackgroundService>();
            var authSettings = new AuthSettings();
            var appPath = Configuration.GetSection("CompositeSettings:Path").Value;

            const string AppSettingsPolicies = "Policies";
            var policyOptions = Configuration.GetSection(AppSettingsPolicies).Get<PolicyOptions>() ?? new PolicyOptions();
            var policyRegistry = services.AddPolicyRegistry();
            services.AddApiServices(Configuration, policyRegistry);

            Configuration.GetSection("AuthSettings").Bind(authSettings);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateIssuerSigningKey = true,
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero,
                            ValidIssuer = authSettings.Issuer,
                            ValidAudience = authSettings.ClientId,
                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.ASCII.GetBytes(authSettings.ClientSecret)),
                        };
                    cfg.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {

                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Redirect(appPath + "/session-timeout");
                            }
                            else
                            {
                                context.Response.Redirect(authSettings.SignInUrl);
                            }
                            return Task.CompletedTask;


                        },
                        OnChallenge = context =>
                         {
                             context.Response.Redirect(authSettings.SignInUrl);
                             context.HandleResponse();
                             return Task.CompletedTask;
                         }


                    };
                });

            services.AddSession();
            services.AddMvc().AddMvcOptions(options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(
                    new HyphenControllerTransformer()));
            }).AddViewOptions(options =>
                options.HtmlHelperOptions.ClientValidationEnabled = false);
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
            app.UseSession();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("yourDetails", appPath + "/your-details", new { controller = "yourDetails", action = "body" });

                endpoints.MapControllerRoute("closeAccount", appPath + "/close-your-account", new { controller = "closeYourAccount", action = "body" });
                endpoints.MapControllerRoute("closeAccountBody", "/body/close-your-account", new { controller = "closeYourAccount", action = "body" });


                endpoints.MapControllerRoute("editDetails", appPath + "/edit-your-details", new { controller = "editYourDetails", action = "body" });
                endpoints.MapControllerRoute("changePassword", appPath + "/change-password", new { controller = "changePassword", action = "body" });

                endpoints.MapControllerRoute("deleteAccount", appPath + "/delete-account", new { controller = "deleteAccount", action = "body" });

                endpoints.MapControllerRoute("confirmDelete", appPath + "/confirm-delete", new { controller = "confirmDelete", action = "body" });
                endpoints.MapControllerRoute("confirmDeleteBody", "/body/confirm-delete", new { controller = "confirmDelete", action = "body" });

                endpoints.MapControllerRoute("shcDeleted", appPath + "/shc-deleted", new { controller = "shcDeleted", action = "body" });
                endpoints.MapControllerRoute("shcDeletedBody", "/body/shc-deleted", new { controller = "shcDeleted", action = "body" });

                endpoints.MapControllerRoute("authSuccess", appPath + "/authSuccess", new { controller = "AuthSuccess", action = "body" });
                endpoints.MapControllerRoute("authSuccessBody", "/body/authSuccess", new { controller = "AuthSuccess", action = "body" });

                endpoints.MapControllers();
            });

        }
    }
}
