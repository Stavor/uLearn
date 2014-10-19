using System;
using System.Linq;
using System.Threading.Tasks;
using uLearn.Web.Models.PeerAssasmentModels;
using uLearn.Web.Models.PeerAssasmentModels.DLA;

namespace uLearn.Web.DataContexts
{
    public class PeerAssasmentRepo
    {
        private readonly ULearnDb db;

        public PeerAssasmentRepo()
            : this(new ULearnDb())
        {
        }

        private PeerAssasmentRepo(ULearnDb db)
        {
            this.db = db;
        }

        public async Task<PeerAssasmentAnswer> AddUserProposition(string courseId, string peerAssasemntId, string userId, string text, DateTime time)
        {
            var peerAssasmentAnswer = new PeerAssasmentAnswer
            {
                CourseId = courseId,
                Text = text,
                Timestamp = time,
                UserId = userId,
                SlideId = peerAssasemntId
            };
            db.PeerAssasmentAnswers.Add(peerAssasmentAnswer);
            await db.SaveChangesAsync();
            return peerAssasmentAnswer;
        }

        public async Task<PeerAssasmentReview> AddUserReview(string courseId, string peerAssasmentId, string userId, int answerId, string review, DateTime time)
        {
            var peerAssasmentReview = new PeerAssasmentReview
            {
                CourseId = courseId,
                Text = review,
                Timestamp = time,
                UserId = userId,
                SlideId = peerAssasmentId,
                PeerAssasmentAnswerId = answerId
            };
            db.PeerAssasmentReviews.Add(peerAssasmentReview);
            await db.SaveChangesAsync();
            return peerAssasmentReview;
        }

        public string GetUserProposition(string courseId, string peerAssasemntId, string userId)
        {
            var answer = db.PeerAssasmentAnswers
                .Where(x => x.CourseId == courseId && x.UserId == userId && x.SlideId == peerAssasemntId)
                .OrderByDescending(x => x.Timestamp)
                .FirstOrDefault();
            return answer == null ? "" : answer.Text;
        }

        public UserReview GetUserReview(string courseId, string peerAssasementId, string userId)
        {
            var review = db.PeerAssasmentReviews
                .Where(x => x.CourseId == courseId && x.UserId == userId && x.SlideId == peerAssasementId)
                .OrderByDescending(x => x.Timestamp)
                .FirstOrDefault();
            var otherUserProposition = review != null
                ? GetProposition(review.PeerAssasmentAnswerId)
                : GetRandomProposition(courseId, peerAssasementId, userId);
            return new UserReview
            {
                OtherUserProposition = otherUserProposition.Text,
                OtherUserPropositionId = otherUserProposition.Id,
                Review = review != null ? review.Text : ""
            };

        }

        private PeerAssasmentAnswer GetProposition(int peerAssasmentAnswerId)
        {
            var answers = db.PeerAssasmentAnswers
                .Where(x => x.Id == peerAssasmentAnswerId).ToArray();
            if (answers.Length != 1)
                throw new Exception("Не существует PeerAssasmentAnswer с таким Id");
            return answers[0];
        }

        private PeerAssasmentAnswer GetRandomProposition(string courseId, string peerAssasementId, string userId)
        {
            var answers = db.PeerAssasmentAnswers
                .Where(x => x.CourseId == courseId && x.SlideId == peerAssasementId).ToArray();
            if(answers.Length == 0)
                throw new Exception("Серьёзно?");
            var randIndex = new Random().Next(answers.Length - 1);
            return answers[randIndex];
        }
    }
}