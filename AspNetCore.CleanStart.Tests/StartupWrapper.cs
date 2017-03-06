using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AspNetCore.CleanStart.Tests
{
    public class StartupWrapper: Startup
    {
        public StartupWrapper(IHostingEnvironment environment)
            : base(environment) {}

        public IHostingEnvironment EnvironmentPublic => Environment;

        public IConfigurationRoot ConfigurationPublic => Configuration;

        public int ConfigureConfigurationCallCount { get; private set; }

        protected override IConfigurationRoot ConfigureConfiguration(ConfigurationBuilder configurationBuilder)
        {
            ++ConfigureConfigurationCallCount;

            return base.ConfigureConfiguration(configurationBuilder);
        }
    }
}
