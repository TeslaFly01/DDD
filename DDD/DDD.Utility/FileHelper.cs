using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Net;

namespace DDD.Utility
{
    public class FileHelper
    {
        public FileHelper()
        {
        }
        public static long GetFileSize(string sPath)
        {
            long i = 0;
            if (File.Exists(sPath))
            {

                try
                {
                    FileInfo myf = new FileInfo(sPath);
                    i = myf.Length;
                }
                catch
                {
                    i = 0;
                }
            }
            return i;
        }

        public static bool DelFile(string f)
        {
            bool re = false;
            if (!string.IsNullOrEmpty(f))
            {
                string furl = HttpContext.Current.Server.MapPath(f);
                if (File.Exists(furl))
                {
                    try
                    {
                        File.Delete(furl);
                        re = true;
                    }
                    catch
                    {
                        re = false;
                    }
                }
            }
            return re;
        }

        /// <summary>
        /// 删除图片文件，同时删除缩微图
        /// </summary>
        /// <param name="f">相对路径文件名</param>
        /// <param name="tnPreName">缩微图文件名前缀</param>
        public static void DelPicFileAndTN(string f,string tnPreName)
        {
            if (!string.IsNullOrEmpty(f))
            {
                string furl = HttpContext.Current.Server.MapPath(f);
                string pictn = f.Insert(f.LastIndexOf("/") + 1, tnPreName);
                try
                {
                    File.Delete(furl);
                    File.Delete(HttpContext.Current.Server.MapPath(pictn));
                }
                catch
                {
                }
            } 
        }

        public static void CopyFiles(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);

            if (!Directory.Exists(varFromDirectory)) return;

            string[] directories = Directory.GetDirectories(varFromDirectory);

            if (directories.Length > 0)
            {
                foreach (string d in directories)
                {
                    CopyFiles(d, varToDirectory + d.Substring(d.LastIndexOf("\\")));
                }
            }


            string[] files = Directory.GetFiles(varFromDirectory);

            if (files.Length > 0)
            {
                foreach (string s in files)
                {
                    File.Copy(s, varToDirectory + s.Substring(s.LastIndexOf("\\")), true);
                }
            }
        }
        //删除指定文件夹对应其他文件夹里的文件
        public static void DeleteFiles(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);

            if (!Directory.Exists(varFromDirectory)) return;

            string[] directories = Directory.GetDirectories(varFromDirectory);

            if (directories.Length > 0)
            {
                foreach (string d in directories)
                {
                    DeleteFiles(d, varToDirectory + d.Substring(d.LastIndexOf("\\")));
                }
            }


            string[] files = Directory.GetFiles(varFromDirectory);

