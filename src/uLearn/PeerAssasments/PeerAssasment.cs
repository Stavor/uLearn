using System;
using System.Xml.Serialization;

namespace uLearn.PeerAssasments
{
    [XmlRoot("peerAssasment", IsNullable = false)]
    public class PeerAssasment
    {
        [XmlAttribute("id")]
        public string Id;

        [XmlElement("title")]
        public string Title;

        [XmlElement("proposition")]
        public string Proposition;

        [XmlElement("step")]
        public PeerAssasmentStep [] Steps;
    }

    public class PeerAssasmentStep
    {
        [XmlAttribute("type")]
        public PeerAssasmentStepType StepType { get; set; }
        
        [XmlElement("deadline")]
        public DateTime? Deadline { get; set; }
    }
}