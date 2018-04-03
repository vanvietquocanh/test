using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace OfferTest
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
        
  
            services.AddAuthentication(options =>
            {

                options.DefaultScheme = "bearer"; // this is the default scheme to be used
                options.DefaultChallengeScheme = "bearer";
            })
            
           .AddJwtBearer("bearer", options =>
           {
               options.Authority = "https://localhost:5000"; // URL of Identity Server; use IConfiguration instead of hardcoding 
              
               
               options.RequireHttpsMetadata = true; // require HTTPS (may be disabled in development, but advice against it)
               options.SaveToken = false; // cache the token for faster authentication
               options.IncludeErrorDetails = true; // get more details on errors; may be disabled in production 
           });
            services.AddMvc();
            
        }

       
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseForwardedHeaders();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            if (env.IsProduction() || env.IsStaging())
            {
                throw new Exception("Not development.");
            }
            
            app.UseMvc();
        }
    }
}
