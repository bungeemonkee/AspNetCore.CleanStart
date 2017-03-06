using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace AspNetCore.CleanStart
{
    /// <summary>
    ///     Wraps .Net Core MVC server creation and execution.
    /// </summary>
    /// <typeparam name="TStartup">The configuration type for the server instance.</typeparam>
    public class Server<TStartup>
        where TStartup : Startup
    {
        /// <summary>
        ///     The URLs given to the server to listen on.
        /// </summary>
        public readonly string[] Urls;

        /// <summary>
        ///     Creates a server instance with URLs to listen on.
        /// </summary>
        /// <param name="urls">The URLs to listen on.</param>
        public Server(params string[] urls)
        {
            Urls = urls;
        }

        /// <summary>
        ///     Run the server synchronously. Wait for a Ctrl-C to exit.
        /// </summary>
        public void Run()
        {
            RunInternal(CancellationToken.None);
        }

        /// <summary>
        ///     Run the server synchronously. Wait for notification from the given token to exit.
        /// </summary>
        public void Run(CancellationToken token)
        {
            RunInternal(token);
        }

        /// <summary>
        ///     Run the server as a task. Wait for a Ctrl-C to exit.
        /// </summary>
        public Task RunAsync()
        {
            return Task.Run(() => RunInternal(CancellationToken.None));
        }

        /// <summary>
        ///     Run the server as a task. Wait for notification from the given token to exit.
        /// </summary>
        public Task RunAsync(CancellationToken token)
        {
            return Task.Run(() => RunInternal(token));
        }

        /// <summary>
        ///     Apply additional configuration to the web host with the given <see cref="IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IWebHostBuilder" /> used to configure the web host.</param>
        protected virtual void ConfigureHost(IWebHostBuilder hostBuilder) { }

        private void RunInternal(CancellationToken token)
        {
            // Find the directory of the configuration assembly
            var path = typeof(TStartup).GetTypeInfo()
                .Assembly.Location;
            path = Path.GetDirectoryName(path);

            // Construct the web host with:
            // * Kestrel as the webserver
            // * Listen on the configured url
            // * Set the webserver root and content path to the content folder
            // * Setup environment variables for IIS integration
            // * Set the startup class to Startup
            var hostBuilder = new WebHostBuilder().UseKestrel()
                .UseWebRoot(path)
                .UseContentRoot(path)
                .UseIISIntegration()
                .UseStartup<TStartup>();

            if (Urls != null && Urls.Length > 0)
            {
                hostBuilder.UseUrls(Urls);
            }

            // Apply additional host configuration
            ConfigureHost(hostBuilder);

            // Create the actual host instance
            var host = hostBuilder.Build();

            // Begin web host execution
            if (token == CancellationToken.None)
            {
                // Run the server with no token - it will wait for a Ctrl-C from the console
                host.Run();
            }
            else
            {
                // Run the server with a token to tell it when to quit
                host.Run(token);
            }
        }
    }
}
