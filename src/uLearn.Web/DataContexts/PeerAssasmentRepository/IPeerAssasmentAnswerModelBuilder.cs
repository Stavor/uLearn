using uLearn.Web.Models.PeerAssasmentModels;
using uLearn.Web.Models.PeerAssasmentModels.DAL;

namespace uLearn.Web.DataContexts.PeerAssasmentRepository
{
    public interface IPeerAssasmentAnswerModelBuilder
    {
        AnswerModel Build(Answer answer);
    }
}