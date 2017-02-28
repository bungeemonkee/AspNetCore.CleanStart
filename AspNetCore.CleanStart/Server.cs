using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.CleanStart
{
    public class Server<TStartup> where TStartup : Startup
    {
        public readonly string[] Urls;

        public Server(params string[] urls)
        {
            Urls = urls;
        }

        public void Run()
        {
            RunInternal(CancellationToken.None);
        }

        public void Run(CancellationToken token)
        {
            RunInternal(token);
        }

        public Task RunAsync()
        {
            return Task.Run(() => RunInternal(CancellationToken.None));
        }

        public Task RunAsync(CancellationToken token)
        {
            return Task.Run(() => RunInternal(token));
        }

        private void RunInternal(CancellationToken token)
        {
            // Find the directory of the configuration assembly
            var path = typeof(TStartup)
                .GetTypeInfo()
                .Assembly
                .Location;
            path = Path.GetDirectoryName(path);

            // Construct the web host with:
            // * Kestrel as the webserver
            // * Listen on the configured url
            // * Set the webserver root and content path to the content folder
            // * Setup environment variables for IIS integration
            // * Set the startup class to Startup
            var hostBuilder = new WebHostBuilder()
                .UseKestrel()
                .UseUrls(Urls)
                .UseWebRoot(path)
                .UseContentRoot(path)
                .UseIISIntegration()
                .UseStartup<Startup>();

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

        protected virtual void ConfigureHost(IWebHostBuilder hostBuilder)
        {
        }
    }
}
