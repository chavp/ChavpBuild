using Chavp.Messages;
using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChavpBuildSlave
{
    public class SystemCommandConsumer : IConsume<SystemCommand>
    {
        public void Consume(SystemCommand message)
        {
            Console.WriteLine("Sever Time: " + message.ServerUtcNow);
            Console.WriteLine("Message: " + message.Message);
        }
    }
}
