using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Provrera...
namespace Common.Interfaces
{
    public interface ILogger
    {
        void Error(string message);
        void Info(string message);
        void Warning(string message);


    }
}
