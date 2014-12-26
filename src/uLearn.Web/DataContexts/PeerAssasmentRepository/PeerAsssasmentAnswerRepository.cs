using System;
using System.Linq;
using uLearn.Web.DataContexts.PeerAssasmentRepository.Exceptions;
using uLearn.Web.DataContexts.PeerAssasmentRepository.OperationResult;
using uLearn.Web.DataContexts.PeerAssasmentRepository.Storage;
using uLearn.Web.Models.PeerAssasmentModels;
using uLearn.Web.Models.PeerAssasmentModels.DAL;

namespace uLearn.Web.DataContexts.PeerAssasmentRepository
{
    public class PeerAsssasmentAnswerRepository : IPeerAsssasmentAnswerRepository
    {
        private readonly IPeerAssasmentStorage storage;
        private readonly IPeerAssasmentAnswerModelBuilder modelBuilder;
        private readonly IPeerAssasmentAnswerUpdater answerUpdater;
        private readonly IPeerAssasmentReviewManager reviewManager;

        public PeerAsssasmentAnswerRepository()
            : this(
                new PeerAssasmentStorage(),
                new PeerAssasmentAnswerModelBuilder(),
                new PeerAssasmentAnswerUpdater(),
                new PeerAssasmentReviewManager())
        {
        }

        private PeerAsssasmentAnswerRepository(
            IPeerAssasmentStorage storage,
            IPeerAssasmentAnswerModelBuilder modelBuilder,
            IPeerAssasmentAnswerUpdater answerUpdater,
            IPeerAssasmentReviewManager reviewManager)
        {
            this.storage = storage;
            this.modelBuilder = modelBuilder;
            this.answerUpdater = answerUpdater;
            this.reviewManager = reviewManager;
        }

        public void UpdateAnswerBy(AnswerId answerId, PropositionModel proposition)
        {
            if (proposition == null)
                throw new ArgumentNullException("proposition");

            InnerUpdate(answerId, ans => answerUpdater.Update(ans, proposition));
        }

        public void UpdateAnswerBy(AnswerId answerId, ReviewModel review)
        {
            if (review == null)
                throw new ArgumentNullException("review");

            InnerUpdate(answerId, ans => answerUpdater.Update(ans, review));
        }

        private void InnerUpdate(AnswerId answerId, Func<Answer, Answer> update)
        {
            InnerRead(answerId)
                .SucceedsWith(r =>
                    r.Length == 0
                        ? default(Answer).MarkAsFail(new AnswerWithIdDoesntExistException(answerId))
                        : r.Last().MarkAsSuccess())
                .SucceedsWith(ans => SaftyExecutor.TryMake(ans, update, "Во премя обновления ответа произошла ошибка."))
                .SucceedsWith(ans => storage.TryUpdate(ans))
                .SucceedsWith(x => x);
        }

        public AnswerModel GetOrCreate(AnswerId answerId, bool needNewReview = false)
        {
            return InnerRead(answerId)
                .SucceedsWith(answers =>
                    answers.Length != 0
                        ? answers.Last().MarkAsSuccess()
                        : CreateNewAnswer(answerId))
                .SucceedsWith(answer => AssignReviewIfNeed(needNewReview, answer))
                .SucceedsWith(tuple => modelBuilder.Build(tuple.Item2, tuple.Item1));
        }

        private Result<Tuple<bool, Answer>> AssignReviewIfNeed(bool needNewReview, Answer answer)
        {
            if (needNewReview)
            {
                return reviewManager.AssignReview(answer)
                    .IfSuccess(tuple =>
                    {
                        if (tuple.Item1)
                        {
                            var res = storage.TryUpdate(tuple.Item2);
                            if (res != null && res.IsFail)
                                return tuple.MarkAsFail(res.FailMessage);
                        }
                        return tuple.MarkAsSuccess();
                    });
            }

            return new Tuple<bool, Answer>(true, answer).MarkAsSuccess();
        }

        private Result<Answer[]> InnerRead(AnswerId answerId)
        {
            return
                CheckCorrectAnswerId(answerId)
                    .IfSuccess(r =>
                        storage.TryRead<Answer>(ans => ans.CourseId == answerId.CourseId
                                                       && ans.SlideId == answerId.SlideId
                                                       && ans.UserId == answerId.UserId));
            //                    .IfSuccess(r =>
            //                        r.Length > 1
            //                            ? default(Answer[]).MarkAsFail(new InconsistentStateByAnswerIdException(answerId))
            //                            : r.MarkAsSuccess());
        }

        private static Result<AnswerId> CheckCorrectAnswerId(AnswerId answerId)
        {
            return (answerId == null
                    || string.IsNullOrEmpty(answerId.CourseId)
                    || string.IsNullOrEmpty(answerId.SlideId)
                    || string.IsNullOrEmpty(answerId.UserId))
                ? answerId.MarkAsFail(new IncorectAnswerIdException(answerId))
                : answerId.MarkAsSuccess();
        }

        private Result<Answer> CreateNewAnswer(AnswerId answerId)
        {
            return storage
                .TryInsert(new Proposition())
                .SucceedsWith(prop => storage.TryInsert(
                    new Answer
                    {
                        CourseId = answerId.CourseId,
                        SlideId = answerId.SlideId,
                        UserId = answerId.UserId,
                        PropositionId = prop.PropositionId
                    }));
        }
    }
}