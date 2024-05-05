using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameClient.Utility.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using GameLibrary.Network;
using GameClient.Utility.Interface;

namespace GameClient.Utility.Network.Tests
{
    [TestClass()]
    public class MatchServiceTests
    {
        static IServiceProvider serviceProvider;

        static MatchServiceTests()
        {
            ServiceCollection services = new();
            services.AddTransient<IMatchHubClientManager, TestMatchHubClient>();
            serviceProvider = services.BuildServiceProvider();
        }

        [TestMethod()]
        [DataRow(4)]
        public void MatchServiceTest(int count)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            IMatchHubClientManager[] client = Enumerable.Repeat(0, count)
                .Select(_ => serviceProvider.GetRequiredService<IMatchHubClientManager>()).ToArray();
            if (client.Any(v => v is null))
                return;
            foreach(var v in client)
            {
                MatchService service = new(v);
                service.StartAsync().Wait();
            }
                    
            Task.Delay(10000).Wait();
            scope.Dispose();
        }

        public class TestMatchHubClient : IMatchHubClientManager
        {

            public TestMatchHubClient()
            {
                Log(playerId + " Created");
            }

            private readonly string playerId = Random.Shared.Next(1, 1000).ToString();

            string IMatchHubClientManager.PlayerId => playerId;

            IMatchHubService IMatchHubClientManager.MatchHubService { get; set; } = null!;

            void IMatchHubClient.MatchCanceled()
            {
                Log(playerId + " MatchCanceled");
            }

            void IMatchHubClientManager.MatchClosed(Exception exception)
            {
                Log(playerId + " MatchClosed " + exception.Message);
            }

            void IMatchHubClient.MatchConfirmed()
            {
                Log(playerId + " MatchConfirmed");
            }

            void IMatchHubClient.Matched()
            {
                Log(playerId + " Matched");
            }
        }

        static void Log(string message)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " " + message);
        }
    }
}