using Common.Contracts;
using Common.Events.Publisher;
using Common.Events.Subscriber;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Analitics
{
    public class Spike
    {
        private List<PvSample> samples;

        public Spike()
        {
            samples = new List<PvSample>();
        }

        private int spikeTreshold = int.Parse(ConfigurationManager.AppSettings["PowerSpikeTreshold"]);

        public void SpikeCheck(PvSample sample)
        {
            samples.Add(sample);
            if (samples.Count >= 2)
            {
                List<PvSample> last = samples.GetRange(samples.Count - 2, 2);
                double firstRead = last[0].AcPwrt;
                double lastRead = last[1].AcPwrt;
                if (Math.Abs(lastRead - firstRead) > spikeTreshold)
                {
                    Listner listner = new Listner();

                    WarningGenerator generator = new WarningGenerator();

                    generator.PowerSpikeEvent += listner.LogWarning;

                    generator.WarningProcess(Enums.WarningTypes.PowerSpike);
                    //Power spike treshold needs to be asked 
                }
            }
        }
    }
}
