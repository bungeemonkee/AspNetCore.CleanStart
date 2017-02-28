using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace AspNetCore.CleanStart
{
    public class Startup : IStartup
    {
        protected IHostingEnvironment Environment { get; }

        protected IConfigurationRoot Configuration { get; }

        protected Startup(IHostingEnvironment environment)
        {
            Environment = environment;
            Environment.ApplicationName = GetType().GetTypeInfo().Assembly.FullName;

            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void Configure(IApplicationBuilder app)
        {
            ConfigureApplication(app);
        }

        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var mvc = services.AddMvc(ConfigureMvcOptions);
            ConfigureMvc(mvc);

            return services.BuildServiceProvider();
        }

        public virtual void ConfigureApplication(IApplicationBuilder application)
        {
            application.UseMvc(ConfigureRouting);
        }

        protected virtual void ConfigureMvc(IMvcBuilder mvcBuilder)
        {
        }

        protected virtual void ConfigureMvcOptions(MvcOptions mvcOptions)
        {
        }

        protected virtual void ConfigureRouting(IRouteBuilder routeBuilder)
        {
        }
    }
}
