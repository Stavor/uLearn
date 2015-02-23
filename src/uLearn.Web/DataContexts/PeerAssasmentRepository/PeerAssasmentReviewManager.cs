using System;
using System.Collections.Generic;
using System.Linq;
using uLearn.Web.DataContexts.PeerAssasmentRepository.OperationResult;
using uLearn.Web.DataContexts.PeerAssasmentRepository.Storage;
using uLearn.Web.Models.PeerAssasmentModels.DAL;

namespace uLearn.Web.DataContexts.PeerAssasmentRepository
{
    public class PeerAssasmentReviewManager : IPeerAssasmentReviewManager
    {
        public PeerAssasmentReviewManager()
            : this(new PeerAssasmentStorage())
        {
        }

        private readonly IPeerAssasmentStorage storage;

        private PeerAssasmentReviewManager(IPeerAssasmentStorage storage)
        {
            this.storage = storage;
        }

        public Result<Tuple<bool, Answer>> AssignReview(Answer answer) //todo переписать
        {
            if (answer == null)
                throw new ArgumentNullException("answer");
            var assigned = false;

            var otherAnswers = storage
                .TryRead<Answer>(x => x.AnswerId != answer.AnswerId
                                      && x.UserId != answer.UserId
                                      && x.CourseId == answer.CourseId
                                      && x.SlideId == answer.SlideId)
                .SucceedsWith(x => x);
            otherAnswers = otherAnswers.Where(x => x.Proposition.Timestamp != null).ToArray();
            var answerForReview = otherAnswers.FirstOrDefault(
                x => !(answer.Reviews ?? new Review[0])
                    .Where(r => r != null)
                    .Select(r => r.PropositionForReviewId)
                    .Contains(x.PropositionId));
            if (answerForReview != null)
            {
                if (answer.Reviews == null)
                    answer.Reviews = new List<Review>();
                answer.Reviews.Add(new Review
                {
                    PropositionForReviewId = answerForReview.PropositionId,
                    Marks = new HashSet<Mark>()
                    {
                        new Mark{Criterion = "Важная оценка"}, 
                    }
                });
                assigned = true;
            }
            
            return (new Tuple<bool, Answer>(assigned, answer)).MarkAsSuccess();
        }
    }
}