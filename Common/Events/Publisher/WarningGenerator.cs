
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Events.Publisher
{
    public class WarningGenerator
    {
        public delegate void WarningEventHandler(object sender, WarningArgs e);

        public event WarningEventHandler PowerFlatLineEvent;
        public event WarningEventHandler PowerSpikeEvent;
        public event WarningEventHandler VoltageImbalanceWarning;
        public event WarningEventHandler OverTempWarning;

        public void WarningProcess(WarningTypes types) {
            switch (types) {
                case WarningTypes.PowerFlatlineWarning:
                    PowerFlatLine();
                    break;
                case WarningTypes.PowerSpike:
                    PowerSpike();
                    break;
                case WarningTypes.VoltageImbalanceWarning:
                    VoltageImbalance();
                    break;
                case WarningTypes.OverTempWarning:
                    OverTemp();
                    break;
            }
        }
        private void PowerFlatLine() {
            if (PowerFlatLineEvent != null) {
                PowerFlatLineEvent(this, new WarningArgs("Power Flatline"));
            }
        }

        private void PowerSpike()
        {
            if (PowerSpikeEvent != null)
            {
                PowerSpikeEvent(this, new WarningArgs("Power Spike"));
            }
        }

        private void VoltageImbalance()
        {
            if (VoltageImbalanceWarning != null)
            {
                VoltageImbalanceWarning(this, new WarningArgs("Voltage Imbalance"));
            }
        }

        private void OverTemp()
        {
            if (OverTempWarning != null)
            {
                OverTempWarning(this, new WarningArgs("Over Temp"));
            }
        }


    }
}
