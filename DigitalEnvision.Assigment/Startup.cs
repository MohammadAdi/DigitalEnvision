using DigitalEnvision.Assigment.Helpers;
using DigitalEnvision.Assigment.Infrastructures;
using DigitalEnvision.Assigment.Jobs;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace DigitalEnvision.Assigment
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
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("ConStr")));
            // Hangfire
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("ConStrHf")));
            services.AddHangfireServer();
            services.AddControllers();
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddTransient<IAlertQueueJob, AlertQueueJob>();
            services.AddTransient<ISendAlert, SendAlert>();
            services.Configure<Hookbin>(Configuration.GetSection("Hookbin"));

            // Swagger Config
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Web API",
                    Description = "Digital Envision - BackEnd Assigment",
                    Contact = new OpenApiContact
                    {
                        Name = "Mohammad Adi Fadilah",
                        Email = "adhi.development@gmail.com",
                    }
                });
                c.CustomSchemaIds(x => x.FullName);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "BackEnd Assigment Test"));
            app.UseHangfireDashboard();

            RecurringJob.AddOrUpdate<IAlertQueueJob>("recurring-alert-queue", (x) => x.RunAlertQueJob(), "30 * * * *");
            RecurringJob.AddOrUpdate<ISendAlert>("recurring-send-alert", (x) => x.RunSendAlert(), "0 */1 * * *");
        }
    }
}
