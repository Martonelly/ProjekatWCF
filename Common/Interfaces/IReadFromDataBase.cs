using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IReadFromDataBase
    {
        List<PvSample> FReadFromDataBase(string path, int limitN);
    }
}
