using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    [DataContract]
    public class PvMeta
    {
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public int TotalRows { get; set; }

        [DataMember]
        public string SchemaVersion { get; set; }

        [DataMember]
        public int RowLimitN { get; set; }

        public PvMeta(string fileName, int totalRows, string schemaVersion, int rowLimitN) {
            FileName = fileName;
            TotalRows = totalRows;
            SchemaVersion = schemaVersion;
            RowLimitN = rowLimitN;
        }
    }
}
