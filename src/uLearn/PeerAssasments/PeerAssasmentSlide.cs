using System.Collections.Generic;

namespace uLearn.PeerAssasments
{
    public class PeerAssasmentSlide : Slide
    {
        public PeerAssasmentSlide(IEnumerable<SlideBlock> blocks, SlideInfo info, PeerAssasment peerAssasment)
            : base(blocks, info, peerAssasment.Title, peerAssasment.Id)
        {
            PeerAssasment = peerAssasment;
        }

        public PeerAssasment PeerAssasment { get; private set; }
    }
}