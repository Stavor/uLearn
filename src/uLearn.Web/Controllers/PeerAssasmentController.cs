using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using uLearn.PeerAssasments;
using uLearn.Web.DataContexts.PeerAssasmentRepository;
using uLearn.Web.Models.PeerAssasmentModels;

namespace uLearn.Web.Controllers
{
    public class PeerAssasmentController : Controller
    {
        [Authorize]
        public ActionResult Run(string courseId, PeerAssasment peerAssasment)
        {
            var userId = User.Identity.GetUserId();
            var slideId = peerAssasment.Id;

            var answer = answerRepository.GetOrCreate(new AnswerId
            {
                UserId = userId,
                CourseId = courseId,
                SlideId = slideId
            });

            return View(answer);
        }

        private readonly PeerAsssasmentAnswerRepository answerRepository = new PeerAsssasmentAnswerRepository();
    }
}