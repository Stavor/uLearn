using System;

namespace uLearn.Web.Models.PeerAssasmentModels.DAL
{
    public class Proposition
    {
        public int PropositionId { get; set; }

        public string Text { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}