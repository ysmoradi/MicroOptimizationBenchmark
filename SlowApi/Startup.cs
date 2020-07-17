using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SlowApi.Controllers;
using System;

namespace SlowApi
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
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Use(async (cntx, next) =>
            {
                try
                {
                    await next().ConfigureAwait(false);
                }
                catch (Exception exp)
                {
                    if (exp is IHttpStatusCodeAwareException httpStatusCodeAwareException)
                    {
                        cntx.Response.StatusCode = (int)httpStatusCodeAwareException.HttpStatusCode;
                    }
                }
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
