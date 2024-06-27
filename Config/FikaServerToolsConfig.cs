using System.Runtime.Serialization;

namespace FikaServerTools.Config
{
    [DataContract]
    public class FikaServerToolsConfig
    {
        [DataMember(Name = "natPunchServer")]
        public FikaNatPunchServerConfig NatPunchServer { get; set; } = new FikaNatPunchServerConfig();
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
