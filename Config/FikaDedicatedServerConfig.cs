using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FikaDedicatedServer.Config
{
    [DataContract]
    public class FikaDedicatedServerConfig
    {
        [DataMember(Name = "natPunchServer")]
        public FikaNatPunchServerConfig FikaNatPunchServer { get; set; } = new FikaNatPunchServerConfig();
    }

    [DataContract]
    public class FikaNatPunchServerConfig
    {
        [DataMember(Name = "enable")]
        public bool Enable { get; set; } = false;

        [DataMember(Name = "ip")]
        public string IP { get; set; } = "0.0.0.0";

        [DataMember(Name = "port")]
        public int Port { get; set; } = 6970;

        [DataMember(Name = "natIntroduceAmount")]
        public int NatIntroduceAmount { get; set; } = 1;
    }
}
