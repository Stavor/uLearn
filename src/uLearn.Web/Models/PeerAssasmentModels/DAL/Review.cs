using System.Collections.Generic;

namespace uLearn.Web.Models.PeerAssasmentModels.DAL
{
    public class Review
    {
        public int ReviewId { get; set; }

        public int PropositionForReviewId { get; set; }

        public string Text { get; set; }

        public virtual Proposition PropositionForReview { get; set; }
        public virtual ICollection<Mark> Marks { get; set; } 
    }
}