using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Vulpine.Core.Draw;

using VColor = Vulpine.Core.Draw.Color;
using SColor = System.Drawing.Color;

namespace ImagingTests
{
    public class ImageSys : Vulpine.Core.Draw.Image
    {
        //stores the internal bitmap
        private Bitmap bmp;

        private object key;
        private int width;
        private int height;

        public ImageSys(Bitmap bmp)
        {
            this.bmp = bmp;

            key = new object();
            width = bmp.Width;
            height = bmp.Height;
        }

        public override int Width
        {
            get { return width; }
        }

        public override int Height
        {
            get { return height; }
        }

        public object Key
        {
            get { return key; }
        }

        protected override VColor GetPixelInternal(int col, int row)
        {
            SColor sc;

            lock (key)
            {
                sc = bmp.GetPixel(col, row);
            }

            //sc = bmp.GetPixel(col, row);

            double r = sc.R / 255.0;
            double g = sc.G / 255.0;
            double b = sc.B / 255.0;

            return VColor.FromRGB(r, g, b);
        }

        protected override void SetPixelInternal(int col, int row, VColor vc)
        {
            byte r = (byte)Math.Floor((vc.Red * 255.0) + 0.5);
            byte g = (byte)Math.Floor((vc.Green * 255.0) + 0.5);
            byte b = (byte)Math.Floor((vc.Blue * 255.0) + 0.5);

            SColor sc = SColor.FromArgb(r, g, b);

            lock (key)
            {
                bmp.SetPixel(col, row, sc);
            }
        }

        public static implicit operator ImageSys(Bitmap bmp)
        { return new ImageSys(bmp); }

        public static explicit operator Bitmap(ImageSys img)
        { return img.bmp; }
    }
}
