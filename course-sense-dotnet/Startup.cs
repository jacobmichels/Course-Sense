using course_sense_dotnet.Application.CapacityManager;
using course_sense_dotnet.Application.NotificationManager;
using course_sense_dotnet.Application.NotificationManager.EmailClient;
using course_sense_dotnet.Application.NotificationManager.SMSClient;
using course_sense_dotnet.Application.WebAdvisor.RequestHelper;
using course_sense_dotnet.Application.WebAdvisor.RequestManager;
using course_sense_dotnet.Models;
using course_sense_dotnet.Repository;
using course_sense_dotnet.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace course_sense_dotnet
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add the singleton services to the DI container.
            // Add the thread-safe collection of NotificationRequests to DI as a singleton class.
            services.AddSingleton<SynchronizedCollection<NotificationRequest>>();
            // The DBRepository needs to be a singleton because there can only be one instance of the LiteDatabase class it uses interally at a time.
            services.AddTransient<IDBRepository, DBRepository>();


            // Add scoped services to the DI container. A new instance of these will be created for each scope.
            // Example of a scope: a request
            services.AddScoped<IEmailClient, EmailClient>();
            services.AddScoped<INotificationManager, NotificationManager>();
            services.AddScoped<ISMSClient, TwilioSMSClient>();
            services.AddScoped<IContactValidator, ContactValidator>();
            services.AddScoped<IRequestsHelper, RequestsHelper>();
            services.AddScoped<IRequestManager, RequestManager>();
            services.AddScoped<ICapacityManager, CapacityManager>();

            // Add the PollingLoop class as a background service.
            // The ExecuteAsync method of this class will be called to run in the background of the application.
            services.AddHostedService<PollingLoop>();

            services.AddLogging();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "course_sense_dotnet", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "course_sense_dotnet v1"));
            }

            loggerFactory.AddFile(app.ApplicationServices.GetService<IConfiguration>()["Logging:FilePath"]);

            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
