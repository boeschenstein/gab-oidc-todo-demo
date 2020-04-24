using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TodoDemo
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
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist/tododemo";
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDos API", Version = "v1" });
            });

            services.AddDbContextPool<ToDoDbContext>(builder => {
                builder.UseSqlite("Data Source=todos.db");
            });
            
            // remove default OId->WS-Fed mappings since we don't need legacy claim IDs
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://login.microsoftonline.com/4925c6ef-421e-417d-a9ce-4b559466d6ff/v2.0";
                    options.TokenValidationParameters.ValidIssuer = "https://sts.windows.net/4925c6ef-421e-417d-a9ce-4b559466d6ff/";
                    options.Audience = "api://todo";
                    options.IncludeErrorDetails = true;
                    options.SaveToken = true;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ToDoManagement", policy => policy.RequireClaim("scp", "ToDo.ReadWrite"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseCsp(config => {
                config.DefaultSources(cfg => cfg.Self())
                .ScriptSources(cfg => cfg.Self().UnsafeEval())
                .StyleSources(cfg => cfg.Self().UnsafeInline())
                .FontSources(cfg => cfg.Self())
                .ImageSources(cfg => cfg.Self().CustomSources("data:"))
                .FrameSources(cfg => cfg.Self().CustomSources("https://bif4-web-identity.azurewebsites.net"))
                .MediaSources(cfg => cfg.None())
                .FrameAncestors(cfg => cfg.None());

                if (env.IsDevelopment()) {
                    // webpack needs websocket but ws:// urls aren't covered under "self" policy
                    config.ConnectSources(cfg => cfg.CustomSources("*"));
                } else {
                    config.ConnectSources(cfg => cfg.Self().CustomSources("https://bif4-web-identity.azurewebsites.net"));
                }
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
