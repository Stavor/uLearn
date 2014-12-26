using System;
using uLearn.Web.DataContexts.PeerAssasmentRepository.OperationResult;
using uLearn.Web.Models.PeerAssasmentModels.DAL;

namespace uLearn.Web.DataContexts.PeerAssasmentRepository
{
    public interface IPeerAssasmentReviewManager
    {
        Result<Tuple<bool, Answer>> AssignReview(Answer aswer);
    }
}