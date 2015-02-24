using uLearn.Web.Models.PeerAssasmentModels;

namespace uLearn.Web.DataContexts.PeerAssasmentRepository
{
    public interface IPeerAsssasmentAnswerRepository
    {
        void UpdateAnswerBy(AnswerId answerId, PropositionModel proposition);
        void UpdateAnswerBy(AnswerId answerId, ReviewModel review, bool isSubmit = false);
        AnswerModel GetOrCreate(AnswerId answerId);
    }
}