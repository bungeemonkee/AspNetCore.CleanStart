using Microsoft.AspNetCore.Hosting;

namespace AspNetCore.CleanStart.Tests
{
    public class ServerWrapper : Server<Startup>
    {
        public bool ConfigureHostCalled { get; private set; }

        public ServerWrapper(string[] urls)
            : base(urls)
        {
        }

        protected override void ConfigureHost(IWebHostBuilder hostBuilder)
        {
            base.ConfigureHost(hostBuilder);

            ConfigureHostCalled = true;
        }
    }
}
