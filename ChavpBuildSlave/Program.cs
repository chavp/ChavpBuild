using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChavpBuildSlave
{
    using Chavp.Messages;
    using ChavpBuildSlave.Properties;
    using EasyNetQ;
    using EasyNetQ.Topology;
    using System.Reflection;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            string rabbitMQBrokerHost = "localhost";
            string virtualHost = "build-pipeline";
            string username = Settings.Default.UserName;
            string password = Settings.Default.Password;

            string connectionString = string.Format(
                "host={0};virtualHost={1};username={2};password={3}",
                rabbitMQBrokerHost, virtualHost, username, password);

            Console.WriteLine("Connect: " + connectionString);
            using (var bus = RabbitHutch.CreateBus(connectionString))
            {
                Console.WriteLine("Start...");

                bus.Subscribe<BuildCommand>(username, command =>
                {
                    Console.WriteLine(command.RepositoryProviderName + ", " + command.RepositoryUri);
                },
                x => x.WithTopic(username));

                var subscriber = new AutoSubscriber(bus, username);

                subscriber.Subscribe(Assembly.GetExecutingAssembly());

                Console.WriteLine("Press any key to quit.");
                Console.ReadLine();
            }

        }
    }
}
