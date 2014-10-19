using uLearn.PeerAssasments;

namespace uLearn.Web.Models.PeerAssasmentModels
{
    public class PeerAssasmentModel
    {
        public string CourseId { get; set; }
        public PeerAssasment PA { get; set; }
        public PeerAssasmentStepType StepType { get; set; }
        public string UserProposition { get; set; }
        public UserReview UserReview { get; set; }
    }

    public class UserReview
    {
        public string OtherUserProposition { get; set; }
        public int OtherUserPropositionId { get; set; }
        public string Review { get; set; }
    }
}