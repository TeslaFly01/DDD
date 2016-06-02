using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using DDD.Utility;

namespace DDD.Utility
{
    /// <summary>
    /// 数字水印
    /// </summary>
    public class WaterMarkImg
    {

        public WaterMarkImg()
        { }

        public enum DealType { WM_IMAGE, WM_TEXT };
        public enum WaterPos { WM_TOP_LEFT, WM_TOP_RIGHT, WM_BOTTOM_RIGHT, WM_BOTTOM_LEFT };

        /// <summary>
        /// 添加图片水印
        /// </summary>
        /// <param name="oldpath">原图片绝对地址</param>
        /// <param name="newpath">新图片放置的绝对地址</param>
        /// <param name="WatermarkText">水印文字</param>
        /// <param name="Watermarkimgpath">水印图片绝对地址</param>
        public bool addWaterMark(string oldpath, string newpath, DealType dealtype, WaterPos WatermarkPosition, string WatermarkText, string Watermarkimgpath)
        {
            bool re = false;
            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(oldpath);

                foreach (Guid guid in image.FrameDimensionsList)//判断是否是GIF动画：是－则不加水印
                {
                    FrameDimension dimension = new FrameDimension(guid);
                    if (image.GetFrameCount(dimension) > 1)
                    {
                        image.Dispose();
                        return false;
                    }
                }

                Bitmap b = new Bitmap(image.Width, image.Height, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(b);
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.High;

                g.DrawImage(image, 0, 0, image.Width, image.Height);

                switch (dealtype)
                {
                    //是图片的话               
                    case DealType.WM_IMAGE:
                        this.addWatermarkImage(g, Watermarkimgpath, WatermarkPosition, image.Width, image.Height);
                        break;
                    //如果是文字                    
                    case DealType.WM_TEXT:
                        this.addWatermarkText(g, WatermarkText, WatermarkPosition, image.Width, image.Height);
                        break;
                }

                //b.Save(newpath);
                //直接save加水印后图片容量大小会成几倍增加
                EncoderParameters parameters = new EncoderParameters(1);
                parameters.Param[0] = new EncoderParameter(Encoder.Quality, ((long)80));
                b.Save(newpath, ImageHelper.GetCodecInfo("image/" + ImageHelper.GetFormat(newpath).ToString().ToLower()), parameters);

                b.Dispose();
                g.Dispose();
                image.Dispose();

                re = true;
            }
            catch
            {
                re = false;
            }
            return re;
        }


        /// <summary>
        ///  加水印文字
        /// </summary>
        /// <param name="picture">imge 对象</param>
        /// <param name="_watermarkText">水印文字内容</param>
        /// <param name="_watermarkPosition">水印位置</param>
        /// <param name="_width">被加水印图片的宽</param>
        /// <param name="_height">被加水印图片的高</param>
        private void addWatermarkText(Graphics picture, string _watermarkText, WaterPos _watermarkPosition, int _width, int _height)
        {
            int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };
            Font crFont = null;
            SizeF crSize = new SizeF();
            for (int i = 0; i < 7; i++)
            {
                crFont = new Font("arial", sizes[i], FontStyle.Bold);
                crSize = picture.MeasureString(_watermarkText, crFont);

                if ((ushort)crSize.Width < (ushort)_width)
                    break;
            }

            float xpos = 0;
            float ypos = 0;

            switch (_watermarkPosition)
            {
                case WaterPos.WM_TOP_LEFT:
                    xpos = ((float)_width * (float).01) + (crSize.Width / 2);
                    ypos = (float)_height * (float).01;
                    break;
                case WaterPos.WM_TOP_RIGHT:
                    xpos = ((float)_width * (float).99) - (crSize.Width / 2);
                    ypos = (float)_height * (float).01;
                    break;
                case WaterPos.WM_BOTTOM_RIGHT:
                    xpos = ((float)_width * (float).99) - (crSize.Width / 2);
                    ypos = ((float)_height * (float).99) - crSize.Height;
                    break;
                case WaterPos.WM_BOTTOM_LEFT:
                    xpos = ((float)_width * (float).01) + (crSize.Width / 2);
                    ypos = ((float)_height * (float).99) - crSize.Height;
                    break;
            }

            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;

            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));
            picture.DrawString(_watermarkText, crFont, semiTransBrush2, xpos + 1, ypos + 1, StrFormat);

            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));
            picture.DrawString(_watermarkText, crFont, semiTransBrush, xpos, ypos, StrFormat);

            semiTransBrush2.Dispose();
            semiTransBrush.Dispose();

        }


        ///<summary>
        ///  加水印图片
        /// </summary>
        /// <param name="picture">imge 对象</param>
        /// <param name="WaterMarkPicPath">水印图片的地址</param>
        /// <param name="_watermarkPosition">水印位置</param>
        /// <param name="_width">被加水印图片的宽</param>
        /// <param name="_height">被加水印图片的高</param>
        private void addWatermarkImage(Graphics picture, string WaterMarkPicPath, WaterPos _watermarkPosition, int _width, int _height)
        {
            Image watermark = new Bitmap(WaterMarkPicPath);

            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();

            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float[][] colorMatrixElements = {
                                                 new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                                 new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                 new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                                 new float[] {0.0f,  0.0f,  0.0f,  0.3f, 0.0f},
                                                 new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                             };

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            int xpos = 0;
            int ypos = 0;
            int WatermarkWidth = 0;
            int WatermarkHeight = 0;
            double bl = 1d;
            //计算水印图片的比率
            //取背景的1/4宽度来比较
            if ((_width > watermark.Width * 4) && (_height > watermark.Height * 4))
            {
                bl = 1;
            }
            else if ((_width > watermark.Width * 4) && (_height < watermark.Height * 4))
            {
                bl = Convert.ToDouble(_height / 4) / Convert.ToDouble(watermark.Height);

            }
            else

                if ((_width < watermark.Width * 4) && (_height > watermark.Height * 4))
                {
                    bl = Convert.ToDouble(_width / 4) / Convert.ToDouble(watermark.Width);
                }
                else
                {
                    if ((_width * watermark.Height) > (_height * watermark.Width))
                    {
                        bl = Convert.ToDouble(_height / 4) / Convert.ToDouble(watermark.Height);

                    }
                    else
                    {
                        bl = Convert.ToDouble(_width / 4) / Convert.ToDouble(watermark.Width);

                    }

                }

            WatermarkWidth = Convert.ToInt32(watermark.Width * bl);
            WatermarkHeight = Convert.ToInt32(watermark.Height * bl);


            switch (_watermarkPosition)
            {
                case WaterPos.WM_TOP_LEFT:
                    xpos = 10;
                    ypos = 10;
                    break;
                case WaterPos.WM_TOP_RIGHT:
                    xpos = _width - WatermarkWidth - 10;
                    ypos = 10;
                    break;
                case WaterPos.WM_BOTTOM_RIGHT:
                    xpos = _width - WatermarkWidth - 10;
                    ypos = _height - WatermarkHeight - 10;
                    break;
                case WaterPos.WM_BOTTOM_LEFT:
                    xpos = 10;
                    ypos = _height - WatermarkHeight - 10;
                    break;
            }

            picture.DrawImage(watermark, new Rectangle(xpos, ypos, WatermarkWidth, WatermarkHeight), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);

            watermark.Dispose();
            imageAttributes.Dispose();
        }

    }

}
