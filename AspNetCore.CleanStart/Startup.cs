using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCore.CleanStart
{
    /// <summary>
    ///     Represents the actions taken when the MVC application is initializing.
    /// </summary>
    public class Startup: IStartup
    {
        private IConfigurationRoot _configuration;

        /// <summary>
        ///     Create a new instance of <see cref="Startup" /> for the given environment.
        /// </summary>
        /// <param name="environment">The current environment.</param>
        public Startup(IHostingEnvironment environment)
        {
            Environment = environment;

            var type = GetType();
            if (type != typeof(Startup))
            {
                Environment.ApplicationName = type.GetTypeInfo()
                    .Assembly.GetName()
                    .Name;
            }
        }

        /// <summary>
        ///     The current <see cref="IHostingEnvironment" /> instance.
        /// </summary>
        protected IHostingEnvironment Environment { get; }

        /// <summary>
        ///     The current <see cref="IConfigurationRoot" /> instance.
        ///     This value is initialized the first time it is accessed by calling <see cref="ConfigureConfiguration" />.
        /// </summary>
        protected IConfigurationRoot Configuration
        {
            get
            {
                if (_configuration != null) return _configuration;

                _configuration = ConfigureConfiguration(new ConfigurationBuilder());
                return _configuration;
            }
        }

        /// <summary>
        ///     Application configuration entry point.
        /// </summary>
        /// <remarks>
        ///     Called by the runtime after <see cref="ConfigureServices" />.
        ///     Calls <see cref="ConfigureApplication(IApplicationBuilder)" />
        /// </remarks>
        /// <param name="app">The application to configure.</param>
        public void Configure(IApplicationBuilder app)
        {
            ConfigureApplication(app);
        }

        /// <summary>
        ///     Service configuration entry point.
        /// </summary>
        /// <remarks>
        ///     Called by the runtime before <see cref="Configure" />.
        ///     Calls <see cref="MvcServiceCollectionExtensions.AddMvc(IServiceCollection,Action{MvcOptions})" />.
        ///     Then <see cref="ConfigureMvc" />
        ///     Then <see cref="ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(IServiceCollection)" />
        /// </remarks>
        /// <param name="services">The <see cref="IServiceCollection" /> to which services will be added.</param>
        /// <returns>The <see cref="IServiceCollection" /> for the application.</returns>
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IStartup>(this);

            services.AddLogging(ConfigureLogging);

            var mvc = services.AddMvc(ConfigureMvcOptions);

            ConfigureMvc(mvc);

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Invoked after the web host has completed shutting down.
        /// </summary>
        public virtual void OnShutdown()
        {
        }

        /// <summary>
        ///     Create the <see cref="IConfigurationRoot" /> for <see cref="Configuration" />.
        /// </summary>
        /// <remarks>
        ///     Creates the default .NET Core configuration by adding a non-optional json file source for "appsettings.json",
        ///     an optional json file source for "appsettings.ENVIRONMENT.json", and adding environment variables"
        /// </remarks>
        /// <param name="configurationBuilder">A <see cref="ConfigurationBinder" /> for constructing the configuration.</param>
        /// <returns>The <see cref="IConfigurationRoot" /> instance.</returns>
        protected virtual IConfigurationRoot ConfigureConfiguration(ConfigurationBuilder configurationBuilder)
        {
            return configurationBuilder.SetBasePath(Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{Environment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        ///     Configure the application instance using an <see cref="IApplicationBuilder" />.
        /// </summary>
        /// <remarks>
        ///     Called by <see cref="Configure(IApplicationBuilder)" />.
        ///     Calls <see cref="MvcApplicationBuilderExtensions.UseMvc(IApplicationBuilder,Action{IRouteBuilder})" /> with
        ///     <see cref="ConfigureRouting" />.
        /// </remarks>
        /// <param name="applicationBuilder">The <see cref="IApplicationBuilder" /> used to configure the application.</param>
        protected virtual void ConfigureApplication(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMvc(ConfigureRouting);
        }

        /// <summary>
        ///     Configures services for the MVC application.
        /// </summary>
        /// <remarks>
        ///     Called by <see cref="ConfigureServices" />.
        /// </remarks>
        /// <param name="mvcBuilder">The <see cref="IMvcBuilder" /> used to configure MVC services.</param>
        protected virtual void ConfigureMvc(IMvcBuilder mvcBuilder) { }

        /// <summary>
        ///     Configures options for the MVC services.
        /// </summary>
        /// <remarks>
        ///     Called by <see cref="MvcServiceCollectionExtensions.AddMvc(IServiceCollection,Action{MvcOptions})" /> inside
        ///     <see cref="ConfigureServices" />.
        /// </remarks>
        /// <param name="mvcOptions">The <see cref="MvcOptions" /> instance to configure.</param>
        protected virtual void ConfigureMvcOptions(MvcOptions mvcOptions) { }

        /// <summary>
        ///     Configures options for MVC routing.
        /// </summary>
        /// <remarks>
        ///     Called by <see cref="MvcApplicationBuilderExtensions.UseMvc(IApplicationBuilder,Action{IRouteBuilder})" /> inside
        ///     <see cref="ConfigureApplication" />.
        ///     <para>
        ///         No routes are added as this is the default setup for .NET Core.
        ///         The intent is that attribute routing
        ///         (https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing#attribute-routing) will be used.
        ///     </para>
        /// </remarks>
        /// <param name="routeBuilder">The route builder to setup MVC routes.</param>
        protected virtual void ConfigureRouting(IRouteBuilder routeBuilder) { }

        /// <summary>
        ///     Configures logging.
        /// </summary>
        /// <remarks>
        ///     Called by <see cref="ConfigureServices" /> after building the <see cref="IServiceProvider" />.
        /// </remarks>
        /// <param name="loggingBuilder">The <see cref="ILoggingBuilder"/> to configure.</param>
        protected virtual void ConfigureLogging(ILoggingBuilder loggingBuilder)
        {
            var configuration = Configuration.GetSection("Logging");

            loggingBuilder.AddConfiguration(configuration);
            loggingBuilder.AddConsole();
            loggingBuilder.AddDebug();
        }
    }
}
