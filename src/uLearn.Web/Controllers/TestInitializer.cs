using uLearn.PeerAssasments;
using uLearn.Web.DataContexts;
using uLearn.Web.DataContexts.PeerAssasmentRepository;
using uLearn.Web.Models.PeerAssasmentModels;
using uLearn.Web.Models.PeerAssasmentModels.DAL;

namespace uLearn.Web.Controllers
{
    public class TestInitializer
    {
        private IPeerAsssasmentAnswerRepository repository;
        private readonly string courseId;
        private readonly string slideId;
        private readonly string userId;

        public TestInitializer(IPeerAsssasmentAnswerRepository repository, string courseId, string slideId, string userId)
        {
            this.repository = repository;
            this.courseId = courseId;
            this.slideId = slideId;
            this.userId = userId;
        }

        public void InitializeFor(PeerAssasmentStepType stepType)
        {
            CleanAllDatas();
            switch (stepType)
            {
                case PeerAssasmentStepType.PropositionWriting:
                    break;
                case PeerAssasmentStepType.Review:
                    InitializeForReview();
                    break;
                case PeerAssasmentStepType.Observe:
                    InitializeForObserve();
                    break;
            }
        }

        private void InitializeForObserve()
        {
            SavePropositions();

            SaveReview(userId, "Да все ебланы! А остальные гандоны!");
            SaveReview(userId, "Да все ебланы! А остальные гандоны!");

            SaveReview("Катериада", "А цитата то не точная! А за что?");
            SaveReview("Катериада", "А цитата то не точная! А за что?");

            SaveReview("Галатея", "Нытьё, нытьё, нытьё");
            SaveReview("Галатея", "Нытьё, нытьё, нытьё");
        }

        private void SaveReview(string id, string review)
        {
            var answerId = GetAnswerId(id);
            repository.GetOrCreate(answerId, true);
            repository.UpdateAnswerBy(answerId, new ReviewModel { Text = review, Marks = {Criterion = "Важная оценка"}});
        }

        private void InitializeForReview()
        {
            SavePropositions();
            repository.GetOrCreate(GetAnswerId(userId), true);
        }

        private void SavePropositions()
        {
            SaveProposition("Катериада", "Все просрали еще в 2007. А потом наступила безысходность. В стране пиздец.");
            SaveProposition("Галатея", "A может все потому, пиздец, что люди полные ебланы и гандоны.");
            SaveProposition(userId, "Дай мошный бит, под который я буду бить лучшего друга.(с)");
        }

        private void SaveProposition(string id, string proposition)
        {
            var answerId = GetAnswerId(id);

            var propositionModel = new PropositionModel
            {
                Text = proposition
            };

            repository.GetOrCreate(answerId);
            repository.UpdateAnswerBy(answerId, propositionModel);
        }

        private AnswerId GetAnswerId(string id)
        {
            return new AnswerId
            {
                UserId = id,
                CourseId = courseId,
                SlideId = slideId
            };
        }
        
        private static void CleanAllDatas()
        {
            CleanSet<Mark>();
            CleanSet<Review>();
            CleanSet<Answer>();
            CleanSet<Proposition>();
        }

        private static void CleanSet<T>()
            where T : class
        {
            var db = new ULearnDb();
            var set = db.Set<T>();
            set.RemoveRange(set);
            db.SaveChanges();
        }
    }
}