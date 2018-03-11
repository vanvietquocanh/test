using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
                options.Audience = "client.mydomain.com"; // ID of the client application; either hardcoded or configureable via IConfiguration if needed 
                options.RequireHttpsMetadata = true; // require HTTPS (may be disabled in development, but advice against it)
                options.SaveToken = true; // cache the token for faster authentication
                options.IncludeErrorDetails = true; // get more details on errors; may be disabled in production 
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
