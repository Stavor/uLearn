using System;
using System.Linq;
using System.Threading.Tasks;
using uLearn.Web.Models;
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

        public string GetUserProposition(string courseId, string peerAssasemntId, string userId)
        {
            var answer = db.PeerAssasmentAnswers
                .Where(x => x.CourseId == courseId && x.UserId == userId && x.SlideId == peerAssasemntId)
                .OrderByDescending(x => x.Timestamp)
                .FirstOrDefault();
            return answer == null ? "" : answer.Text;
        }
    }
}