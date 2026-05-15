using Common.Events.Publisher;
using Common.Helpers;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Events.Subscriber
{
    public class Listner
    {
        private ILogger logger = new Logger();
        public void LogInfo(object sender, OnTransferArgs e) {
            logger.Info(e.Message);
        }
        public void LogWarning(object sender, WarningArgs e)
        {
            logger.Warning(e.Message);
        }
    }
}
