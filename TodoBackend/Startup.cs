using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TodoBackend.Models;
using TodoBackend.Services;

namespace TodoBackend
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
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });
            var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            services.AddDbContext<TodoContext>(opt =>
                opt.UseNpgsql(string.Format(@"host={0};database={1};user id={2};password={3}", dbHost, dbName, dbUser, dbPassword)));
            services.AddControllers();
            services.AddSingleton<TodoService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}