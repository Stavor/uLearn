using System.IO;

namespace uLearn.PeerAssasments
{
    public class PeerAssasmentSlideLoader : ISlideLoader
    {
        public string Extension
        {
            get { return "pa"; }
        }

        public Slide Load(FileInfo file, string unitName, int slideIndex)
        {
            var peerAssasment = file.DeserializeXml<PeerAssasment>();
            var slideInfo = new SlideInfo(unitName, file, slideIndex);
            return new PeerAssasmentSlide(new SlideBlock[0], slideInfo, peerAssasment);
        }
    }
}