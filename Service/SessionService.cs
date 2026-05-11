using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Contracts;

namespace Service
{
    internal class SessionService : ISessionService
    {
        public void EndSession()
        {
            throw new NotImplementedException();
        }

        public void PushSample(PvSample sample)
        {
            throw new NotImplementedException();
        }

        public void StartSession(PvMeta meta)
        {
           Console.WriteLine($"Got the meta stuff!{meta.FileName}, {meta.TotalRows}, {meta.SchemaVersion}, {meta.RowLimitN}");
        }
    }
}
