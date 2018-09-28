using Microsoft.AspNetCore.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace AspNetCore.CleanStart.Tests
{
    [ExcludeFromCodeCoverage]
    public class ServerWrapper: Server<Startup>
    {
        public ServerWrapper(string[] urls)
            : base(urls) { }

        public bool ConfigureHostCalled { get; private set; }

        protected override void ConfigureHost(IWebHostBuilder hostBuilder)
        {
            base.ConfigureHost(hostBuilder);

            ConfigureHostCalled = true;
        }
    }
}
