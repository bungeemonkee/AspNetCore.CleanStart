using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.CleanStart
{
    /// <summary>
    ///     Wraps .Net Core MVC server creation and execution.
    /// </summary>
    /// <typeparam name="TStartup">The configuration type for the server instance.</typeparam>
    public class Server<TStartup>
        where TStartup: Startup
    {
        /// <summary>
        ///     The instance of the startup class.
        ///     If this is not given in the constructor it is null.
        /// </summary>
        public readonly TStartup Startup;

        /// <summary>
        ///     The URLs given to the server to listen on.
        /// </summary>
        public readonly string[] Urls;

        /// <summary>
        ///     Creates a server instance with URLs to listen on.
        /// </summary>
        /// <param name="urls">The URLs to listen on.</param>
        public Server(params string[] urls)
            : this(null, urls) { }

        /// <summary>
        ///     Create a <see cref="Server{TStartup}" /> with the given startup instance.
        /// </summary>
        /// <param name="startup">The startup class instance.</param>
        public Server(TStartup startup)
            : this(startup, null) { }

        /// <summary>
        ///     Create a <see cref="Server{TStartup}" /> with the given startup instance.
        /// </summary>
        /// <param name="startup">The startup class instance.</param>
        /// <param name="urls">The URLs to listen on.</param>
        public Server(TStartup startup, params string[] urls)
        {
            Startup = startup;
            Urls = urls;

            ContentRoot = Directory.GetCurrentDirectory();
            WebRoot = Path.Combine(ContentRoot, "wwwroot");
        }

        /// <summary>
        ///     The web (static file) root. By default the current directory plus "wwwroot";
        /// </summary>
        public virtual string WebRoot { get; protected set; }

        /// <summary>
        ///     The content root. By default the current directory;
        /// </summary>
        public virtual string ContentRoot { get; protected set; }

        /// <summary>
        ///     Run the server synchronously. Wait for a Ctrl-C to exit.
        /// </summary>
        public void Run()
        {
            RunAsync(CancellationToken.None)
                .Wait();
        }

        /// <summary>
        ///     Run the server synchronously. Wait for notification from the given token to exit.
        /// </summary>
        public void Run(CancellationToken token)
        {
            RunAsync(token)
                .Wait();
        }

        /// <summary>
        ///     Run the server as a task. Wait for a Ctrl-C to exit.
        /// </summary>
        public Task RunAsync()
        {
            return RunAsync(CancellationToken.None);
        }

        /// <summary>
        ///     Run the server as a task. Wait for notification from the given token to exit.
        /// </summary>
        public async Task RunAsync(CancellationToken token)
        {
            // Construct the web host with:
            // * Kestrel as the webserver
            // * Listen on the configured url
            // * Set the webserver root and content path
            // * Setup environment variables for IIS integration
            // * Set the startup class to TStartup
            var hostBuilder = new WebHostBuilder().UseKestrel(ConfigureKestrel)
                .UseWebRoot(WebRoot)
                .UseContentRoot(ContentRoot)
                .UseIISIntegration()
                .UseStartup<TStartup>();

            // Set the startup - either by class or instance
            if (Startup != null) hostBuilder.ConfigureServices(x => x.AddSingleton<IStartup>(Startup));
            else hostBuilder.UseStartup<TStartup>();

            // Set the urls to listen on
            if (Urls != null && Urls.Length > 0) hostBuilder.UseUrls(Urls);

            // Apply additional host configuration
            ConfigureHost(hostBuilder);

            // Create the actual host instance
            var host = hostBuilder.Build();

            // Begin web host execution
            if (token == CancellationToken.None)
            {
                // Run the server with no token - it will wait for a Ctrl-C from the console
                await host.RunAsync();
            }
            else
            {
                // Run the server with a token to tell it when to quit
                await host.RunAsync(token);
            }
        }

        /// <summary>
        ///     Apply additional configuration to the web host with the given <see cref="IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IWebHostBuilder" /> used to configure the web host.</param>
        protected virtual void ConfigureHost(IWebHostBuilder hostBuilder) { }

        /// <summary>
        ///     Apply additional configuration to the kestrel server with the given <see cref="KestrelServerOptions" />.
        /// </summary>
        /// <param name="kestrelServerOptions">The <see cref="KestrelServerOptions" /> to configure.</param>
        protected virtual void ConfigureKestrel(KestrelServerOptions kestrelServerOptions) { }
    }
}
