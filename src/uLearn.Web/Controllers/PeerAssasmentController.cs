using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using uLearn.PeerAssasments;
using uLearn.Web.DataContexts;
using uLearn.Web.Models;
using uLearn.Web.Models.PeerAssasmentModels;

namespace uLearn.Web.Controllers
{
    public class PeerAssasmentController : Controller
    {
        [Authorize]
        public ActionResult Run(string courseId, PeerAssasment peerAssasment)
        {
            var step = GetCurrentStep(peerAssasment);
            var userId = User.Identity.GetUserId();
            if (!CheckAccessConditions(userId, step, peerAssasment))
                throw new Exception("Вернуть страничку \"Вы все ...\"");


            var proposition = peerAssasementRepo.GetUserProposition(courseId, peerAssasment.Id, User.Identity.GetUserId());
            var model = new PeerAssasmentModel
            {
                CourseId = courseId,
                PA = peerAssasment,
                StepType = step,
                UserProposition = proposition
            };
            return View("FirstPAView", model);
        }

        [HttpPost]
        [Authorize]
        public void SubmitProposition(string courseId, string peerAssasmentId)
        {
            var user = User.Identity;
            var proposition = GetProposition(Request.InputStream);
            peerAssasementRepo.AddUserProposition(courseId, peerAssasmentId, user.GetUserId(), proposition, DateTime.UtcNow);
        }

        private bool CheckAccessConditions(string userId, PeerAssasmentStepType stepType, PeerAssasment peerAssasment)
        {
            return true;
        }

        private PeerAssasmentStepType GetCurrentStep(PeerAssasment peerAssasment)
        {
            var now = DateTime.Now;
            var step = (peerAssasment.Steps ?? new PeerAssasmentStep[0])
                .FirstOrDefault(x => x.Deadline > now);
            return step == null ? PeerAssasmentStepType.Undefined : step.StepType;
        }

        private static string GetProposition(Stream inputStream)
        {
            var codeBytes = new MemoryStream();
            inputStream.CopyTo(codeBytes);
            return Encoding.UTF8.GetString(codeBytes.ToArray());
        }

        private readonly PeerAssasmentRepo peerAssasementRepo = new PeerAssasmentRepo();
    }
}