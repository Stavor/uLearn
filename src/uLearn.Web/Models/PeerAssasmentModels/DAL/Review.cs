using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace uLearn.Web.Models.PeerAssasmentModels.DAL
{
    public class Review
    {
        public Review()
        {
            Marks = new HashSet<Mark>();
        }

        public int ReviewId { get; set; }

        public int PropositionForReviewId { get; set; }

        public string Text { get; set; }

        [ForeignKey("PropositionForReviewId")]
        public virtual Proposition PropositionForReview { get; set; }

        public virtual ICollection<Mark> Marks { get; set; }
    }
}