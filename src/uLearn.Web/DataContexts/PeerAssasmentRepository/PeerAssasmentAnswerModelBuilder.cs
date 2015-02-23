using System.Collections.Generic;
using System.Linq;
using uLearn.Web.Models.PeerAssasmentModels;
using uLearn.Web.Models.PeerAssasmentModels.DAL;

namespace uLearn.Web.DataContexts.PeerAssasmentRepository
{
    public class PeerAssasmentAnswerModelBuilder : IPeerAssasmentAnswerModelBuilder
    {
        public AnswerModel Build(Answer answer, bool assignNotFail)
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
                Review = BuildReview(answer.Reviews, assignNotFail)
            };
        }

        private static ReviewModel BuildReview(IEnumerable<Review> reviews, bool assignNotFail)
        {
            if (reviews == null)
                return null;

            var reviewsArr = reviews.ToArray();

            if (reviewsArr.Length == 0)
                return null;

            var reviewData = reviewsArr.LastOrDefault() ?? new Review();
            return new ReviewModel
            {
                IsNotAssign = assignNotFail,
                TextForReview = (reviewData.PropositionForReview ?? new Proposition()).Text.RenderMd(), //todo странное условие
                Text = reviewData.Text,
                Marks = BuildMarks(reviewData.Marks ?? new Mark[0])
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
                    Criterion = m.Criterion,
                    MinMark = -5,
                    MaxMark = 5
                }).ToArray();
        }

        private static PropositionModel BuildPropositon(Proposition proposition)
        {
            return proposition == null
                ? null
                : new PropositionModel
                {
                    Text = proposition.Text,
                    RenderedText = proposition.Text.RenderMd()
                };
        }
    }
}