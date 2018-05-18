using System;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AspNetCore.CleanStart.Tests
{
    [TestClass]
    public class ServerTests
    {
        [TestMethod]
        public void Constructor_Saves_Urls()
        {
            const string url = "http://test.ing";

            var server = new Server<Startup>(url);

            var result = server.Urls;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(url, result[0]);
        }

        [TestMethod]
        public void Constructor_Saves_Startup()
        {
            const string url = "http://test.ing";

            var server = new Server<Startup>(url);

            var result = server.Urls;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(url, result[0]);
        }

        [TestMethod]
        public void Run_Calls_ConfigureHost()
        {
            const string url = "http://test.ing";

            var server = new ServerWrapper(new[]
            {
                url
            });

            var cancelSource = new CancellationTokenSource();

            // Create a new task to test the server in
            Exception exception = null;
            var thread = new Thread(() =>
            {
                try
                {
                    server.RunAsync(cancelSource.Token)
                        .Wait();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            });
            thread.Start();

            // Wait for a second for the server to really start
            Thread.Sleep(1000);

            // Trigger the token to exit the server
            cancelSource.Cancel();

            // Wait for the thread to end, max: 10 seconds
            thread.Join(10000);

            // Re-throw any exception that occurred
            if (exception != null)
            {
                throw new Exception(exception.Message, exception);
            }

            Assert.IsTrue(server.ConfigureHostCalled);
        }

        [TestMethod]
        public void RunAsync_Calls_ConfigureHost()
        {
            const string url = "http://test.ing";

            var server = new ServerWrapper(new[]
            {
                url
            });

            var cancelSource = new CancellationTokenSource();

            // Create a new task to test the server in
            Exception exception = null;
            var thread = new Thread(() =>
            {
                try
                {
                    server.RunAsync(cancelSource.Token)
                        .Wait();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            });
            thread.Start();

            // Wait for a second for the server to really start
            Thread.Sleep(1000);

            // Trigger the token to exit the server
            cancelSource.Cancel();

            // Wait for the thread to end, max: 10 seconds
            thread.Join(10000);

            // Re-throw any exception that occurred
            if (exception != null)
            {
                throw new Exception(exception.Message, exception);
            }

            Assert.IsTrue(server.ConfigureHostCalled);
        }

        [TestMethod]
        public void RunAsync_Calls_OnShutdown()
        {
            var environment = new Mock<IHostingEnvironment>();
            environment.SetupAllProperties();
            environment.Object.ContentRootPath = Directory.GetCurrentDirectory();

            var startup = new StartupWrapper(environment.Object);
            var server = new Server<StartupWrapper>(startup);

            var cancelSource = new CancellationTokenSource();

            // Create a new task to test the server in
            Exception exception = null;
            var thread = new Thread(() =>
            {
                try
                {
                    server.RunAsync(cancelSource.Token)
                        .Wait();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            });
            thread.Start();

            // Wait for a second for the server to really start
            Thread.Sleep(1000);

            // Trigger the token to exit the server
            cancelSource.Cancel();

            // Wait for the thread to end, max: 10 seconds
            thread.Join(10000);

            // Re-throw any exception that occurred
            if (exception != null)
            {
                throw new Exception(exception.Message, exception);
            }

            Assert.AreEqual(1, startup.OnShutdownCallCount);
        }
    }
}