            if (files.Length > 0)
            {
                foreach (string s in files)
                {
                    File.Delete(varToDirectory + s.Substring(s.LastIndexOf("\\")));
                }
            }
        }

        /// <summary>
        /// 检测上传的文件是否有效： true－无效 ; false－有效
        /// </summary>
        /// <param name="FileUpload1"></param>
        /// <param name="mySize">单位：KB</param>
        /// <param name="myFileExten">允许的文件后缀集合:{ ".gif", ".png", ".jpg", ".jpeg", ".bmp" }</param>
        /// <param name="UpTitle">上传的文件类型：图片？附件？...</param>
        /// <returns></returns>
        public static bool ChkFileError(FileUpload FileUpload1, int mySize, string[] myFileExten, string UpTitle)
        {
            bool errorflag = false;

            if (FileUpload1.PostedFile == null)
            {
                HttpContext.Current.Response.Write("<script>alert(\"上传" + UpTitle + "失败，请先选择一个文件\");</script>");
                errorflag = true;
            }
            else
            {
                if (FileUpload1.PostedFile.FileName.Length < 3)
                {
                    HttpContext.Current.Response.Write("<script>alert(\"上传" + UpTitle + "失败，请先选择一个文件\");</script>");
                    errorflag = true;
                }
                else
                {
                    bool fileok = false;
                    string fileExtension = Path.GetExtension(FileUpload1.FileName).ToLower();
                    string[] allowedExtensions = myFileExten;
                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (fileExtension == allowedExtensions[i])
                        {
                            fileok = true;
                            break;
                        }

                    }
                    if (!fileok)
                    {
                        string j = "";
                        for (int i = 0; i < allowedExtensions.Length; i++)
                        {
                            j += allowedExtensions[i] + "/";
                        }
                        HttpContext.Current.Response.Write("<script>alert(\"上传" + UpTitle + "失败：只能是" + j + "的文件！\");</script>");
                        errorflag = true;
                    }
                    else
                    {
                        if (FileUpload1.PostedFile.ContentLength <= 0)
                        {
                            HttpContext.Current.Response.Write("<script>alert(\"上传" + UpTitle + "失败，请先选择一个文件\");</script>");
                            errorflag = true;
                        }
                        else
                        {
                            if (FileUpload1.PostedFile.ContentLength > mySize * 1024)
                            {
                                decimal mySizeMsg = (decimal)mySize / 1024;
                                HttpContext.Current.Response.Write("<script>alert(\"上传" + UpTitle + "失败，上传" + UpTitle + "大小限定在" + mySizeMsg.ToString("0.0") + "M以内\");</script>");
                                errorflag = true;
                            }

                        }
                    }
                }
            }
            return errorflag;
        }

        /// <summary>
        /// 检测上传的文件是否有效(JSReg-避免破坏css样式)： true－无效 ; false－有效
        /// </summary>
        /// <param name="js"></param>
        /// <param name="FileUpload1"></param>
        /// <param name="mySize">单位：KB</param>
        /// <param name="myFileExten">允许的文件后缀集合:{ ".gif", ".png", ".jpg", ".jpeg", ".bmp" }</param>
        /// <param name="UpTitle">上传的文件类型：图片？附件？...</param>
        /// <returns></returns>
        public static bool ChkFileError(JScriptReg js, FileUpload FileUpload1, int mySize, string[] myFileExten, string UpTitle)
        {
            bool errorflag = false;

            if (FileUpload1.PostedFile == null)
            {
                js.Alert("上传" + UpTitle + "失败，请先选择一个文件");
                errorflag = true;
            }
            else
            {
                if (FileUpload1.PostedFile.FileName.Length < 3)
                {
                    js.Alert("上传" + UpTitle + "失败，请先选择一个文件");
                    errorflag = true;
                }
                else
                {
                    bool fileok = false;
                    string fileExtension = Path.GetExtension(FileUpload1.FileName).ToLower();
                    string[] allowedExtensions = myFileExten;
                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (fileExtension == allowedExtensions[i])
                        {
                            fileok = true;
                            break;
                        }

                    }
                    if (!fileok)
                    {
                        string j = "";
                        for (int i = 0; i < allowedExtensions.Length; i++)
                        {
                            j += allowedExtensions[i] + "/";
                        }
                        js.Alert("上传" + UpTitle + "失败：只能是" + j + "的文件！");
                        errorflag = true;
                    }
                    else
                    {
                        if (FileUpload1.PostedFile.ContentLength <= 0)
                        {
                            js.Alert("上传" + UpTitle + "失败，请先选择一个文件");
                            errorflag = true;
                        }
                        else
                        {
                            if (FileUpload1.PostedFile.ContentLength > mySize * 1024)
                            {
                                decimal mySizeMsg = (decimal)mySize / 1024;
                                js.Alert("上传" + UpTitle + "失败，上传" + UpTitle + "大小限定在" + mySizeMsg.ToString("0.0") + "M以内");
                                errorflag = true;
                            }

                        }
                    }
                }
            }
            return errorflag;
        }

        /// <summary>
        /// 上传文件，选择生成高画质缩微图
        /// </summary>
        /// <param name="FileUpload1"></param>
        /// <param name="PicPath">输出文件保存在数据库的路径</param>
        /// <param name="dctpath">保存文件夹</param>
        /// <param name="CreatTNImg">是否生成"tn"缩微图</param>
        /// <param name="TNImg_W">缩微图宽</param>
        /// <param name="TNImg_H">缩微图高</param>
        /// <returns></returns>
        public static bool UploadFile(FileUpload FileUpload1, out string PicPath, string dctpath, bool CreatTNImg, int TNImg_W, int TNImg_H)
        {
            return UploadFile(FileUpload1, out PicPath, dctpath, CreatTNImg, TNImg_W, TNImg_H, "Cut", InterpolationMode.High, SmoothingMode.HighQuality);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="FileUpload1"></param>
        /// <param name="PicPath">输出文件保存在数据库的路径</param>
        /// <param name="dctpath">保存文件夹</param>
        /// <param name="CreatTNImg">是否生成"tn"缩微图</param>
        /// <param name="TNImg_W">缩微图宽</param>
        /// <param name="TNImg_H">缩微图高</param>
        /// <param name="CutMode">生成缩略图的方式:
        /// HW 指定高宽缩放（可能变形） 
        /// W 指定宽，高按比例
        /// H 指定高，宽按比例
        /// Cut 指定高宽裁减（不变形） </param>    
        /// <param name="interpolMode">插值算法</param>
        /// <param name="smoothingMode">平滑处理</param>
        /// <returns></returns>
        public static bool UploadFile(FileUpload FileUpload1, out string PicPath, string dctpath, bool CreatTNImg, int TNImg_W, int TNImg_H, string CutMode, InterpolationMode interpolMode, SmoothingMode smoothingMode)
        {
            string datefile = DateTime.Now.ToString("yyyy-MM");
            string path = HttpContext.Current.Server.MapPath(dctpath + datefile);
            //string path =dctpath+datefile;// zyl修改过
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = DateTime.Now.ToString("yyyyMMddHHmmss");
            string allfilename = "";
            string extfilename = "";
            Random r = new Random();
            filename = filename + r.Next(1000);
            extfilename = FileUpload1.PostedFile.FileName.Substring(FileUpload1.PostedFile.FileName.LastIndexOf(".") + 1);
            allfilename = filename + "." + extfilename;
            PicPath = dctpath + datefile + "/" + allfilename;//数据库保存路径
            string PicUrl = path + "/" + allfilename;//物理相对路径
            string tnPicUrl = path + "/" + "tn" + filename + "." + extfilename;

            bool addflag = true;
            try
            {
                FileUpload1.PostedFile.SaveAs(PicUrl);
                if (CreatTNImg)
                    ImageHelper.MakeThumbnail(PicUrl, tnPicUrl, TNImg_W, TNImg_H, CutMode, interpolMode, smoothingMode);
            }
            catch
            {
                addflag = false;
            }
            return addflag;
        }

        public static bool UrlFileExists(string URL,out string FileContentType,int myTimeOut)
        {
            HttpWebRequest req = null;
            HttpWebResponse res = null;
            FileContentType = string.Empty;
            try
            {
                req = (HttpWebRequest)WebRequest.Create(URL);
                req.Method = "HEAD";
                req.Timeout = myTimeOut;
                res = (HttpWebResponse)req.GetResponse();
                FileContentType = res.ContentType;
                return (res.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                return false;
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                    res = null;
                }
                if (req != null)
                {
                    req.Abort();
                    req = null;
                }
            }

        }

        /// <summary>
        /// //使用OutputStream.Write分块下载文件  
        /// </summary>
        /// <param name="myFilePath">文件相对地址</param>
        /// <param name="myNewFileName">文件重命名，空－默认名字</param>
        /// <param name="chunkSize">指定块大小，-1－102400（100k）</param>
        public static bool DownFile(string myFilePath,string myNewFileName,long chunkSize)
        {
            bool re = false;

            myFilePath = HttpContext.Current.Server.MapPath(myFilePath);
            if (File.Exists(myFilePath))
            {
                FileInfo fileInfo = new FileInfo(myFilePath);
                if (fileInfo.Length == 0) return re;
                else
                {
                    if (myNewFileName == "")
                        myNewFileName = //HttpUtility.UrlPathEncode(myFilePath.Substring(myFilePath.LastIndexOf("/") + 1));
                            HttpUtility.UrlPathEncode(fileInfo.Name);
                    else myNewFileName = HttpUtility.UrlPathEncode(myNewFileName) + fileInfo.Extension;
                }
            }
            else return re;

            //指定块大小   
            if (chunkSize == -1)
                chunkSize = 102400;
            //建立一个100K的缓冲区   
            byte[] buffer = new byte[chunkSize];
            //已读的字节数   
            long dataToRead = 0;
            FileStream stream = null;
            try
            {
                //打开文件   
                stream = new FileStream(myFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                dataToRead = stream.Length;

                //添加Http头   
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachement;filename=" + myNewFileName);
                HttpContext.Current.Response.AddHeader("Content-Length", dataToRead.ToString());

                while (dataToRead > 0)
                {
                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        int length = stream.Read(buffer, 0, Convert.ToInt32(chunkSize));
                        HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
                        HttpContext.Current.Response.Flush();
                        HttpContext.Current.Response.Clear();
                        dataToRead -= length;
                    }
                    else
                    {
                        //防止client失去连接
                        dataToRead = -1;
                    }
                }
                re = true;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write("Error:" + ex.Message);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
                HttpContext.Current.Response.Close();
            }
            return re;
        }


        #region 文件下载文件名乱码修复

        /// <summary>
        /// 为字符串中的非英文字符编码Encodes non-US-ASCII characters in a string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static string ToHexString(string s)
        {
            char[] chars = s.ToCharArray();
            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < chars.Length; index++)
            {
                bool needToEncode = NeedToEncode(chars[index]);
                if (needToEncode)
                {
                    string encodedString = ToHexString(chars[index]);
                    builder.Append(encodedString);
                }
                else
                {
                    builder.Append(chars[index]);
                }
            }
            return builder.ToString();
        }
        /// <summary>
        ///指定一个字符是否应该被编码 Determines if the character needs to be encoded.
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        private static bool NeedToEncode(char chr)
        {
            string reservedChars = "$-_.+!*'(),@=&";
            if (chr > 127)
                return true;
            if (char.IsLetterOrDigit(chr) || reservedChars.IndexOf(chr) >= 0)
                return false;
            return true;
        }
        /// <summary>
        /// 为非英文字符串编码Encodes a non-US-ASCII character.
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        private static string ToHexString(char chr)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] encodedBytes = utf8.GetBytes(chr.ToString());
            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < encodedBytes.Length; index++)
            {
                builder.AppendFormat("%{0}", Convert.ToString(encodedBytes[index], 16));
            }
            return builder.ToString();
        }


        /// <summary>
        /// 文件下载文件名乱码修复
        ///1.如果指定的文件名里包含了空格,FireFox就会截取空格前的部分作为默认文件名，IE就会在空格位置通过+号填补
        ///2.中文字符乱码，准确的是非 ASCII 字符乱码，当原文件的文件名中含有非 ASCII 字符时，将引发客户端获取到的文件名错乱
        ///3.一些特殊字符不能被正常输出（当然这里我并不是那些不常见的符号）比如“.”在IE下就会变为“[1].”
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string FormatDownFileName(string fileName)
        {
            string re = string.Empty;
            string encodefileName = ToHexString(fileName);       //使用自定义的
            if (HttpContext.Current.Request.Browser.Browser.Contains("IE"))
            {
                string ext = encodefileName.Substring(encodefileName.LastIndexOf('.'));//得到扩展名
                string name = encodefileName.Remove(encodefileName.Length - ext.Length);//得到文件名称
                name = name.Replace(".", "%2e"); //关键代码
                re = name + ext;
            }
            else
            {
                re = encodefileName;
            }
            return re;
        }

        #endregion

    }
}
