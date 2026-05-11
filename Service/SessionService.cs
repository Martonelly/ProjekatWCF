using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Contracts;

namespace Service
{
    public class SessionService : ISessionService
    {
        public void EndSession()
        {
            throw new NotImplementedException();
        }

        public void PushSample(PvSample sample)
        {
            //Reciveing the samples --> proceed with the requirements
        }

        public void StartSession(PvMeta meta)
        {
           //Just a test to see if it works 
           Console.WriteLine($"Got the meta stuff!{meta.FileName}, {meta.TotalRows}, {meta.SchemaVersion}, {meta.RowLimitN}");
            //TODO Create a CSV file for the incoming Samples (Data/<PlantId>/<YYYY-MM-DD>/session.csv. )
        }
    }
}
