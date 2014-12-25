using System.Collections.Generic;
using System.Linq;
using uLearn.Web.Models.PeerAssasmentModels;
using uLearn.Web.Models.PeerAssasmentModels.DAL;

namespace uLearn.Web.DataContexts.PeerAssasmentRepository
{
    public class PeerAssasmentAnswerModelBuilder : IPeerAssasmentAnswerModelBuilder
    {
        public AnswerModel Build(Answer answer)
        {
            return new AnswerModel
            {
                AnswerId = new AnswerId
                {
                    UserId = answer.UserId,
                    CourseId = answer.CourseId,
                    SlideId = answer.SlideId,
                },
                Proposition = BuildPropositon(answer.Proposition),
                Review = BuildReview(answer.Reviews)
            };
        }

        private static ReviewModel BuildReview(IEnumerable<Review> reviews)
        {
            if (reviews == null)
                return null;

            var reviewsArr = reviews.ToArray();

            if (reviewsArr.Length == 0)
                return null;

            var reviewData = reviewsArr[reviewsArr.Length];
            return new ReviewModel
            {
                Text = reviewData.Text,
                Marks = BuildMarks(reviewData.Marks)
            };
        }

        private static MarkModel[] BuildMarks(IEnumerable<Mark> marks)
        {
            if (marks == null)
                return null;

            return marks
                .Where(m => m != null)
                .Select(m => new MarkModel
                {
                    Mark = m.Value,
                    Criterion = m.Criterion
                }).ToArray();
        }

        private static PropositionModel BuildPropositon(Proposition proposition)
        {
            return proposition == null
                ? null
                : new PropositionModel
                {
                    Text = proposition.Text
                };
        }
    }
}