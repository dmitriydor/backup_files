using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Backup
{
    [DataContract]
    class Configuration
    {
        [DataMember]
        public List<string> SourceDir;
        [DataMember]
        public string TargetDir;
    }
}
