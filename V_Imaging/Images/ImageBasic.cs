/**
 *  This file is an integral part of the Vulpine Core Library
 *  Copyright (c) 2016-2019 Benjamin Jacob Dawson
 *
 *      http://www.jakesden.com/corelibrary.html
 *
 *  The Vulpine Core Library is free software; you can redistribute it 
 *  and/or modify it under the terms of the GNU Lesser General Public
 *  License as published by the Free Software Foundation; either
 *  version 2.1 of the License, or (at your option) any later version.
 *
 *  The Vulpine Core Library is distributed in the hope that it will 
 *  be useful, but WITHOUT ANY WARRANTY; without even the implied 
 *  warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
 *  See the GNU Lesser General Public License for more details.
 *
 *      https://www.gnu.org/licenses/lgpl-2.1.html
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with this library; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Draw.Images
{
    public class ImageBasic : Image
    {
        //remembers the width and height of the image
        private int width;
        private int height;

        //stores the size of one pixel in bites
        private int bite_size;

        //stores the pixel format
        private PixelFormat2 format;

        //stores the image data as a single bite array
        private byte[] data;

        public ImageBasic(int width, int height)
        {
            this.width = width;
            this.height = height;

            bite_size = 4;
            format = PixelFormat2.Rgba32;

            data = new byte[width * height * bite_size];
        }


        public ImageBasic(int width, int height, PixelFormat2 format)
        {
            this.width = width;
            this.height = height;

            bite_size = format.BitLength / 8;
            this.format = format;

            data = new byte[width * height * bite_size];
        }

        #region Class Properties...

        /// <summary>
        /// The width of the current image.
        /// </summary>
        public override int Width
        {
            get { return width; }
        }

        /// <summary>
        /// The height of the current image.
        /// </summary>
        public override int Height
        {
            get { return height; }
        }

        /// <summary>
        /// Determins the format used to store the pixel data.
        /// </summary>
        public PixelFormat2 Format
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

        /// <summary>
        /// Indicates how many chanels the image can store. Each chanel can be
        /// thought of as a seperate greyscale image. Greyscale images only need
        /// a single channel, while full color images need three. If transparancy
        /// information is included, then four chanels are needed.
        /// </summary>
        public int NumChanels
        {
            get { return format.NumChanels; }
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
            //locates the desiered pixel
            int index = GetStartIndex(col, row);

            return format.DecodeColor(data, index);

            //switch (format)
            //{
            //    case PixelFormat.Rgba16: return GetRgba16(index);
            //    case PixelFormat.Rgba32: return GetRgba32(index);
            //    case PixelFormat.Rgba64: return GetRgba64(index);

            //    case PixelFormat.Rgb15: return GetRgb15(index);
            //    case PixelFormat.Rgb24: return GetRgb24(index);
            //    case PixelFormat.Rgb48: return GetRgb48(index);

            //    case PixelFormat.Rc16: return GetRc16(index);
            //    case PixelFormat.Rc32: return GetRc32(index);

            //    case PixelFormat.Grey8: return GetGrey8(index);
            //    case PixelFormat.Grey16: return GetGrey16(index);
            //}

            ////we are unable to determin the pixel format
            //throw new NotSupportedException();
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
            //locates the desiered pixel
            int index = GetStartIndex(col, row);

            format.EncodeColor(data, index, color);

            //switch (format)
            //{
            //    case PixelFormat.Rgba16: SetRgba16(index, color); return;
            //    case PixelFormat.Rgba32: SetRgba32(index, color); return;
            //    case PixelFormat.Rgba64: SetRgba64(index, color); return;

            //    case PixelFormat.Rgb15: SetRgb15(index, color); return;
            //    case PixelFormat.Rgb24: SetRgb24(index, color); return;
            //    case PixelFormat.Rgb48: SetRgb48(index, color); return;

            //    case PixelFormat.Rc16: SetRc16(index, color); return;
            //    case PixelFormat.Rc32: SetRc32(index, color); return;

            //    case PixelFormat.Grey8: SetGrey8(index, color); return;
            //    case PixelFormat.Grey16: SetGrey16(index, color); return;
            //}

            ////we are unable to determin the pixel format
            //throw new NotSupportedException();
        }


        private static int Descritise(double value, double max)
        {
            return (int)Math.Floor((value * max) + 0.5);
        }

        private int GetStartIndex(int col, int row)
        {
            return ((col * height) + row) * bite_size;
        }

        #endregion //////////////////////////////////////////////////////////////


        #region Color Extraction...



        private Color GetRgba16(int index)
        {
            ushort pix = BitConverter.ToUInt16(data, index);

            double r = ((pix & 0xF000) >> 12) / 15.0;
            double g = ((pix & 0x0F00) >> 8) / 15.0;
            double b = ((pix & 0x00F0) >> 4) / 15.0;
            double a = ((pix & 0x000F) >> 0) / 15.0;

            return Color.FromRGB(r, g, b, a);
        }

        private Color GetRgba32(int index)
        {
            double r = data[index + 0] / 255.0;
            double g = data[index + 1] / 255.0;
            double b = data[index + 2] / 255.0;
            double a = data[index + 3] / 255.0;

            return Color.FromRGB(r, g, b, a);
        }

        private Color GetRgba64(int index)
        {
            int rint = BitConverter.ToUInt16(data, index);
            int gint = BitConverter.ToUInt16(data, index + 2);
            int bint = BitConverter.ToUInt16(data, index + 4);
            int aint = BitConverter.ToUInt16(data, index + 6);

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
            double r = data[index + 0] / 255.0;
            double g = data[index + 1] / 255.0;
            double b = data[index + 2] / 255.0;

            return Color.FromRGB(r, g, b);
        }

        private Color GetRgb48(int index)
        {
            int rint = BitConverter.ToUInt16(data, index);
            int gint = BitConverter.ToUInt16(data, index + 2);
            int bint = BitConverter.ToUInt16(data, index + 4);

            double r = rint / 65535.0;
            double g = gint / 65535.0;
            double b = bint / 65535.0;

            return Color.FromRGB(r, g, b);
        }

        private Color GetRc16(int index)
        {
            double r = data[index + 0] / 255.0;
            double c = data[index + 1] / 255.0;

            return Color.FromRGB(r, c, c);
        }

        private Color GetRc32(int index)
        {
            int rint = BitConverter.ToUInt16(data, index);
            int cint = BitConverter.ToUInt16(data, index + 2);

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
            int temp = BitConverter.ToUInt16(data, index);
            double val = temp / 65535.0;
            return Color.FromRGB(val, val, val);
        }


        #endregion //////////////////////////////////////////////////////////////


        #region Color Insertion...

        private void SetRgba16(int index, Color c)
        {
            int rint = Descritise(c.Red, 15.0);
            int gint = Descritise(c.Green, 15.0);
            int bint = Descritise(c.Blue, 15.0);
            int aint = Descritise(c.Alpha, 15.0);

            int pix = (rint << 12) + (gint << 8) + (bint << 4) + aint;
            byte[] bits = BitConverter.GetBytes((ushort)pix);

            data[index + 0] = bits[0];
            data[index + 1] = bits[1];
        }

        private void SetRgba32(int index, Color c)
        {
            data[index + 0] = (byte)Descritise(c.Red, 255.0);
            data[index + 1] = (byte)Descritise(c.Green, 255.0);
            data[index + 2] = (byte)Descritise(c.Blue, 255.0);
            data[index + 3] = (byte)Descritise(c.Alpha, 255.0);
        }

        private void SetRgba64(int index, Color c)
        {
            byte[] bits;

            ushort rint = (ushort)Descritise(c.Red, 65535.0);
            ushort gint = (ushort)Descritise(c.Green, 65535.0);
            ushort bint = (ushort)Descritise(c.Blue, 65535.0);
            ushort aint = (ushort)Descritise(c.Alpha, 65535.0);

            bits = BitConverter.GetBytes(rint);
            data[index + 0] = bits[0];
            data[index + 1] = bits[1];

            bits = BitConverter.GetBytes(gint);
            data[index + 2] = bits[0];
            data[index + 3] = bits[1];

            bits = BitConverter.GetBytes(bint);
            data[index + 4] = bits[0];
            data[index + 5] = bits[1];

            bits = BitConverter.GetBytes(aint);
            data[index + 6] = bits[0];
            data[index + 7] = bits[1];
        }


        private void SetRgb15(int index, Color c)
        {
            int rint = Descritise(c.Red, 31.0);
            int gint = Descritise(c.Green, 31.0);
            int bint = Descritise(c.Blue, 31.0);

            int pix = (rint << 10) + (gint << 5) + (bint << 0);
            byte[] bits = BitConverter.GetBytes((ushort)pix);

            data[index + 0] = bits[0];
            data[index + 1] = bits[1];
        }


        private void SetRgb24(int index, Color c)
        {
            data[index + 0] = (byte)Descritise(c.Red, 255.0);
            data[index + 1] = (byte)Descritise(c.Green, 255.0);
            data[index + 2] = (byte)Descritise(c.Blue, 255.0);
        }

        private void SetRgb48(int index, Color c)
        {
            byte[] bits;

            ushort rint = (ushort)Descritise(c.Red, 65535.0);
            ushort gint = (ushort)Descritise(c.Green, 65535.0);
            ushort bint = (ushort)Descritise(c.Blue, 65535.0);

            bits = BitConverter.GetBytes(rint);
            data[index + 0] = bits[0];
            data[index + 1] = bits[1];

            bits = BitConverter.GetBytes(gint);
            data[index + 2] = bits[0];
            data[index + 3] = bits[1];

            bits = BitConverter.GetBytes(bint);
            data[index + 4] = bits[0];
            data[index + 5] = bits[1];
        }

        private void SetRc16(int index, Color c)
        {
            double mix = c.Green * 0.83738 + c.Blue * 0.16262;

            data[index + 0] = (byte)Descritise(c.Red, 255.0);
            data[index + 1] = (byte)Descritise(mix, 255.0);
        }

        private void SetRc32(int index, Color c)
        {          
            byte[] bits;

            double mix = c.Green * 0.83738 + c.Blue * 0.16262;
            ushort rint = (ushort)Descritise(c.Red, 65535.0);
            ushort cint = (ushort)Descritise(mix, 65535.0);

            bits = BitConverter.GetBytes(rint);
            data[index + 0] = bits[0];
            data[index + 1] = bits[1];

            bits = BitConverter.GetBytes(cint);
            data[index + 2] = bits[0];
            data[index + 3] = bits[1];
        }

        private void SetGrey8(int index, Color c)
        {
            data[index] = (byte)Descritise(c.Luminance, 255.0);
        }

        private void SetGrey16(int index, Color c)
        {
            ushort temp = (ushort)Descritise(c.Luminance, 65535.0);
            byte[] bits = BitConverter.GetBytes(temp);

            data[index + 0] = bits[0];
            data[index + 1] = bits[1];
        }


        #endregion //////////////////////////////////////////////////////////////


    }
}
