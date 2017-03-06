using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.CleanStart.Tests
{
    public class StartupWrapper : Startup
    {
        public IHostingEnvironment EnvironmentPublic => Environment;

        public IConfigurationRoot ConfigurationPublic => Configuration;

        public int ConfigureConfigurationCallCount { get; private set; }

        public StartupWrapper(IHostingEnvironment environment) : base(environment)
        {
        }

        protected override IConfigurationRoot ConfigureConfiguration(ConfigurationBuilder configurationBuilder)
        {
            ++ConfigureConfigurationCallCount;

            return base.ConfigureConfiguration(configurationBuilder);
        }
    }
}
