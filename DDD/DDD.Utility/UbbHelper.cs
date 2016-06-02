using System.Text.RegularExpressions;

namespace DDD.Utility
{
    public static class UbbHelper
    {

        #region 公共静态方法

        /// <summary>
        /// UBB代码处理函数
        /// </summary>
        /// <param name="sDetail">输入字符串</param>
        /// <returns>输出字符串</returns>
        public static string UBBToHTML(string sDetail)
        {
            if (string.IsNullOrEmpty(sDetail)) return sDetail;
            Regex r;
            Match m;
            #region 处理空格
            sDetail = sDetail.Replace(" ", "&nbsp;");
            #endregion
            #region 处理单引号
            sDetail = sDetail.Replace("'", "’");
            #endregion
            #region 处理双引号
            sDetail = sDetail.Replace("\"", "&quot;");
            #endregion
            #region html标记符
            sDetail = sDetail.Replace("<", "&lt;");
            sDetail = sDetail.Replace(">", "&gt;");

            #endregion
            #region 处理换行
            //处理换行，在每个新行的前面添加两个全角空格
            r = new Regex(@"(\r\n((&nbsp;)|　)+)(?<正文>\S+)", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<BR>　　" + m.Groups["正文"].ToString());
            }
            //处理换行，在每个新行的前面添加两个全角空格
            sDetail = sDetail.Replace("\r\n", "<BR>");
            #endregion
            #region 处[b][/b]标记
            r = new Regex(@"(\[b\])([ \S\t]*?)(\[\/b\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<B>" + m.Groups[2].ToString() + "</B>");
            }
            #endregion
            #region 处[i][/i]标记
            r = new Regex(@"(\[i\])([ \S\t]*?)(\[\/i\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<I>" + m.Groups[2].ToString() + "</I>");
            }
            #endregion
            #region 处[u][/u]标记
            r = new Regex(@"(\[U\])([ \S\t]*?)(\[\/U\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<U>" + m.Groups[2].ToString() + "</U>");
            }
            #endregion
            #region 处[p][/p]标记
            //处[p][/p]标记
            r = new Regex(@"((\r\n)*\[p\])(.*?)((\r\n)*\[\/p\])", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<P class=\"pstyle\">" + m.Groups[3].ToString() + "</P>");
            }
            #endregion
            #region 处[sup][/sup]标记
            //处[sup][/sup]标记
            r = new Regex(@"(\[sup\])([ \S\t]*?)(\[\/sup\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<SUP>" + m.Groups[2].ToString() + "</SUP>");
            }
            #endregion
            #region 处[sub][/sub]标记
            //处[sub][/sub]标记
            r = new Regex(@"(\[sub\])([ \S\t]*?)(\[\/sub\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<SUB>" + m.Groups[2].ToString() + "</SUB>");
            }
            #endregion
            #region 处标记
            //处标记
            r = new Regex(@"(\[url\])([ \S\t]*?)(\[\/url\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<A href=\"" + m.Groups[2].ToString() + "\" target=\"_blank\">" + m.Groups[2].ToString() + "</A>");
            }
            #endregion
            #region 处[url=xxx][/url]标记
            //处[url=xxx][/url]标记
            r = new Regex(@"(\[url=([ \S\t]+)\])([ \S\t]*?)(\[\/url\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<A href=\"" + m.Groups[2].ToString() + "\" target=\"_blank\">"
            + m.Groups[3].ToString() + "</A>");
            }
            #endregion
            #region 处[email][/email]标记
            //处[email][/email]标记
            r = new Regex(@"(\[email\])([ \S\t]*?)(\[\/email\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<A href=\"mailto:" + m.Groups[2].ToString() + "\" target=\"_blank\">" +
                 m.Groups[2].ToString() + "</A>");
            }
            #endregion
            #region 处[email=xxx][/email]标记
            //处[email=xxx][/email]标记
            r = new Regex(@"(\[email=([ \S\t]+)\])([ \S\t]*?)(\[\/email\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<A href=\"mailto:" + m.Groups[2].ToString() + "\" target=\"_blank\">" +
                 m.Groups[3].ToString() + "</A>");
            }
            #endregion
            #region 处[size=x][/size]标记
            //处[size=x][/size]标记
            r = new Regex(@"(\[size=([1-7])\])([ \S\t]*?)(\[\/size\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<FONT SIZE=" + m.Groups[2].ToString() + ">" +
                 m.Groups[3].ToString() + "</FONT>");
            }
            #endregion
            #region 处[color=x][/color]标记
            //处[color=x][/color]标记
            r = new Regex(@"(\[color=([\S]+)\])([ \S\t]*?)(\[\/color\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<FONT COLOR=" + m.Groups[2].ToString() + ">" +
                 m.Groups[3].ToString() + "</FONT>");
            }
            #endregion
            #region 处[font=x][/font]标记
            //处[font=x][/font]标记
            r = new Regex(@"(\[font=([\S]+)\])([ \S\t]*?)(\[\/font\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<FONT FACE=" + m.Groups[2].ToString() + ">" +
                 m.Groups[3].ToString() + "</FONT>");
            }
            #endregion
            #region 处理图片链接
            //处理图片链接
            //r = new Regex("\\[picture\\](\\d+?)\\[\\/picture\\]", RegexOptions.IgnoreCase);
            //for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            //{
            //    sDetail = sDetail.Replace(m.Groups[0].ToString(),
            //     "<A href=\"ShowImage.aspx?Type=ALL&Action=forumImage&ImageID=" + m.Groups[1].ToString() + "\" target=\"_blank\"><IMG border=0 Title=\"点击打开新窗口查看\" src=\"ShowImage.aspx?Action=forumImage&ImageID=" + m.Groups[1].ToString() + "\"></A>");
            //}
            #endregion
            #region 处理[align=x][/align]
            //处理[align=x][/align]
            r = new Regex(@"(\[align=([\S]+)\])([ \S\t]*?)(\[\/align\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<P align=" + m.Groups[2].ToString() + ">" +
                 m.Groups[3].ToString() + "</P>");
            }
            #endregion
            #region 处[H=x][/H]标记
            //处[H=x][/H]标记
            r = new Regex(@"(\[H=([1-6])\])([ \S\t]*?)(\[\/H\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<H" + m.Groups[2].ToString() + ">" +
                 m.Groups[3].ToString() + "</H" + m.Groups[2].ToString() + ">");
            }
            #endregion
            #region 处理[list=x][*][/list]
            //处理[list=x][*][/list]
            r = new Regex(@"(\[list(=(A|a|I|i| ))?\]([ \S\t]*)\r\n)((\[\*\]([ \S\t]*\r\n))*?)(\[\/list\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                string strLI = m.Groups[5].ToString();
                Regex rLI = new Regex(@"\[\*\]([ \S\t]*\r\n?)", RegexOptions.IgnoreCase);
                Match mLI;
                for (mLI = rLI.Match(strLI); mLI.Success; mLI = mLI.NextMatch())
                {
                    strLI = strLI.Replace(mLI.Groups[0].ToString(), "<LI>" + mLI.Groups[1]);
                }
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<UL TYPE=\"" + m.Groups[3].ToString() + "\"><B>" + m.Groups[4].ToString() + "</B>" +
                 strLI + "</UL>");
            }

            #endregion
            #region 处[SHADOW=x][/SHADOW]标记
            //处[SHADOW=x][/SHADOW]标记
            r = new Regex(@"(\[SHADOW=)(\d*?),(#*\w*?),(\d*?)\]([\S\t]*?)(\[\/SHADOW\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<TABLE WIDTH=" + m.Groups[2].ToString() + "STYLE=FILTER:SHADOW(COLOR=" + m.Groups[3].ToString() + ",STRENGTH=" + m.Groups[4].ToString() + ")>" + m.Groups[5].ToString() + "</TABLE>");
            }
            #endregion
            #region 处[glow=x][/glow]标记
            //处[glow=x][/glow]标记
            r = new Regex(@"(\[glow=)(\d*?),(#*\w*?),(\d*?)\]([\S\t]*?)(\[\/glow\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<TABLE WIDTH=" + m.Groups[2].ToString() + "  STYLE=FILTER:GLOW(COLOR=" + m.Groups[3].ToString() + ", STRENGTH=" + m.Groups[4].ToString() + ")>" +
                 m.Groups[5].ToString() + "</TABLE>");
            }
            #endregion
            #region 处[center][/center]标记
            r = new Regex(@"(\[center\])([ \S\t]*?)(\[\/center\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<CENTER>" + m.Groups[2].ToString() + "</CENTER>");
            }
            #endregion
            #region 处[ IMG][ /IMG]标记
            //r = new Regex(@"(\[IMG\])(http|https|ftp):\/\/([ \S\t]*?)(\[\/IMG\])", RegexOptions.IgnoreCase);
            //for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            //{
            //    sDetail = sDetail.Replace(m.Groups[0].ToString(), "<br><a onfocus=this.blur() href=" + m.Groups[2].ToString() + "://" + m.Groups[3].ToString() + " target=_blank><IMG SRC=" + m.Groups[2].ToString() + "://" + m.Groups[3].ToString() + " border=0 alt=按此在新窗口浏览图片 onload=javascript:if(screen.width-333<this.width)this.width=screen.width-333></a>");
            //}
            #endregion
            #region 处[em]标记
            r = new Regex(@"(\[em([\S\t]*?)\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<img src=pic/em" + m.Groups[2].ToString() + ".gif border=0 align=middle>");
            }
            #endregion
            #region 处[flash=x][/flash]标记
            //处[mp=x][/mp]标记
            r = new Regex(@"(\[flash=)(\d*?),(\d*?)\]([\S\t]*?)(\[\/flash\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<a href=" + m.Groups[4].ToString() + " TARGET=_blank><IMG SRC=pic/swf.gif border=0 alt=点击开新窗口欣赏该FLASH动画!> [全屏欣赏]</a><br><br><OBJECT codeBase=http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0 classid=clsid:D27CDB6E-AE6D-11cf-96B8-444553540000 width=" + m.Groups[2].ToString() + " height=" + m.Groups[3].ToString() + "><PARAM NAME=movie VALUE=" + m.Groups[4].ToString() + "><PARAM NAME=quality VALUE=high><param name=menu value=false><embed src=" + m.Groups[4].ToString() + " quality=high menu=false pluginspage=http://www.macromedia.com/go/getflashplayer type=application/x-shockwave-flash width=" + m.Groups[2].ToString() + " height=" + m.Groups[3].ToString() + ">" + m.Groups[4].ToString() + "</embed></OBJECT>");
            }
            #endregion
            #region 处[dir=x][/dir]标记
            //处[dir=x][/dir]标记
            r = new Regex(@"(\[dir=)(\d*?),(\d*?)\]([\S\t]*?)(\[\/dir\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<object classid=clsid:166B1BCA-3F9C-11CF-8075-444553540000 codebase=http://download.macromedia.com/pub/shockwave/cabs/director/sw.cab#version=7,0,2,0 width=" + m.Groups[2].ToString() + " height=" + m.Groups[3].ToString() + "><param name=src value=" + m.Groups[4].ToString() + "><embed src=" + m.Groups[4].ToString() + " pluginspage=http://www.macromedia.com/shockwave/download/ width=" + m.Groups[2].ToString() + " height=" + m.Groups[3].ToString() + "></embed></object>");
            }
            #endregion
            #region 处[rm=x][/rm]标记
            //处[rm=x][/rm]标记
            r = new Regex(@"(\[rm=)(\d*?),(\d*?)\]([\S\t]*?)(\[\/rm\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<OBJECT classid=clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA class=OBJECT id=RAOCX width=" + m.Groups[2].ToString() + " height=" + m.Groups[3].ToString() + "><PARAM NAME=SRC VALUE=" + m.Groups[4].ToString() + "><PARAM NAME=CONSOLE VALUE=Clip1><PARAM NAME=CONTROLS VALUE=imagewindow><PARAM NAME=AUTOSTART VALUE=true></OBJECT><br><OBJECT classid=CLSID:CFCDAA03-8BE4-11CF-B84B-0020AFBBCCFA height=32 id=video2 width=" + m.Groups[2].ToString() + "><PARAM NAME=SRC VALUE=" + m.Groups[4].ToString() + "><PARAM NAME=AUTOSTART VALUE=-1><PARAM NAME=CONTROLS VALUE=controlpanel><PARAM NAME=CONSOLE VALUE=Clip1></OBJECT>");
            }
            #endregion
            #region 处[mp=x][/mp]标记
            //处[mp=x][/mp]标记
            r = new Regex(@"(\[mp=)(\d*?),(\d*?)\]([\S\t]*?)(\[\/mp\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<object align=middle classid=CLSID:22d6f312-b0f6-11d0-94ab-0080c74c7e95 class=OBJECT id=MediaPlayer width=" + m.Groups[2].ToString() + " height=" + m.Groups[3].ToString() + " ><param name=ShowStatusBar value=-1><param name=Filename value=" + m.Groups[4].ToString() + "><embed type=application/x-oleobject codebase=http://activex.microsoft.com/activex/controls/mplayer/en/nsmp2inf.cab#Version=5,1,52,701 flename=mp src=" + m.Groups[4].ToString() + "  width=" + m.Groups[2].ToString() + " height=" + m.Groups[3].ToString() + "></embed></object>");
            }
            #endregion
            #region 处[qt=x][/qt]标记
            //处[qt=x][/qt]标记
            r = new Regex(@"(\[qt=)(\d*?),(\d*?)\]([\S\t]*?)(\[\/qt\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<embed src=" + m.Groups[4].ToString() + " width=" + m.Groups[2].ToString() + " height=" + m.Groups[3].ToString() + " autoplay=true loop=false controller=true playeveryframe=false cache=false scale=TOFIT bgcolor=#000000 kioskmode=false targetcache=false pluginspage=http://www.apple.com/quicktime/>");
            }
            #endregion
            #region 处[QUOTE][/QUOTE]标记
            r = new Regex(@"(\[QUOTE\])([ \S\t]*?)(\[\/QUOTE\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<div style='border:#CCCCCC 1px dashed; width:94%; color:#999999; padding:3px; background:#F8F8F8'>" + m.Groups[2].ToString() + "</div><br /> ");
            }
            #endregion
            #region 处[move][/move]标记
            r = new Regex(@"(\[move\])([ \S\t]*?)(\[\/move\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<MARQUEE scrollamount=3>" + m.Groups[2].ToString() + "</MARQUEE>");
            }
            #endregion
            #region 处[FLY][/FLY]标记
            r = new Regex(@"(\[FLY\])([ \S\t]*?)(\[\/FLY\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<MARQUEE width=80% behavior=alternate scrollamount=3>" + m.Groups[2].ToString() + "</MARQUEE>");
            }
            #endregion
            #region 处[image][/image]标记
            //处[image][/image]标记
            r = new Regex(@"(\[image\])([ \S\t]*?)(\[\/image\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<img src=\"" + m.Groups[2].ToString() + "\" border=0 align=middle><br>");
            }
            #endregion

            return sDetail;
        }


        /// <summary>
        /// 微信支持版 updateTime: 2014年1月27日 11:07:42
        /// </summary>
        /// <param name="sDetail"></param>
        /// <returns></returns>
        public static string UBBToWeiXinHTML(string sDetail)
        {
            if (string.IsNullOrEmpty(sDetail)) return sDetail;
            sDetail = sDetail.Replace("&nbsp;", " ");
            sDetail = sDetail.Replace("<br/> ", "\n");
            Regex r;
            Match m;
            #region 处[url=xxx][/url]标记
            //处[url=xxx][/url]标记
            r = new Regex(@"(\[url=([ \S\t]+)\])([ \S\t]*?)(\[\/url\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<a href=\"" + m.Groups[2].ToString() + "\" >"
            + m.Groups[3].ToString() + "</a>");
            }
            #endregion
            return sDetail;
        }


        /// <summary>
        /// 享叮当富文本帮助类
        /// </summary>
        /// <param name="sDetail"></param>
        /// <returns></returns>
        public static string UBBToXddHTML(string sDetail)
        {
            if (string.IsNullOrEmpty(sDetail)) return sDetail;

            Regex r;
            Match m;

            #region 处[url=xxx][/url]标记
            //处[url=xxx][/url]标记
            r = new Regex(@"(\[url=([ \S\t]+)\])([ \S\t]*?)(\[\/url\])", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(),
                 "<a href=\"" + m.Groups[2] + "\" >"
            + m.Groups[3] + "</a>");
            }
            #endregion

            return sDetail;
        }

        #endregion
    }
}
