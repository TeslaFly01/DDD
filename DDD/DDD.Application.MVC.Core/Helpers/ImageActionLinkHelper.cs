using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace DDD.Application.MVC.Core.Helpers
{
    public static class ImageActionLinkHelper
    {
        public static string ImageActionLink(this AjaxHelper helper, string imageUrl, string altText, string titleText, string actionName, object routeValues, AjaxOptions ajaxOptions)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", imageUrl);
            builder.MergeAttribute("alt", altText);
            builder.MergeAttribute("title", titleText);
            var link = helper.ActionLink("[replaceme]", actionName, routeValues, ajaxOptions);
            return link.ToHtmlString().Replace("[replaceme]", builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}
