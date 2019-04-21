using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AvG_Abgabe_1___Webapp.Service;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using AvG_Abgabe_1___Webapp.Model;

namespace AvG_Abgabe_1___Webapp
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
            // neben AddTransient gibt es noch AddScoped and AddSingleton
            // Serviceklasse als Transient registrieren => pro Request eine neue Instanz der Services
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Für DB
            var connection = @"Server=(localdb)\mssqllocaldb;Database=EFGetStarted.AspNetCore.NewDb;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<SupplierContext>
                (options => options.UseSqlServer(connection));
            // SupplierContext requires
            // using AvG_Abgabe_1___Webapp.Model;
            // UseSqlServer requires
            // using Microsoft.EntityFrameworkCore;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            applicationLifetime.ApplicationStarted.Register(OnStart);
        }

        public void OnStart()
        {
           
        }
    }
}
