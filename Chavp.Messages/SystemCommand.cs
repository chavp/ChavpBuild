using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chavp.Messages
{
    public class SystemCommand
    {
        public SystemCommand()
        {
            ServerUtcNow = DateTime.UtcNow;
        }

        public DateTime ServerUtcNow { get; set; }
        public string Message { get; set; }
    }
}
