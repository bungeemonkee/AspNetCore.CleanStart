using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AspNetCore.CleanStart.Tests
{
    [TestClass]
    public class StartupTests
    {
        [TestMethod]
        public void Constructor_Saves_Environment()
        {
            var name = typeof(StartupWrapper).GetTypeInfo()
                .Assembly.GetName()
                .Name;

            var environment = new Mock<IHostingEnvironment>();
            environment.SetupAllProperties();

            var startup = new StartupWrapper(environment.Object);

            var result = startup.EnvironmentPublic;

            Assert.AreSame(environment.Object, result);
            Assert.AreEqual(name, result.ApplicationName);
        }

        [TestMethod]
        public void Configuration_Returns_Same_Instance_Twice()
        {
            var environment = new Mock<IHostingEnvironment>();
            environment.SetupAllProperties();
            environment.SetupGet(x => x.ContentRootPath)
                .Returns(Directory.GetCurrentDirectory);

            var startup = new StartupWrapper(environment.Object);

            var result1 = startup.ConfigurationPublic;
            var result2 = startup.ConfigurationPublic;

            Assert.AreSame(result1, result2);
            Assert.AreEqual(1, startup.ConfigureConfigurationCallCount);
        }
    }
}
