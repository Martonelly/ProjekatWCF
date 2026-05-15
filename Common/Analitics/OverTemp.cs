using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Contracts;
using Common.Events.Publisher;
using Common.Events.Subscriber;

namespace Common.Analitics
{
    public class OverTemp
    {
        private int overTempTreshold = int.Parse(System.Configuration.ConfigurationManager.AppSettings["OverTempTreshold"]);

        public void OverTempCheck(PvSample sample)
        {
            if (sample.Temper > overTempTreshold)
            {
                Listner listner = new Listner();
                WarningGenerator generator = new WarningGenerator();
                generator.OverTempWarning += listner.LogWarning;
                generator.WarningProcess(Enums.WarningTypes.OverTempWarning);
            }
        }
    }
}
