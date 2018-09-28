using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace AspNetCore.CleanStart.Tests
{
    [ExcludeFromCodeCoverage]
    public class StartupWrapper: Startup
    {
        public StartupWrapper(IHostingEnvironment environment)
            : base(environment) { }

        public IHostingEnvironment EnvironmentPublic => Environment;

        public IConfigurationRoot ConfigurationPublic => Configuration;

        public int ConfigureConfigurationCallCount { get; private set; }

        public int OnShutdownCallCount { get; private set; }

        protected override IConfigurationRoot ConfigureConfiguration(ConfigurationBuilder configurationBuilder)
        {
            ++ConfigureConfigurationCallCount;

            return base.ConfigureConfiguration(configurationBuilder);
        }

        public override void OnShutdown()
        {
            ++OnShutdownCallCount;

            base.OnShutdown();
        }
    }
}
