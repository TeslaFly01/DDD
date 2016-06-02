using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Web;

namespace DDD.Utility
{
    public class ImageHelper
    {
        public ImageHelper()
        {

        }

        /// <summary>
        /// 默认生成高画质缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式:HW W H Cut </param>    
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            MakeThumbnail(originalImagePath, thumbnailPath, width, height, mode, InterpolationMode.High, SmoothingMode.HighQuality);
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式:HW W H Cut </param>    
        /// <param name="interpolMode">插值算法</param>
        /// <param name="smoothingMode">平滑处理</param>
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode, InterpolationMode interpolMode, SmoothingMode smoothingMode)
        {
            Image originalImage = Image.FromFile(originalImagePath);
            int ow = originalImage.Width;
            int oh = originalImage.Height;
            int towidth = width > ow ? ow : width;
            int toheight = height > oh ? oh : height;

            int x = 0;
            int y = 0;


            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight, PixelFormat.Format32bppPArgb);
            Image bitmap2 = null;

            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = interpolMode;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = smoothingMode;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //清空画布
            g.Clear(Color.White);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), x, y, ow, oh, GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                //bitmap.Save(thumbnailPath);直接保存忒太了
                EncoderParameters parameters = new EncoderParameters(1);
                parameters.Param[0] = new EncoderParameter(Encoder.Quality, ((long)80));

                bitmap2 = new Bitmap(bitmap);
                bitmap.Dispose();
                g.Dispose();

                bitmap2.Save(thumbnailPath, ImageHelper.GetCodecInfo("image/" + ImageHelper.GetFormat(thumbnailPath).ToString().ToLower()), parameters);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                if (bitmap2 != null)
                {
                    bitmap2.Dispose();
                }
                g.Dispose();
            }
        }

        /// <summary>
        /// 得到图片格式
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <returns></returns>
        public static ImageFormat GetFormat(string name)
        {
            string ext = name.Substring(name.LastIndexOf(".") + 1);
            switch (ext.ToLower())
            {
                case "jpg":
                case "jpeg":
                    return ImageFormat.Jpeg;
                case "bmp":
                    return ImageFormat.Bmp;
                case "png":
                    return ImageFormat.Png;
                case "gif":
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Jpeg;
            }
        }

        /// <summary>
        /// 获取图像编码解码器的所有相关信息
        /// </summary>
        /// <param name="mimeType">包含编码解码器的多用途网际邮件扩充协议 (MIME) 类型的字符串</param>
        /// <returns>返回图像编码解码器的所有相关信息</returns>
        public static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }

        /// <summary>
        /// 计算新尺寸
        /// </summary>
        /// <param name="width">原始宽度</param>
        /// <param name="height">原始高度</param>
        /// <param name="maxWidth">最大新宽度</param>
        /// <param name="maxHeight">最大新高度</param>
        /// <returns></returns>
        public static Size ResizeImage(int width, int height, int maxWidth, int maxHeight)
        {
            decimal MAX_WIDTH = (decimal)maxWidth;
            decimal MAX_HEIGHT = (decimal)maxHeight;
            decimal ASPECT_RATIO = MAX_WIDTH / MAX_HEIGHT;

            int newWidth, newHeight;

            decimal originalWidth = (decimal)width;
            decimal originalHeight = (decimal)height;

            if (originalWidth > MAX_WIDTH || originalHeight > MAX_HEIGHT)
            {
                decimal factor;
                // determine the largest factor 
                if (originalWidth / originalHeight > ASPECT_RATIO)
                {
                    factor = originalWidth / MAX_WIDTH;
                    newWidth = Convert.ToInt32(originalWidth / factor);
                    newHeight = Convert.ToInt32(originalHeight / factor);
                }
                else
                {
                    factor = originalHeight / MAX_HEIGHT;
                    newWidth = Convert.ToInt32(originalWidth / factor);
                    newHeight = Convert.ToInt32(originalHeight / factor);
                }
            }
            else
            {
                newWidth = width;
                newHeight = height;
            }

            return new Size(newWidth, newHeight);

        }

        public static Size ResizeImage(string ff, int maxWidth, int maxHeight)
        {
            try
            {
                Bitmap bitmap = new Bitmap(ff);
                return ResizeImage(bitmap.Width, bitmap.Height, maxWidth, maxHeight);
            }
            catch
            {
                return new Size(maxWidth, maxHeight);
            }
        }

        public static int GetResizeImageW(string ff, int maxWidth, int maxHeight)
        {
            try
            {
                return ResizeImage(ff, maxWidth, maxHeight).Width;
            }
            catch
            {
                return maxWidth;
            }
        }

        public static int GetResizeImageH(string ff, int maxWidth, int maxHeight)
        {
            try
            {
                return ResizeImage(ff, maxWidth, maxHeight).Height;
            }
            catch
            {
                return maxHeight;
            }
        }

        public static string GetResizeImage4Html(string ff, int maxWidth, int maxHeight)
        {
            Size size = ResizeImage(ff, maxWidth, maxHeight);
            return "width=" + size.Width.ToString() + "px height=" + size.Height.ToString() + "px";
        }

        public static void CreateResizeImage(string temp_url, string oldurl, string _authKey)
        {
            #region 生成缩略图 by 2015-7-24 TODO:后期有空将此部分代码放入穿透层工具类中
            WebRequest request = WebRequest.Create(oldurl);
            var response = (HttpWebResponse)request.GetResponse();
            var dataStream = response.GetResponseStream();
            Image thisImg = new Bitmap(dataStream);
            var proportion = 0.77;
            var w = thisImg.Width;
            var h = thisImg.Height;
            var t = 0;
            var l = 0;
            if (thisImg.Width > thisImg.Height)
            {
                w = (int)Math.Round((thisImg.Height) / proportion, 0);
                l = -(int)(thisImg.Width - w) / 2;
            }
            else
            {
                if (thisImg.Width < thisImg.Height)
                {
                    h = (int)Math.Round((thisImg.Width) * proportion, 0);
                    t = -(int)(thisImg.Height - h) / 2;
                }
            }
            var newimg = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(newimg);
            g.DrawImage(thisImg, l, t);
            g.Save();
            var ms = new MemoryStream();
            newimg.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            //强制回收
            newimg.Dispose();
            g.Dispose();
            thisImg.Dispose();
            WebRequest temp_request = WebRequest.Create(temp_url);
            temp_request.Method = "POST";
            temp_request.Timeout = 1000 * 60 * 3; //重置执行活动扫描标志服务超时：3分钟
            temp_request.Headers.Add("Authorization", _authKey);
            var temp_bt = ms.ToArray();
            temp_request.ContentLength = temp_bt.Length;
            var temp_dataStream = temp_request.GetRequestStream();
            temp_dataStream.Write(temp_bt, 0, temp_bt.Length);
            ms.Dispose();
            GC.Collect();
            #endregion
        }
        #region GetTnImg

        /// <summary>
        /// 默认取得高画质缩微图地址，不存在则重新生成
        /// </summary>
        /// <param name="orgImg">源图路径（相对路径[数据库存放地址]）</param>
        /// <param name="tnPrefix">缩微图前缀名称</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mode">生成缩略图的方式:HW W H Cut </param>
        /// <param name="interpolMode">插值算法</param>
        /// <param name="smoothingMode">平滑处理</param>
        /// <returns></returns>
        public static string GetTNimg(string orgImg, string tnPrefix, int width, int height, string mode)
        {
            return GetTNimg(orgImg, tnPrefix, width, height, mode, InterpolationMode.High, SmoothingMode.HighQuality);
        }

        /// <summary>
        /// 取得缩微图地址，不存在则重新生成
        /// </summary>
        /// <param name="orgImg">源图路径（相对路径[数据库存放地址]）</param>
        /// <param name="tnPrefix">缩微图前缀名称</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mode">生成缩略图的方式:HW W H Cut </param>
        /// <returns></returns>
        public static string GetTNimg(string orgImg, string tnPrefix, int width, int height, string mode, InterpolationMode interpolMode, SmoothingMode smoothingMode)
        {
            string tnImg = orgImg.Insert(orgImg.LastIndexOf("/") + 1, tnPrefix);
            if (File.Exists(HttpContext.Current.Server.MapPath(orgImg)))
            {
                if (!File.Exists(HttpContext.Current.Server.MapPath(tnImg)))
                {
                    ImageHelper.MakeThumbnail(HttpContext.Current.Server.MapPath(orgImg), HttpContext.Current.Server.MapPath(tnImg), width, height, mode, interpolMode, smoothingMode);
                }
            }
            return tnImg;
        }

        /// <summary>
        /// 取得缩微图地址，不判断是否存在而重新生成
        /// </summary>
        /// <param name="orgImg">源图路径（相对路径[数据库存放地址]）</param>
        /// <param name="tnPrefix">缩微图前缀名称</param>
        /// <returns></returns>
        public static string GetTNimg(string orgImg, string tnPrefix)
        {
            string tnImg = orgImg.Insert(orgImg.LastIndexOf("/") + 1, tnPrefix);

            return tnImg;
        }

        #endregion

    }


}
