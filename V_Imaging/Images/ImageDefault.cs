using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Draw.Images
{
    public class ImageDefault : Image
    {
        //remembers the width and height of the image
        private int width;
        private int height;

        //stores the size of one pixel in bites
        private int bite_size;

        //stores the pixel format
        private PixelFormat format;

        //stores the image data as a single bite array
        private byte[] data;

        public ImageDefault(int width, int height)
        {
            this.width = width;
            this.height = height;

            bite_size = 4;
            format = PixelFormat.Rgba32;

            data = new byte[width * height * bite_size];
        }


        public ImageDefault(int width, int height, PixelFormat format)
        {
            this.width = width;
            this.height = height;

            bite_size = GetNumBites(format);
            this.format = format;

            data = new byte[width * height * bite_size];
        }

        #region Class Properties...

        /// <summary>
        /// The width of the current image. Read-Only
        /// </summary>
        public override int Width
        {
            get { return width; }
        }

        /// <summary>
        /// The height of the current image. Read-Only
        /// </summary>
        public override int Height
        {
            get { return height; }
        }

        /// <summary>
        /// Determins the format used to store the pixel data
        /// </summary>
        public PixelFormat Format
        {
            get { return format; }
        }

        /// <summary>
        /// Returns the number of bits used per pixel.
        /// </summary>
        public int BitDepth
        {
            get { return bite_size * 8; }
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Image Implementaiton...

        /// <summary>
        /// Provides access to the internal pixel data. This method should
        /// only ever be called with bounded indicies.
        /// </summary>
        /// <param name="col">Column from the left</param>
        /// <param name="row">Row from the top</param>
        /// <returns>The color of the desired pixel</returns>
        protected override Color GetPixelInit(int col, int row)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Provides access to the internal pixel data. This method should
        /// only ever be called with bounded indicies.
        /// </summary>
        /// <param name="col">Column from the left</param>
        /// <param name="row">Row from the top</param>
        /// <param name="color">New color of the pixel</param>
        protected override void SetPixelInit(int col, int row, Color color)
        {
            throw new NotImplementedException();
        }

        #endregion //////////////////////////////////////////////////////////////


        


        private static int GetNumBites(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.Rgba16: return 2;
                case PixelFormat.Rgba32: return 4;
                case PixelFormat.Rgba48: return 6;
                case PixelFormat.Rgba64: return 8;

                case PixelFormat.Rgb15: return 2;
                case PixelFormat.Rgb24: return 3;
                case PixelFormat.Rgb30: return 4;
                case PixelFormat.Rgb48: return 6;

                case PixelFormat.Yuv16: return 2;
                case PixelFormat.Yuv32: return 4;

                case PixelFormat.Grey8: return 1;
                case PixelFormat.Grey16: return 2;
            }

            //we are unable to determin the pixel format
            throw new NotSupportedException();
        }

        private Color GetRgba16(int index)
        {
            ushort pix = BitConverter.ToUInt16(data, index);

            uint rint = (pix & 0xF000u) >> 12;
            uint gint = (pix & 0x0F00u) >> 8;
            uint bint = (pix & 0x00F0u) >> 4;
            uint aint = (pix & 0x000Fu) >> 0;

            double r = rint / 15.0;
            double g = gint / 15.0;
            double b = bint / 15.0;
            double a = aint / 15.0;

            return Color.FromRGB(r, g, b, a);
        }

        private Color GetRgba24(int index)
        {
            byte[] temp = new byte[4];

            temp[0] = data[index];
            temp[1] = data[index + 1];
            temp[2] = data[index + 3];
            temp[4] = 0;

            uint pix = BitConverter.ToUInt32(temp, 0);

            uint rint = (pix & 0x00FC0000u) >> 18;
            uint gint = (pix & 0x0003F000u) >> 12;
            uint bint = (pix & 0x00000FC0u) >> 6;
            uint aint = (pix & 0x0000003Fu) >> 0;

            double r = rint / 63.0;
            double g = gint / 63.0;
            double b = bint / 63.0;
            double a = aint / 63.0;

            return Color.FromRGB(r, g, b, a);
        }

        private Color GetRgba24_alt(int index)
        {
            byte byte0 = data[index];
            byte byte1 = data[index + 1];
            byte byte2 = data[index + 2];

            uint rint = (byte0 & 0xFCu) >> 2;
            uint gint = ((byte0 & 0x03u) << 4) + ((byte1 & 0xF0u) >> 4);
            uint bint = ((byte1 & 0x0Fu) << 2) + ((byte2 & 0xC0u) >> 6);
            uint aint = (byte2 & 0x3Fu) >> 0;

            double r = rint / 63.0;
            double g = gint / 63.0;
            double b = bint / 63.0;
            double a = aint / 63.0;

            return Color.FromRGB(r, g, b, a);
        }

        private Color GetRgba32(int index)
        {
            uint rint = data[index];
            uint bint = data[index + 1];
            uint gint = data[index + 2];
            uint aint = data[index + 3];

            double r = rint / 255.0;
            double g = gint / 255.0;
            double b = bint / 255.0;
            double a = aint / 255.0;

            return Color.FromRGB(r, g, b, a);
        }

        private Color GetRgba48(int index)
        {
            byte[] temp = new byte[8];

            temp[0] = data[index];
            temp[1] = data[index + 1];
            temp[2] = data[index + 2];
            temp[3] = data[index + 3];
            temp[4] = data[index + 4];
            temp[5] = data[index + 5];
            temp[6] = 0;
            temp[7] = 0;

            ulong pix = BitConverter.ToUInt64(temp, 0);

            ulong rint = (pix & 0x0000FFF000000000u) >> 36;
            ulong gint = (pix & 0x0000000FFF000000u) >> 24;
            ulong bint = (pix & 0x0000000000FFF000u) >> 12;
            ulong aint = (pix & 0x0000000000000FFFu) >> 0;

            double r = rint / 4095.0;
            double g = gint / 4095.0;
            double b = bint / 4095.0;
            double a = aint / 4095.0;

            return Color.FromRGB(r, g, b, a);
        }

        private Color GetRgba48_alt(int index)
        {
            byte byte0 = data[index];
            byte byte1 = data[index + 1];
            byte byte2 = data[index + 2];
            byte byte3 = data[index + 3];
            byte byte4 = data[index + 4];
            byte byte5 = data[index + 5];

            uint rint = ((byte0 & 0xFFu) << 4) + ((byte1 & 0xF0u) >> 4);
            uint gint = ((byte1 & 0x0Fu) << 8) + ((byte2 & 0xFFu) >> 0);
            uint bint = ((byte3 & 0xFFu) << 4) + ((byte4 & 0xF0u) >> 4);
            uint aint = ((byte4 & 0x0Fu) << 8) + ((byte5 & 0xFFu) >> 0);

            double r = rint / 4095.0;
            double g = gint / 4095.0;
            double b = bint / 4095.0;
            double a = aint / 4095.0;

            return Color.FromRGB(r, g, b, a);
        }


        private Color GetRgba64(int index)
        {
            uint rint = BitConverter.ToUInt16(data, index);
            uint bint = BitConverter.ToUInt16(data, index + 2);
            uint gint = BitConverter.ToUInt16(data, index + 4);
            uint aint = BitConverter.ToUInt16(data, index + 6);

            double r = rint / 65535.0;
            double g = gint / 65535.0;
            double b = bint / 65535.0;
            double a = aint / 65535.0;

            return Color.FromRGB(r, g, b, a);
        }


        private Color GetRgb15(int index)
        {
            ushort pix = BitConverter.ToUInt16(data, index);

            uint rint = (pix & 0x7C00u) >> 10;
            uint gint = (pix & 0x03E0u) >> 5;
            uint bint = (pix & 0x001Fu) >> 0;

            double r = rint / 31.0;
            double g = gint / 31.0;
            double b = bint / 31.0;

            return Color.FromRGB(r, g, b);
        }


        private Color GetRgb24(int index)
        {
            uint rint = data[index];
            uint gint = data[index + 1];
            uint bint = data[index + 2];

            double r = rint / 255.0;
            double g = gint / 255.0;
            double b = bint / 255.0;

            return Color.FromRGB(r, g, b);
        }


        private Color GetRgb30(int index)
        {
            uint pix = BitConverter.ToUInt32(data, index);

            uint rint = (pix & 0x3FF00000u) >> 20;
            uint gint = (pix & 0x000FFC00u) >> 10;
            uint bint = (pix & 0x000003FFu) >> 0;

            double r = rint / 1023.0;
            double g = gint / 1023.0;
            double b = bint / 1023.0;

            return Color.FromRGB(r, g, b);
        }


        private Color GetRgb48(int index)
        {
            uint rint = BitConverter.ToUInt16(data, index);
            uint bint = BitConverter.ToUInt16(data, index + 2);
            uint gint = BitConverter.ToUInt16(data, index + 4);

            double r = rint / 65535.0;
            double g = gint / 65535.0;
            double b = bint / 65535.0;

            return Color.FromRGB(r, g, b);
        }


        private Color GetYuv16(int index)
        {
            ushort pix = BitConverter.ToUInt16(data, index);

            uint yint = (pix & 0xFF00u) >> 8;
            uint aint = (pix & 0x00F0u) >> 4;
            uint bint = (pix & 0x000Fu) >> 0;

            double y = yint / 255.0;
            double u = (aint / 15.0) - 0.5;
            double v = (bint / 15.0) - 0.5;

            return Color.FromYUV(y, u, v);
        }

        private Color GetYuv16_alt(int index)
        {
            ushort pix = BitConverter.ToUInt16(data, index);

            //uses a 6-5-5 bit structor
            uint yint = (pix & 0xFC00u) >> 10;
            uint aint = (pix & 0x03E0u) >> 5;
            uint bint = (pix & 0x001Fu) >> 0;

            //the top most level of U and V are not used
            double y = yint / 63.0;
            double u = (aint / 30.0) - 0.5;
            double v = (bint / 30.0) - 0.5;

            return Color.FromYUV(y, u, v);
        }

        private Color GetYuv32(int index)
        {
            uint yint = BitConverter.ToUInt16(data, index);
            uint aint = data[index + 2];
            uint bint = data[index + 3];

            double y = yint / 65535.0;
            double u = (aint / 255.0) - 0.5;
            double v = (bint / 255.0) - 0.5;

            return Color.FromYUV(y, u, v);
        }

        private Color GetYuv32_alt(int index)
        {
            uint pix = BitConverter.ToUInt32(data, index);

            //uses a 12-10-10 bit pattern
            uint yint = (pix & 0xFFF00000u) >> 20;
            uint aint = (pix & 0x000FFC00u) >> 10;
            uint bint = (pix & 0x000003FFu) >> 0;

            //the top 3 levels of U and V are not used
            double y = yint / 4095.0;
            double u = (aint / 1020.0) - 0.5;
            double v = (bint / 1020.0) - 0.5;

            return Color.FromYUV(y, u, v);
        }

        private Color GetRc16(int index)
        {
            uint rint = data[index];
            uint cint = data[index + 1];

            double r = rint / 255.0;
            double c = cint / 255.0;

            return Color.FromRGB(r, c, c);
        }

        private Color GetRc32(int index)
        {
            uint rint = BitConverter.ToUInt16(data, index);
            uint cint = BitConverter.ToUInt16(data, index + 2);

            double r = rint / 65535.0;
            double c = cint / 65535.0;

            return Color.FromRGB(r, c, c);
        }

        private Color GetGrey8(int index)
        {
            double val = data[index] / 256.0;
            return Color.FromRGB(val, val, val);
        }

        private Color GetGrey16(int index)
        {
            uint temp = BitConverter.ToUInt16(data, index);
            double val = temp / 65535.0;
            return Color.FromRGB(val, val, val);
        }
    }
}
