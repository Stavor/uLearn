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
        public PeerAssasmentStep[] Steps;

        [XmlElement("marks")]
        public Marks Marks;
    }

    public class Marks
    {
        [XmlElement("mark")]
        public Mark[] Mark;
    }

    public class Mark
    {
        [XmlAttribute("criterion")]
        public string Criterion { get; set; }

        [XmlAttribute("minVal")]
        public int MinValue { get; set; }

        [XmlAttribute("maxVal")]
        public int MaxValue { get; set; }

        [XmlAttribute("weightInViewFreq")]
        public double WeightInViewFreq { get; set; }

        [XmlAttribute("weightInSummary")]
        public double WeightInSummary { get; set; }
    }

    public class PeerAssasmentStep
    {
        [XmlAttribute("type")]
        public PeerAssasmentStepType StepType { get; set; }

        [XmlElement("deadline")]
        public DateTime? Deadline { get; set; }
    }

    public enum PeerAssasmentStepType
    {
        Undefined,
        PropositionWriting,
        Review,
        Observe
    }
}