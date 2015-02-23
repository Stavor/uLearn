using System;
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

            return InternalRun(userId, courseId, slideId);
        }

        public ActionResult InternalRun(string userId, string courseId, string slideId)
        {
            var answerRepository = createAnswerRepository(courseId, slideId);
            var answer = answerRepository.GetOrCreate(new AnswerId
            {
                UserId = userId,
                CourseId = courseId,
                SlideId = slideId
            });

            return View("Run", answer); 
        }

        private readonly Func<string, string, PeerAsssasmentAnswerRepository> createAnswerRepository = 
            (courseId, slideId) => new PeerAsssasmentAnswerRepository(GetPeerAssasment(courseId, slideId));
        
        [HttpPost]
        [Authorize]
        public JsonResult SaveProposition(string courseId, string peerAssasmentId, PropositionModel proposition)
        {
            var answerRepository = createAnswerRepository(courseId, peerAssasmentId);
            var user = User.Identity;
            var answerId = new AnswerId
            {
                UserId = user.GetUserId(),
                CourseId = courseId,
                SlideId = peerAssasmentId
            };
            answerRepository.UpdateAnswerBy(answerId, proposition);

            var answer = answerRepository.GetOrCreate(answerId);
            ViewBag.CourseId = answerId.CourseId;
            ViewBag.PeerAssasmentId = answerId.SlideId;

            return Json(new OperationResult //todo запариться за это 
            {
                ClientActionName = "reloadProposition",
                ParametrDescription = answer.Proposition ?? new PropositionModel()
            });
        }

        [HttpPost]
        [Authorize]
        public ActionResult SubmitProposition(string courseId, string peerAssasmentId, PropositionModel proposition)
        {
            var answerRepository = createAnswerRepository(courseId, peerAssasmentId);
            var user = User.Identity;
            var answerId = new AnswerId
            {
                UserId = user.GetUserId(),
                CourseId = courseId,
                SlideId = peerAssasmentId
            };
            answerRepository.UpdateAnswerBy(answerId, proposition);
            answerRepository.GetOrCreate(answerId, true);
            return RedirectToAction("Slide", "Course", new { courseId, slideIndex = peerAssasmentId });
        }

        [HttpPost]
        [Authorize]
        public ActionResult SubmitReview(string courseId, string peerAssasmentId, ReviewModel review)
        {
            var answerRepository = createAnswerRepository(courseId, peerAssasmentId);
            var user = User.Identity;
            var answerId = new AnswerId
            {
                UserId = user.GetUserId(),
                CourseId = courseId,
                SlideId = peerAssasmentId
            };
            answerRepository.UpdateAnswerBy(answerId, review);

//            var answer = answerRepository.GetOrCreate(answerId);
            ViewBag.CourseId = answerId.CourseId;
            ViewBag.PeerAssasmentId = answerId.SlideId;

            return Json("Test!");
        }

        [HttpGet]
        [Authorize]
        public ActionResult SetState(PeerAssasmentStepType step, string courseId, string slideId)
        {
            var answerRepository = createAnswerRepository(courseId, slideId);
            var userId = User.Identity.GetUserId();
            var initializer = new TestInitializer(answerRepository, courseId, slideId, userId);
            initializer.InitializeFor(step);

            return RedirectToAction("Slide", "Course", new { courseId, slideIndex = slideId });
        }

        private static PeerAssasment GetPeerAssasment(string courseId, string slideId)
        {
            var courseManager = WebCourseManager.Instance;
            var slide = courseManager.GetCourse(courseId).GetSlideById(slideId);
            return (slide as PeerAssasmentSlide).PeerAssasment;
        }
    }
}