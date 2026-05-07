using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    [ServiceContract]
    public interface ISessionService
    {
        [OperationContract]
        void StartSession(PvMeta meta);
        [OperationContract]
        void PushSample(PvSample sample);
        [OperationContract]
        void EndSession();
    }
}
