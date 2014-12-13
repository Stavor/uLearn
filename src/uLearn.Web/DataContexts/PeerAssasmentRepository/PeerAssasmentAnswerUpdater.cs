using System;
using System.Linq;
using uLearn.Web.Models.PeerAssasmentModels;
using uLearn.Web.Models.PeerAssasmentModels.DAL;

namespace uLearn.Web.DataContexts.PeerAssasmentRepository
{
    public class PeerAssasmentAnswerUpdater : IPeerAssasmentAnswerUpdater
    {
        public Answer Update(Answer ans, PropositionModel proposition)
        {
            if (proposition == null)
                throw new ArgumentNullException("proposition");
            if (ans == null)
                throw new ArgumentNullException("ans");

            ans.Proposition = ans.Proposition ?? new Proposition();
            ans.Proposition.Text = proposition.Text;
            ans.Proposition.Timestamp = DateTime.UtcNow;

            return ans;
        }

        public Answer Update(Answer ans, ReviewModel review)
        {
            if (review == null)
                throw new ArgumentNullException("review");

            if (ans == null)
                throw new ArgumentNullException("ans");

            if (ans.Reviews == null || !ans.Reviews.Any())
                throw new Exception("������������ �� ��������� �������� �����.");

            ans.Reviews.Last().Text = review.Text;
            ans.Reviews.Last().Marks = (review.Marks ?? new MarkModel[0])
                .Where(x => x != null)
                .Select(x =>
                    new Mark
                    {
                        Value = x.Mark,
                        Criterion = x.Criterion,
                    }).ToArray();
            return ans;
        }
    }
}