using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace DDD.Utility.ValidateCode
{
    /// <summary>
    /// 线条+噪点干扰 (彩色字、干扰)
    /// </summary>
    public class ValidateCode_Style1 : ValidateCodeType
    {
        private Color backgroundColor = Color.White;
        private bool chaos = true;
        private Color chaosColor = Color.FromArgb(170, 170, 0x33);
        //private Color[] drawColors = new Color[] { Color.FromArgb(170, 170, 0x33) };
        private Color[] drawColors = new Color[] { Color.FromArgb(0x08, 0x2e, 0x54), Color.FromArgb(0x9c, 0x66, 0x1f), Color.FromArgb(0xff, 0x45, 0x00), Color.FromArgb(0x33, 0xa1, 0xc9), Color.FromArgb(0xa0, 0x20, 0xf0), Color.FromArgb(0x8b, 0x45, 0x13), Color.FromArgb(0x22, 0x8b, 0x22), Color.FromArgb(0x6a, 0x5a, 0xcd), Color.FromArgb(0x29, 0x24, 0x21) };
        private bool fontTextRenderingHint;
        private int imageHeight = 30;
        private int padding = 1;
        private int validataCodeLength = 4;
        private int validataCodeSize = 0x10;
        private string validateCodeFont = "Arial";

        public override byte[] CreateImage(out string validataCode)
        {
            Bitmap bitmap;
            string formatString = "A,b,C,D,e,F,G,H,i,j,K,m,n,P,q,R,S,T,U,V,w,x,y,Z,3,4,5,7,8,9";
            GetRandom(formatString, this.ValidataCodeLength, out validataCode);
            MemoryStream stream = new MemoryStream();
            this.ImageBmp(out bitmap, validataCode);
            bitmap.Save(stream, ImageFormat.Png);
            bitmap.Dispose();
            bitmap = null;
            stream.Close();
            stream.Dispose();
            return stream.GetBuffer();
        }

        private void CreateImageBmp(ref Bitmap bitMap, string validateCode,Color color)
        {
            Graphics graphics = Graphics.FromImage(bitMap);
            Random random = new Random();
            if (this.fontTextRenderingHint)
            {
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            }
            else
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            Font font = new Font(this.validateCodeFont, (float)this.validataCodeSize, FontStyle.Regular);
            int maxValue = Math.Max((this.ImageHeight - this.validataCodeSize) - 5, 0);
            for (int i = 0; i < this.validataCodeLength; i++)
            {                
                Brush brush = new SolidBrush(color);
                int[] numArray = new int[] { ((i * this.validataCodeSize) + random.Next(1)) + 3, random.Next(maxValue) - 4 };
                Point point = new Point(numArray[0], numArray[1]);
                graphics.DrawString(validateCode[i].ToString(), font, brush, (PointF)point);
            }
            graphics.Dispose();
        }

        private void DisposeImageBmp(ref Bitmap bitmap, Color color)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            Pen pen = new Pen(color, 0.5f);
            Point[] pointArray = new Point[2];
            Random random = new Random();
            if (this.Chaos)
            {
                for (int i = 0; i < (this.validataCodeLength * 1); i++) //线条
                {
                    pointArray[0] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                    pointArray[1] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                    graphics.DrawLine(pen, pointArray[0], pointArray[1]);
                }
                for (int i = 0; i < (this.validataCodeLength * 6); i++) //噪点
                {
                    int x = random.Next(bitmap.Width);
                    int y = random.Next(bitmap.Height);
                    graphics.DrawRectangle(pen, x, y, 1, 1);
                }
            }
            graphics.Dispose();
        }

        private static void GetRandom(string formatString, int len, out string codeString)
        {
            codeString = string.Empty;
            string[] strArray = formatString.Split(new char[] { ',' });
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                int index = random.Next(0x186a0) % strArray.Length;
                codeString = codeString + strArray[index].ToString();
            }
        }

        private void ImageBmp(out Bitmap bitMap, string validataCode)
        {
            int width = (int)(((this.validataCodeLength * this.validataCodeSize) * 1.3) + 4.0);
            bitMap = new Bitmap(width, this.ImageHeight);
            Random random = new Random();
            Color color = this.DrawColors[random.Next(this.DrawColors.Length)];
            this.DisposeImageBmp(ref bitMap,color);
            this.CreateImageBmp(ref bitMap, validataCode,color);
        }

        public Color BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }
            set
            {
                this.backgroundColor = value;
            }
        }

        public bool Chaos
        {
            get
            {
                return this.chaos;
            }
            set
            {
                this.chaos = value;
            }
        }

        public Color ChaosColor
        {
            get
            {
                return this.chaosColor;
            }
            set
            {
                this.chaosColor = value;
            }
        }

        public Color[] DrawColors
        {
            get
            {
                return this.drawColors;
            }
            set
            {
                this.drawColors = value;
            }
        }

        private bool FontTextRenderingHint
        {
            get
            {
                return this.fontTextRenderingHint;
            }
            set
            {
                this.fontTextRenderingHint = value;
            }
        }

        public int ImageHeight
        {
            get
            {
                return this.imageHeight;
            }
            set
            {
                this.imageHeight = value;
            }
        }

        public override string Name
        {
            get
            {
                return "线条干扰(蓝色)";
            }
        }

        public int Padding
        {
            get
            {
                return this.padding;
            }
            set
            {
                this.padding = value;
            }
        }

        public int ValidataCodeLength
        {
            get
            {
                return this.validataCodeLength;
            }
            set
            {
                this.validataCodeLength = value;
            }
        }

        public int ValidataCodeSize
        {
            get
            {
                return this.validataCodeSize;
            }
            set
            {
                this.validataCodeSize = value;
            }
        }

        public string ValidateCodeFont
        {
            get
            {
                return this.validateCodeFont;
            }
            set
            {
                this.validateCodeFont = value;
            }
        }
    }
}
