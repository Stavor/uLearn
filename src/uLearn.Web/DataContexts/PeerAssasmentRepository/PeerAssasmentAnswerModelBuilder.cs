using System;
using System.Collections.Generic;
using System.Linq;
using uLearn.PeerAssasments;
using uLearn.Web.Models.PeerAssasmentModels;
using uLearn.Web.Models.PeerAssasmentModels.DAL;
using Mark = uLearn.Web.Models.PeerAssasmentModels.DAL.Mark;

namespace uLearn.Web.DataContexts.PeerAssasmentRepository
{
    public class PeerAssasmentAnswerModelBuilder : IPeerAssasmentAnswerModelBuilder
    {
        private readonly PeerAssasment peerAssasment;

        public PeerAssasmentAnswerModelBuilder(PeerAssasment peerAssasment)
        {
            this.peerAssasment = peerAssasment;
        }

        public AnswerModel Build(Answer answer, Review[] ansverPropreviews, bool assignNotFail)
        {
            return new AnswerModel
            {
                AnswerId = new AnswerId
                {
                    UserId = answer.UserId,
                    CourseId = answer.CourseId,
                    SlideId = answer.SlideId,
                },
                Proposition = BuildPropositon(answer),
                Review = BuildReview(answer.Reviews, assignNotFail),
                Observe = BuildObserve(ansverPropreviews)
            };
        }

        private ObserveModel BuildObserve(Review[] ansverPropreviews)
        {
            if (ansverPropreviews == null)
                return null;
            return new ObserveModel
            {
                Reviews = ansverPropreviews.Select(x => new ReviewFoObserve
                {
                    RenderedText = x.Text.RenderMd(),
                    Marks = (x.Marks ?? new Mark[0]).Select(m => new MarkModel
                    {
                        Criterion = m.Criterion,
                        Mark = m.Value
                    }).ToArray()
                }).ToArray()
            };
        }

        private ReviewModel BuildReview(IEnumerable<Review> reviews, bool assignNotFail)
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
                ReviewCnt = peerAssasment.ReviewCnt - reviewsArr.Length + 1,
                TextForReview = (reviewData.PropositionForReview ?? new Proposition()).Text.RenderMd(), //todo странное условие
                Text = reviewData.Text,
                Marks = BuildMarks(reviewData.Marks ?? new Mark[0])
            };
        }

        private MarkModel[] BuildMarks(IEnumerable<Mark> marks)
        {
            if (marks == null)
                return null;

            Func<IEnumerable<Mark>, string, string> getMarkOrDefault = (m, crit) =>
                (m.FirstOrDefault(x => x.Criterion == crit) ?? new Mark()).Value;

            return (
                from markDef in (peerAssasment.Marks.Mark ?? new PeerAssasments.Mark[0])
                select new MarkModel
                {
                    Criterion = markDef.Criterion,
                    MaxMark = markDef.MaxValue,
                    MinMark = markDef.MinValue,
                    Mark = getMarkOrDefault(marks, markDef.Criterion)
                }).ToArray();
        }

        private static PropositionModel BuildPropositon(Answer answer)
        {
            return answer.Proposition == null
                ? null
                : new PropositionModel
                {
                    IsReadonly = answer.Reviews != null,
                    Text = answer.Proposition.Text,
                    RenderedText = answer.Proposition.Text.RenderMd()
                };
        }
    }
}