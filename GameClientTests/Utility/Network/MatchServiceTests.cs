using Microsoft.Extensions.DependencyInjection;
using GameLibrary.Network;
using GameClient.Utility.Interface;
using GameClient.Utility.Network;

namespace GameClientTests.Utility.Network
{
    [TestClass()]
    public class MatchServiceTests
    {
        private static readonly IServiceProvider ServiceProvider;

        static MatchServiceTests()
        {
            ServiceCollection services = new();
            services.AddTransient<IMatchHubClientManager, TestMatchHubClient>();
            ServiceProvider = services.BuildServiceProvider();
        }

        [TestMethod()]
        [DataRow(4)]
        public async void MatchServiceTest(int count)
        {
            using IServiceScope scope = ServiceProvider.CreateScope();
            IMatchHubClientManager[] client = Enumerable.Repeat(0, count)
                .Select(_ => ServiceProvider.GetRequiredService<IMatchHubClientManager>()).ToArray();

            foreach(var v in client)
            {
                MatchService service = new(v);
                await service.StartAsync(default);
            }
                    
            Task.Delay(10000).Wait();
        }

        public class TestMatchHubClient : IMatchHubClientManager
        {

            public TestMatchHubClient()
            {
                Log(playerId + " Created");
            }

            private readonly long playerId = Random.Shared.Next(1, 1000);

            long IMatchHubClientManager.PlayerId => playerId;


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