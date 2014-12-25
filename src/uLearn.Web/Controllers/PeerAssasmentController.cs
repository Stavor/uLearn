﻿using System.IO;
using System.Text;
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
        
        [HttpPost]
        [Authorize]
        public JsonResult SaveProposition(string courseId, string peerAssasmentId)
        {
            var user = User.Identity;
            var proposition = GetFromStream<PropositionModel>(Request.InputStream);
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

            return Json(answer.Proposition ?? new PropositionModel());
        }

        private static T GetFromStream<T>(Stream inputStream)
        {
            var codeBytes = new MemoryStream();
            inputStream.CopyTo(codeBytes);
            var str = Encoding.UTF8.GetString(codeBytes.ToArray());
            return System.Web.Helpers.Json.Decode<T>(str);
        }
    }
}