﻿using System.ComponentModel;
using System.IO;
using BookService.Models;
using BookService.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;

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
            services.AddDbContext<AppDBContext>(opts => opts.UseSqlServer(Configuration["AppSettings:DefaultConnection:ConnectionString"]));
            services.AddTransient(typeof(IBookRepository<Book, int>), typeof(BookRepository));
            services.AddMvc();

            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc(
                        "v1",
                        new Swashbuckle.AspNetCore.Swagger.Info
                        {
                            Title = "Book Service",
                            Version = "v1",
                            Description = "Basic RESTful book service."
                        });
                    var xmlDocumentationPath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "BookService.xml");
                    options.IncludeXmlComments(xmlDocumentationPath);
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1/swagger.json", "Book Service V1");
            });
        }
    }
}
