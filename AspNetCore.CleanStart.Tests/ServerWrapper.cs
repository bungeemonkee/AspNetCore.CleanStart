using Microsoft.AspNetCore.Hosting;

namespace AspNetCore.CleanStart.Tests
{
    public class ServerWrapper: Server<Startup>
    {
        public ServerWrapper(string[] urls)
            : base(urls) {}

        public bool ConfigureHostCalled { get; private set; }

        protected override void ConfigureHost(IWebHostBuilder hostBuilder)
        {
            base.ConfigureHost(hostBuilder);

            ConfigureHostCalled = true;
        }
    }
}
