namespace uLearn.Web.Models.PeerAssasmentModels
{
    public class ReviewModel
    {
        public bool WasSubmit { get; set; }
        public bool IsNotAssign { get; set; }
        public string TextForReview { get; set; }
        public int ReviewCnt { get; set; }
        public string Text { get; set; }
        public MarkModel[] Marks { get; set; }
    }
}