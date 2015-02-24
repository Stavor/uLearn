using System;
using System.Linq;
using uLearn.PeerAssasments;
using uLearn.Web.DataContexts.PeerAssasmentRepository.Exceptions;
using uLearn.Web.DataContexts.PeerAssasmentRepository.OperationResult;
using uLearn.Web.DataContexts.PeerAssasmentRepository.Storage;
using uLearn.Web.Models.PeerAssasmentModels;
using uLearn.Web.Models.PeerAssasmentModels.DAL;

namespace uLearn.Web.DataContexts.PeerAssasmentRepository
{
    public class PeerAsssasmentAnswerRepository : IPeerAsssasmentAnswerRepository
    {
        private readonly PeerAssasment peerAssasment;
        private readonly PeerAssasmentStepType step;
        private readonly IPeerAssasmentStorage storage;
        private readonly IPeerAssasmentAnswerModelBuilder modelBuilder;
        private readonly IPeerAssasmentAnswerUpdater answerUpdater;
        private readonly IPeerAssasmentReviewManager reviewManager;

        public PeerAsssasmentAnswerRepository(PeerAssasment peerAssasment, PeerAssasmentStepType step)
            : this(
                new PeerAssasmentStorage(),
                new PeerAssasmentAnswerModelBuilder(peerAssasment),
                new PeerAssasmentAnswerUpdater(),
                new PeerAssasmentReviewManager())
        {
            this.peerAssasment = peerAssasment;
            this.step = step;
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

        public void UpdateAnswerBy(AnswerId answerId, ReviewModel review, bool isSubmit = false)
        {
            if (review == null)
                throw new ArgumentNullException("review");

            var answer = InnerUpdate(answerId, ans => answerUpdater.Update(ans, review));
            if (isSubmit && step == PeerAssasmentStepType.Review && answer.Reviews.Count < peerAssasment.ReviewCnt)
                AssignReview(answer).SucceedsWith(x => x);
        }

        private Answer InnerUpdate(AnswerId answerId, Func<Answer, Answer> update)
        {
            return InnerRead(answerId)
                .SucceedsWith(r =>
                    r.Length == 0
                        ? default(Answer).MarkAsFail(new AnswerWithIdDoesntExistException(answerId))
                        : r.Last().MarkAsSuccess())
                .SucceedsWith(ans => 
                    SaftyExecutor.TryMake(ans, update, "Во премя обновления ответа произошла ошибка."))
                .SucceedsWith(ans => storage.TryUpdate(ans))
                .SucceedsWith(x => x);
        }

        public AnswerModel GetOrCreate(AnswerId answerId)
        {
            return InnerRead(answerId)
                .SucceedsWith(answers =>
                    answers.Length != 0
                        ? answers.Last().MarkAsSuccess()
                        : CreateNewAnswer(answerId))
                .SucceedsWith(answer => AssignReviewIfNeed(IsNeedFirstReview(answer), answer))
                .SucceedsWith(answer => GetObserveIfNeed(answer))
                .SucceedsWith(tuple => modelBuilder.Build(tuple.Item2, tuple.Item3, tuple.Item1));
        }

        private Result<Tuple<bool, Answer, Review[]>> GetObserveIfNeed(Tuple<bool, Answer> prevRes)
        {
            if (step != PeerAssasmentStepType.Observe)
                return new Tuple<bool, Answer, Review[]>(prevRes.Item1, prevRes.Item2, null).MarkAsSuccess();
            var res = storage.TryRead<Review>(x => x.PropositionForReviewId == prevRes.Item2.PropositionId);
            if (res.IsFail)
                new Tuple<bool, Answer, Review[]>(prevRes.Item1, prevRes.Item2, null).MarkAsFail(new Exception("Не получилось вычитать review!"));
            return new Tuple<bool, Answer, Review[]>(prevRes.Item1, prevRes.Item2, res.Value).MarkAsSuccess();
        }

        private bool IsNeedFirstReview(Answer answer)
        {
            return step == PeerAssasmentStepType.Review && (answer.Reviews == null || answer.Reviews.Count == 0);
        }


        private Result<Tuple<bool, Answer>> AssignReviewIfNeed(bool needNewReview, Answer answer)
        {
            return needNewReview 
                ? AssignReview(answer) 
                : new Tuple<bool, Answer>(true, answer).MarkAsSuccess();
        }

        private Result<Tuple<bool, Answer>> AssignReview(Answer answer)
        {
            return reviewManager.AssignReview(answer)
                .IfSuccess(tuple =>
                {
                    if (tuple.Item1)
                    {
                        var res = storage.TryUpdate(tuple.Item2);
                        if (res.IsFail)
                        {
                            return tuple.MarkAsFail(res.FailMessage);
                        }
                        answer = new PeerAssasmentStorage().TryRead<Answer>(x => x.CourseId == tuple.Item2.CourseId && x.SlideId == tuple.Item2.SlideId && x.UserId == tuple.Item2.UserId)
                            .Value.Last();
                    }
                    return (new Tuple<bool, Answer>(tuple.Item1, answer)).MarkAsSuccess();
                });
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