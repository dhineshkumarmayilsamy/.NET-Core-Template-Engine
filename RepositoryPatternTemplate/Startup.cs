using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
#if EnableSwaggerSupport
using Microsoft.OpenApi.Models;
#endif
using Model.DomainModel;
using Repository;
using Repository.Interfaces;
using Service;
using Service.Extensions.DependencyInjection;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RepositoryPatternTemplate.Middleware;

namespace RepositoryPatternTemplate
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

            //CORS
            var origins = new List<string>();
            Configuration.GetSection("Cors:Domains").Bind(origins);

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.WithOrigins(origins.ToArray())
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            }));

            // Dependencies

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductService, ProductService>();


            //Automapper
            services.AddAutoMapper();

            // Db Context
            int major = Convert.ToInt32(Configuration.GetSection("MySqlVersion")["Major"]);
            int minor = Convert.ToInt32(Configuration.GetSection("MySqlVersion")["Minor"]);
            int build = Convert.ToInt32(Configuration.GetSection("MySqlVersion")["Build"]);
            Version v = new Version(major, minor, build);

            var serverVersion = new MySqlServerVersion(v);
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("DbConnection"), serverVersion);
            });

            services.AddControllers();

#if EnableSwaggerSupport
            //Swagger 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RepositoryPatternTemplate", Version = "v1" });
            });
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
#if EnableSwaggerSupport
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RepositoryPatternTemplate v1"));
#endif
            }
            else
            {
                // Production
                app.UseStatusCodePages(context =>
                {
                    if (context.HttpContext.Response.StatusCode == 404)
                    {
                        context.HttpContext.Response.Redirect("/");
                    }

                    return Task.CompletedTask;
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //Logger
            app.UseLoggerMiddleware();

            // this will serve wwwroot/index.html when path is '/'
            app.UseDefaultFiles();
            // this will serve js, css, images etc.
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
