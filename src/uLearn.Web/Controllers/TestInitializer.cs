using uLearn.PeerAssasments;
using uLearn.Web.DataContexts;
using uLearn.Web.DataContexts.PeerAssasmentRepository;
using uLearn.Web.Models.PeerAssasmentModels;
using uLearn.Web.Models.PeerAssasmentModels.DAL;

namespace uLearn.Web.Controllers
{
    public class TestInitializer
    {
        private readonly string userId;
        private readonly string courseId;

        public TestInitializer(string userId, string courseId)
        {
            this.userId = userId;
            this.courseId = courseId;
        }

        public void InitializeFor(PeerAssasmentStepType stepType, PeerAssasment peerAssasment)
        {
            CleanAllDatas();
            switch (stepType)
            {
                case PeerAssasmentStepType.PropositionWriting:
                    break;
                case PeerAssasmentStepType.Review:
                    InitializeForReview(peerAssasment);
                    break;
                case PeerAssasmentStepType.Observe:
                    InitializeForObserve(peerAssasment);
                    break;
            }
        }

        private void InitializeForObserve(PeerAssasment peerAssasment)
        {
            var repo = new PeerAsssasmentAnswerRepository(peerAssasment, PeerAssasmentStepType.Review);
            SavePropositions(repo, peerAssasment);
            SaveReview(peerAssasment, repo, userId, "Да все ебланы! А остальные гандоны!");
            SaveReview(peerAssasment, repo, userId, "Да все ебланы! А остальные гандоны!");

            SaveReview(peerAssasment, repo, "Катериада", "А цитата то не точная! А за что?");
            SaveReview(peerAssasment, repo, "Катериада", "А цитата то не точная! А за что?");
 
            SaveReview(peerAssasment, repo, "Галатея", "Нытьё, нытьё, нытьё");
            SaveReview(peerAssasment, repo, "Галатея", "Нытьё, нытьё, нытьё");
        }

        private void SaveReview(PeerAssasment peerAssasment, PeerAsssasmentAnswerRepository repo, string id, string review)
        {
            var answerId = GetAnswerId(id, peerAssasment);
            repo.GetOrCreate(answerId);
            repo.UpdateAnswerBy(answerId, new ReviewModel
            {
                Text = review,
                WasSubmit = true,
                Marks = new[]
                {
                    new MarkModel { Criterion = peerAssasment.Marks.Mark[0].Criterion, Mark = "3" },
                    new MarkModel { Criterion = peerAssasment.Marks.Mark[1].Criterion, Mark = "2" },
                }
            }, true);
        }

        private void InitializeForReview(PeerAssasment peerAssasment)
        {
            var repo = new PeerAsssasmentAnswerRepository(peerAssasment, PeerAssasmentStepType.PropositionWriting);
            SavePropositions(repo, peerAssasment);
            repo.GetOrCreate(GetAnswerId(userId, peerAssasment));
        }

        private void SavePropositions(PeerAsssasmentAnswerRepository repo, PeerAssasment peerAssasment)
        {
            SaveProposition(peerAssasment, repo, "Катериада", "Все просрали еще в 2007. А потом наступила безысходность. В стране пиздец.");
            SaveProposition(peerAssasment, repo, "Галатея", "A может все потому, пиздец, что люди полные ебланы и гандоны.");
            SaveProposition(peerAssasment, repo, userId, "Дай мошный бит, под который я буду бить лучшего друга.(с)");
        }

        private void SaveProposition(PeerAssasment peerAssasment, PeerAsssasmentAnswerRepository repo, string id, string proposition)
        {
            var answerId = GetAnswerId(id, peerAssasment);

            var propositionModel = new PropositionModel
            {
                Text = proposition
            };

            repo.GetOrCreate(answerId);
            repo.UpdateAnswerBy(answerId, propositionModel);
        }

        private AnswerId GetAnswerId(string id, PeerAssasment peerAssasment)
        {
            return new AnswerId
            {
                UserId = id,
                CourseId = courseId,
                SlideId = peerAssasment.Id
            };
        }
        
        private static void CleanAllDatas()
        {
            CleanSet<Models.PeerAssasmentModels.DAL.Mark>();
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