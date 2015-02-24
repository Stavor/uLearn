using System;
using System.Linq;
using System.Web;
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
            var state = GetState(peerAssasment);
            var userId = User.Identity.GetUserId();
            var slideId = peerAssasment.Id;

            return InternalRun(userId, courseId, slideId, state);
        }

        public ActionResult InternalRun(string userId, string courseId, string slideId, PeerAssasmentStepType step)
        {
            var answerRepository = createAnswerRepository(courseId, slideId, step);
            var answerId = new AnswerId
            {
                UserId = userId,
                CourseId = courseId,
                SlideId = slideId
            };
            var answer = answerRepository.GetOrCreate(answerId);
            return View("Run", answer);
        }

        private readonly Func<string, string, PeerAssasmentStepType, PeerAsssasmentAnswerRepository> createAnswerRepository =
            (courseId, slideId, step) => new PeerAsssasmentAnswerRepository(GetPeerAssasment(courseId, slideId), step);

        private readonly Func<PeerAssasment, PeerAssasmentStepType, PeerAsssasmentAnswerRepository> createAnswerRepository2 =
            (peerAssasment, step) => new PeerAsssasmentAnswerRepository(peerAssasment, step);

        [HttpPost]
        [Authorize]
        public JsonResult SaveProposition(string courseId, string peerAssasmentId, PropositionModel proposition)
        {
            var peerAssasment = GetPeerAssasment(courseId, peerAssasmentId); //todo нафигачить Init()
            var state = GetState(peerAssasment);
            var answerRepository = createAnswerRepository2(peerAssasment, state);

            var user = User.Identity;
            var answerId = new AnswerId
            {
                UserId = user.GetUserId(),
                CourseId = courseId,
                SlideId = peerAssasmentId
            };
            answerRepository.UpdateAnswerBy(answerId, proposition);

            var answer = answerRepository.GetOrCreate(answerId);

            return Json(answer.Proposition);
        }

        [HttpPost]
        [Authorize]
        public ActionResult SaveReview(string courseId, string peerAssasmentId, ReviewModel review)
        {
            var peerAssasment = GetPeerAssasment(courseId, peerAssasmentId); //todo нафигачить Init()
            var state = GetState(peerAssasment);
            var answerRepository = createAnswerRepository2(peerAssasment, state);

            var user = User.Identity;
            var answerId = new AnswerId
            {
                UserId = user.GetUserId(),
                CourseId = courseId,
                SlideId = peerAssasmentId
            };
            answerRepository.UpdateAnswerBy(answerId, review);
            var answer = createAnswerRepository2(peerAssasment, state).GetOrCreate(answerId);

            return Json(answer.Review);
        }

        [HttpPost]
        [Authorize]
        public ActionResult SubmitReview(string courseId, string peerAssasmentId, ReviewModel review)
        {
            var peerAssasment = GetPeerAssasment(courseId, peerAssasmentId); //todo нафигачить Init()
            var state = GetState(peerAssasment);
            var answerRepository = createAnswerRepository2(peerAssasment, state);

            var user = User.Identity;
            var answerId = new AnswerId
            {
                UserId = user.GetUserId(),
                CourseId = courseId,
                SlideId = peerAssasmentId
            };
            answerRepository.UpdateAnswerBy(answerId, review, true);
            var answer = createAnswerRepository2(peerAssasment, state).GetOrCreate(answerId);

            return Json(answer.Review);
        }

        [HttpGet]
        [Authorize]
        public ActionResult SetState(PeerAssasmentStepType step, string courseId, string slideId)
        {
            var peerAssasment = GetPeerAssasment(courseId, slideId);
            var userId = User.Identity.GetUserId();
            var initializer = new TestInitializer(userId, courseId);

            initializer.InitializeFor(step, peerAssasment);
            if (step != PeerAssasmentStepType.Undefined)
                Response.Cookies.Add(new HttpCookie("peerAssasmentState", step.ToString()));
            else
            {
                var coockie = new HttpCookie("peerAssasmentState", null)
                {
                    Expires = DateTime.UtcNow.AddDays(-1d)
                };
                Response.Cookies.Add(coockie);
            }

            return RedirectToAction("Slide", "Course", new { courseId, slideIndex = slideId });
        }

        private PeerAssasmentStepType GetState(PeerAssasment peerAssasment)
        {
            var paCoockie = Request.Cookies.Get("peerAssasmentState");
            PeerAssasmentStepType step;
            if (paCoockie != null && paCoockie.Value != null && Enum.TryParse(paCoockie.Value, out step))
                return step;
            var now = DateTime.Now;
            if (peerAssasment != null && peerAssasment.Steps != null)
                return (peerAssasment.Steps.FirstOrDefault(x => x.Deadline.HasValue && x.Deadline.Value > now) ?? new PeerAssasmentStep()).StepType;
            return PeerAssasmentStepType.Undefined;
        }

        private static PeerAssasment GetPeerAssasment(string courseId, string slideId)
        {
            var courseManager = WebCourseManager.Instance;
            var slide = courseManager.GetCourse(courseId).GetSlideById(slideId);
            return (slide as PeerAssasmentSlide).PeerAssasment;
        }
    }
}