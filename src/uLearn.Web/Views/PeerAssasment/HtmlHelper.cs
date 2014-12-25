using System.Web.Mvc;
using System.Web.Mvc.Html;
using uLearn.Web.Models.PeerAssasmentModels;

namespace uLearn.Web.Views.PeerAssasment
{
    public static class HtmlHelper
    {
        public static MvcHtmlString PostActionButtonFor<T>(this HtmlHelper<T> html, PostActionButtonModel submitUrl)
        {
            return html.Partial("PostActionButtonView", submitUrl);
        }
    }
}