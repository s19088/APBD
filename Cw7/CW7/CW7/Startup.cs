using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CW7.Middleware;
using CW7.Models;
using CW7.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CW7
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
            services.AddScoped<IStudentDbService, SqlServeStudentrDbService>();
            services.AddScoped<IOService, LogWriter>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IStudentDbService dbService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<LoggingMiddleware>();
            app.UseWhen(context => context.Request.Path.ToString().Contains("students"), app =>//api/students/eska
            {
                app.Use(async (context, next) =>
                {
                    if (!context.Request.Headers.ContainsKey("Index"))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("brak indeksu");
                        return; //short circut
                    }
                    Student student = dbService.GetStudent(context.Request.Headers["Index"].ToString());
                    if (student == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        await context.Response.WriteAsync("nie ma studenta otym indeksie");
                        return;
                    }
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(student));
                    await next();
                });
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}