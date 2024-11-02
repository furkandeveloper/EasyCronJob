using EasyCron.Sample.Jobs;
using EasyCronJob.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace EasyCron.Sample
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EasyCron.Sample", Version = "v1" });
            });

            services.ApplyResulation<ConsoleCronJob>(options =>
            {
                options.CronExpression = "*/10 * * * * *";
                options.TimeZoneInfo = TimeZoneInfo.Local;
                options.CronFormat = Cronos.CronFormat.IncludeSeconds;
            });
            services.ApplyResulation<MyJob>(options =>
            {
                options.CronExpression = "* * * * *";
                options.TimeZoneInfo = TimeZoneInfo.Local;
                options.CronFormat = Cronos.CronFormat.Standard;
            });


            //services.InitializeCronServices();
            //services.AutoConfigurer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EasyCron.Sample v1"));
            }

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
