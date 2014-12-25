namespace uLearn.Web.Models.PeerAssasmentModels
{
    public class AnswerModel
    {
        public AnswerId AnswerId { get; set; }
        public PropositionModel Proposition { get; set; }
        public ReviewModel Review { get; set; }
    }
}