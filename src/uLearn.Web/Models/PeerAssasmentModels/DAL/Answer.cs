using System.Collections.Generic;

namespace uLearn.Web.Models.PeerAssasmentModels.DAL
{
    public class Answer
    {
        public int AnswerId { get; set; }

        public int PropositionId { get; set; }

        public string UserId { get; set; }
        public string CourseId { get; set; }
        public string SlideId { get; set; }

        public virtual Proposition Proposition { get; set; }
        public virtual ICollection<Review> Reviews { get; set; } 
    }
}