using Common.Contracts;
using Common.Events.Publisher;
using Common.Events.Subscriber;
using Common.Helpers;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Analitics
{
    public class Flatline
    {
        private List<PvSample> samples;

        public Flatline() {
            samples = new List<PvSample>();
        }

        private int faltlineRange = int.Parse(ConfigurationManager.AppSettings["PowerFlatlineWindow"]);

        private int range = int.Parse(ConfigurationManager.AppSettings["PowerFlatLineEta"]);
        public void FlatlineCheck(PvSample sample) {
            samples.Add(sample);
            if (samples.Count >= faltlineRange) {
                List<PvSample> last = samples.GetRange(samples.Count - faltlineRange, faltlineRange);
                double firstRead = last[0].AcPwrt;
                double lastRead = last[last.Count-1].AcPwrt;
                if (Math.Abs(lastRead - firstRead) < range) {
                    Listner listner = new Listner();

                    WarningGenerator generator = new WarningGenerator();

                    generator.PowerFlatLineEvent += listner.LogWarning;

                    generator.WarningProcess(Enums.WarningTypes.PowerFlatlineWarning);
                    samples.Clear();
                }
            }
        }
    }
}
