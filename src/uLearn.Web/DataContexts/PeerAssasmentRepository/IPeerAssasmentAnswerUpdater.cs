using uLearn.Web.Models.PeerAssasmentModels;
using uLearn.Web.Models.PeerAssasmentModels.DAL;

namespace uLearn.Web.DataContexts.PeerAssasmentRepository
{
    public interface IPeerAssasmentAnswerUpdater
    {
        Answer Update(Answer ans, PropositionModel proposition);
        Answer Update(Answer ans, ReviewModel review);
    }
}