using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void Run_Calls_ConfigureHost()
        {
            const string url = "http://test.ing";

            var server = new ServerWrapper(new[]
            {
                url
            });

            var cancelSource = new CancellationTokenSource();

            // Create a new task to test the server in
            var thread = new Thread(() => { server.Run(cancelSource.Token); });
            thread.Start();

            // Wait for a second for the server to really start
            Thread.Sleep(1000);

            // Trigger the token to exit the server
            cancelSource.Cancel();

            // Wait for the thread to end, max: 10 seconds
            thread.Join(10000);

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
            var thread = new Thread(() =>
            {
                server.RunAsync(cancelSource.Token)
                    .Wait();
            });
            thread.Start();

            // Wait for a second for the server to really start
            Thread.Sleep(1000);

            // Trigger the token to exit the server
            cancelSource.Cancel();

            // Wait for the thread to end, max: 10 seconds
            thread.Join(10000);

            Assert.IsTrue(server.ConfigureHostCalled);
        }
    }
}
