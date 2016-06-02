using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Utility
{
    public class LinkHelper
    {

        public static string FormatLinkPic4Html(string myPic, string myTitle, string myUrl)
        { 
            string re = "";
            if (!string.IsNullOrEmpty(myPic))
            {
                if (!string.IsNullOrEmpty(myUrl))
                    re = "<a target=_blank title=\"" + myTitle + "\" href=\"" + myUrl + "\"><img border=0 src=\"" + myPic + "\"></a>";
                else
                    re = "<img border=0 title=\"" + myTitle + "\" src=\"" + myPic + "\">";
            }
            return re;
        }

        public static string FormatLinkTitle4Html(string myTitle, string myUrl)
        {
            string re = myTitle;
            if (!string.IsNullOrEmpty(myUrl))
                re = "<a target=_blank title=\"" + myTitle + "\" href=\"" + myUrl + "\">" + myTitle + "</a>";
            return re;
        }
    }
}
