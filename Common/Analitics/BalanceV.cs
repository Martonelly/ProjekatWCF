using Common.Events.Publisher;
using Common.Events.Subscriber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Contracts;

namespace Common.Analitics
{
    public class BalanceV
    {
        private int imbalanceRange = int.Parse(System.Configuration.ConfigurationManager.AppSettings["VoltageImbalance"]);

        public void BalanceVCheck(PvSample sample)
        {
            double VL1TO2 = sample.Vl1to2;
            double VL2TO3 = sample.Vl2to3;
            double VL3TO1 = sample.Vl3to1;

            double max = Math.Max(VL1TO2, Math.Max(VL2TO3, VL3TO1));
            double min = Math.Min(VL1TO2, Math.Min(VL2TO3, VL3TO1));
            double imbalance = (max - min); // R in projct specifications

            double alowedRange = (VL1TO2 + VL2TO3 + VL3TO1) / 3 * imbalanceRange/100;

            if (imbalance > alowedRange)
            {
                Listner listner = new Listner();
                WarningGenerator generator = new WarningGenerator();
                generator.VoltageImbalanceWarning += listner.LogWarning;
                generator.WarningProcess(Enums.WarningTypes.VoltageImbalanceWarning);
            }

        }
    }
}
