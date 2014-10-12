using uLearn.PeerAssasments;
using uLearn.Web.Controllers;

namespace uLearn.Web.Models
{
    public class PeerAssasmentModel
    {
        public string CourseId { get; set; }
        public PeerAssasment PA { get; set; }
        public PeerAssasmentStepType StepType { get; set; }
        public string UserProposition { get; set; }
    }
}