using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Utility
{
    public class NewsHelper
    {
        public static string FormatTLink(string myID, string myLinkUrl, string myDLink)
        {
            if (string.IsNullOrEmpty(myDLink))
                return myLinkUrl + myID;
            else
                return myDLink;
        }
    }
}
