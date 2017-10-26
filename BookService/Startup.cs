using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Serilog;
using BookService.Diagnostics;
using BookService.Models;
using BookService.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace BookService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(opts => opts.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));
            services.AddTransient(typeof(IBookRepository<Book, int>), typeof(BookRepository));
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    //options.Authority = "Online JWT Builder";
                    //options.Audience = "api.example.com/bookservice";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("Auth:Key").Value)),
                        ValidAudience = Configuration.GetSection("Auth:Audience").Value,
                        ValidIssuer = Configuration.GetSection("Auth:Issuer").Value,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = true,
                        ValidateLifetime = true
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Log.Information("Authentication failure: ", context.Exception.Message);
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            //Log.Debug("Validated token: ", context.SecurityToken);
                            Log.Debug("Validated token: ", context);
                            return Task.CompletedTask;
                        }
                    };
                });

//          Instead of services.AddMvc(); we are explicit, leaving out Razor views =>
            services.AddMvcCore()
                    .AddApiExplorer()
                    .AddAuthorization()
                    .AddDataAnnotations()
                    .AddJsonFormatters()
                    .AddCors(option =>
                    {
                        option.AddPolicy("AllowAnyOrigin", policy =>
                        {
                            policy.WithOrigins("http://*")
                                  .AllowAnyMethod()
                                  .AllowCredentials();
                        });
                        option.AddPolicy("OnlyMonitoring", policy =>
                        {
                            policy.WithOrigins("http://*")
                                  .WithMethods("GET");
                        });
                    });

            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc(
                        "v1",
                        new Info
                        {
                            Title = "Book Service",
                            Version = "v1",
                            Description = "Basic RESTful book service."
                        });
                    var xmlDocumentationPath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "BookService.xml");
                    options.IncludeXmlComments(xmlDocumentationPath);                    
                    options.AddSecurityDefinition("Bearer", new ApiKeyScheme() { In = "header", Description = "Please insert JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Log.Logger.Debug($"Hosting environment: {env.EnvironmentName}");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (Configuration.GetValue("EFDesignTime","") == "true")
            {
                Log.Error("EF designing!");
            }

            app.UseAuthentication();
            app.UseMiddleware<RequestLogMiddleware>();
            app.UseCors("AllowAnyOrigin");

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1/swagger.json", "Book Service V1");
            });

            app.UseMvc();
        }
    }
}
