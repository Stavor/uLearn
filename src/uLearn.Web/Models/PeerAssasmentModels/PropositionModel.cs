namespace uLearn.Web.Models.PeerAssasmentModels
{
    public class PropositionModel
    {
        public bool IsReadonly { get { return true; } }
        public string Text { get; set; }
        public string RenderedText { get; set; }
    }
}