using Application;
using Application.Common.Interfaces;
using Domain.Common;
using Infrastructure;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc();
            //services.AddRazorPages().AddNewtonsoftJson();
            services.AddMvc(option => option.EnableEndpointRouting = false).AddNewtonsoftJson();

            services.AddAntiforgery(option => {
                option.HeaderName = "XSRF-TOKEN";
                option.SuppressXFrameOptionsHeader = false;
            });
            services.AddHttpContextAccessor();

            services.Configure<ApiCredentials>(options => Configuration.GetSection("ExternalAPICredentials").Bind(options));
            services.Configure<DbCredentials>(options => Configuration.GetSection("DbCredentials").Bind(options));

            services.AddInfrastructure();
            services.AddApplication();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UsePathBase(pathBase: "/movie");
            //app.UseRouting();
            app.UseStaticFiles(); // For the wwwroot folder
            app.UseHttpsRedirection();

            UserProvider.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            /*
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });*/
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

        }
    }
}
