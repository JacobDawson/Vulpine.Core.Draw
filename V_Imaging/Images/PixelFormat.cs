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
    public enum PixelFormat
    {
        Rgba16,
        Rgba32,
        Rgba64,

        Rgb15,
        Rgb24,
        Rgb48,

        Rc16,
        Rc32,
        
        Grey8,
        Grey16,       
    }

    public static class PixelFormatEx
    {
        public static int GetNumBites(this PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.Rgba16: return 2;
                case PixelFormat.Rgba32: return 4;
                case PixelFormat.Rgba64: return 8;

                case PixelFormat.Rgb15: return 2;
                case PixelFormat.Rgb24: return 3;
                case PixelFormat.Rgb48: return 6;

                case PixelFormat.Rc16: return 2;
                case PixelFormat.Rc32: return 4;

                case PixelFormat.Grey8: return 1;
                case PixelFormat.Grey16: return 2;
            }

            //we are unable to determin the pixel format
            throw new NotSupportedException();
        }

        public static int GetNumChanels(this PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.Rgba16: return 4;
                case PixelFormat.Rgba32: return 4;
                case PixelFormat.Rgba64: return 4;

                case PixelFormat.Rgb15: return 3;
                case PixelFormat.Rgb24: return 3;
                case PixelFormat.Rgb48: return 3;

                case PixelFormat.Rc16: return 2;
                case PixelFormat.Rc32: return 2;

                case PixelFormat.Grey8: return 1;
                case PixelFormat.Grey16: return 1;
            }

            //we are unable to determin the pixel format
            throw new NotSupportedException();
        }
    }

    public class PixelFormat2
    {
        #region Class Definitions...

        private int bpp;
        private int chan;

        private DelDecodeColor dec;
        private DelEncodeColor enc;

        private PixelFormat2(int bpp, int chan, DelDecodeColor dec, DelEncodeColor enc)
        {
            this.bpp = bpp;
            this.chan = chan;
            this.dec = dec;
            this.enc = enc;
        }

        public int BitLength
        {
            get { return bpp; }
        }

        public int NumChanels
        {
            get { return chan; }
        }

        public Color DecodeColor(byte[] data, int index)
        {
            return dec.Invoke(data, index);
        }

        public void EncodeColor(byte[] data, int index, Color c)
        {
            enc.Invoke(data, index, c);
        }

        private static int Descritise(double value, double max)
        {
            return (int)Math.Floor((value * max) + 0.5);
        }

        #endregion ///////////////////////////////////////////////////////////////////////

        #region Common Pixel Formats...


        public static PixelFormat2 Rgba16
        {
            get
            {
                //defines the decoder
                DelDecodeColor dec = delegate(byte[] data, int index)
                {
                    ushort pix = BitConverter.ToUInt16(data, index);

                    double r = ((pix & 0xF000) >> 12) / 15.0;
                    double g = ((pix & 0x0F00) >> 8) / 15.0;
                    double b = ((pix & 0x00F0) >> 4) / 15.0;
                    double a = ((pix & 0x000F) >> 0) / 15.0;

                    return Color.FromRGB(r, g, b, a);
                };

                //defines the encoder
                DelEncodeColor enc = delegate(byte[] data, int index, Color c)
                {
                    int rint = Descritise(c.Red, 15.0);
                    int gint = Descritise(c.Green, 15.0);
                    int bint = Descritise(c.Blue, 15.0);
                    int aint = Descritise(c.Alpha, 15.0);

                    int pix = (rint << 12) + (gint << 8) + (bint << 4) + aint;
                    byte[] bits = BitConverter.GetBytes((ushort)pix);

                    data[index + 0] = bits[0];
                    data[index + 1] = bits[1];
                };

                return new PixelFormat2(16, 4, dec, enc);
            }
        }

        public static PixelFormat2 Rgba32
        {
            get
            {
                //defines the decoder
                DelDecodeColor dec = delegate(byte[] data, int index)
                {
                    double r = data[index + 0] / 255.0;
                    double g = data[index + 1] / 255.0;
                    double b = data[index + 2] / 255.0;
                    double a = data[index + 3] / 255.0;

                    return Color.FromRGB(r, g, b, a);
                };

                //defines the encoder
                DelEncodeColor enc = delegate(byte[] data, int index, Color c)
                {
                    data[index + 0] = (byte)Descritise(c.Red, 255.0);
                    data[index + 1] = (byte)Descritise(c.Green, 255.0);
                    data[index + 2] = (byte)Descritise(c.Blue, 255.0);
                    data[index + 3] = (byte)Descritise(c.Alpha, 255.0);
                };

                return new PixelFormat2(32, 4, dec, enc);
            }
        }

        public static PixelFormat2 Rgba64
        {
            get
            {
                //defines the decoder
                DelDecodeColor dec = delegate(byte[] data, int index)
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
                };

                //defines the encoder
                DelEncodeColor enc = delegate(byte[] data, int index, Color c)
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
                };

                return new PixelFormat2(64, 4, dec, enc);
            }
        }

        public static PixelFormat2 Rgb16
        {
            get
            {
                //defines the decoder
                DelDecodeColor dec = delegate(byte[] data, int index)
                {
                    ushort pix = BitConverter.ToUInt16(data, index);

                    uint rint = (pix & 0x7C00u) >> 10;
                    uint gint = (pix & 0x03E0u) >> 5;
                    uint bint = (pix & 0x001Fu) >> 0;

                    double r = rint / 31.0;
                    double g = gint / 31.0;
                    double b = bint / 31.0;

                    return Color.FromRGB(r, g, b);
                };

                //defines the encoder
                DelEncodeColor enc = delegate(byte[] data, int index, Color c)
                {
                    int rint = Descritise(c.Red, 31.0);
                    int gint = Descritise(c.Green, 31.0);
                    int bint = Descritise(c.Blue, 31.0);

                    int pix = (rint << 10) + (gint << 5) + (bint << 0);
                    byte[] bits = BitConverter.GetBytes((ushort)pix);

                    data[index + 0] = bits[0];
                    data[index + 1] = bits[1];
                };

                return new PixelFormat2(16, 3, dec, enc);
            }
        }

        public static PixelFormat2 Rgb24
        {
            get
            {
                //defines the decoder
                DelDecodeColor dec = delegate(byte[] data, int index)
                {
                    double r = data[index + 0] / 255.0;
                    double g = data[index + 1] / 255.0;
                    double b = data[index + 2] / 255.0;

                    return Color.FromRGB(r, g, b);
                };

                //defines the encoder
                DelEncodeColor enc = delegate(byte[] data, int index, Color c)
                {
                    data[index + 0] = (byte)Descritise(c.Red, 255.0);
                    data[index + 1] = (byte)Descritise(c.Green, 255.0);
                    data[index + 2] = (byte)Descritise(c.Blue, 255.0);
                };

                return new PixelFormat2(24, 3, dec, enc);
            }
        }

        public static PixelFormat2 Rgb48
        {
            get
            {
                //defines the decoder
                DelDecodeColor dec = delegate(byte[] data, int index)
                {
                    int rint = BitConverter.ToUInt16(data, index);
                    int gint = BitConverter.ToUInt16(data, index + 2);
                    int bint = BitConverter.ToUInt16(data, index + 4);

                    double r = rint / 65535.0;
                    double g = gint / 65535.0;
                    double b = bint / 65535.0;

                    return Color.FromRGB(r, g, b);
                };

                //defines the encoder
                DelEncodeColor enc = delegate(byte[] data, int index, Color c)
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
                };

                return new PixelFormat2(48, 3, dec, enc);
            }
        }

        public static PixelFormat2 Rc16
        {
            get
            {
                //defines the decoder
                DelDecodeColor dec = delegate(byte[] data, int index)
                {
                    double r = data[index + 0] / 255.0;
                    double c = data[index + 1] / 255.0;

                    return Color.FromRGB(r, c, c);
                };

                //defines the encoder
                DelEncodeColor enc = delegate(byte[] data, int index, Color c)
                {
                    double mix = c.Green * 0.83738 + c.Blue * 0.16262;

                    data[index + 0] = (byte)Descritise(c.Red, 255.0);
                    data[index + 1] = (byte)Descritise(mix, 255.0);
                };

                return new PixelFormat2(16, 2, dec, enc);
            }
        }

        public static PixelFormat2 Rc32
        {
            get
            {
                //defines the decoder
                DelDecodeColor dec = delegate(byte[] data, int index)
                {
                    int rint = BitConverter.ToUInt16(data, index);
                    int cint = BitConverter.ToUInt16(data, index + 2);

                    double r = rint / 65535.0;
                    double c = cint / 65535.0;

                    return Color.FromRGB(r, c, c);
                };

                //defines the encoder
                DelEncodeColor enc = delegate(byte[] data, int index, Color c)
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
                };

                return new PixelFormat2(32, 2, dec, enc);
            }
        }

        public static PixelFormat2 Grey8
        {
            get
            {
                //defines the decoder
                DelDecodeColor dec = delegate(byte[] data, int index)
                {
                    double val = data[index] / 256.0;
                    return Color.FromRGB(val, val, val);
                };

                //defines the encoder
                DelEncodeColor enc = delegate(byte[] data, int index, Color c)
                {
                    data[index] = (byte)Descritise(c.Luminance, 255.0);
                };

                return new PixelFormat2(8, 1, dec, enc);
            }
        }

        public static PixelFormat2 Grey16
        {
            get
            {
                //defines the decoder
                DelDecodeColor dec = delegate(byte[] data, int index)
                {
                    int temp = BitConverter.ToUInt16(data, index);
                    double val = temp / 65535.0;
                    return Color.FromRGB(val, val, val);
                };

                //defines the encoder
                DelEncodeColor enc = delegate(byte[] data, int index, Color c)
                {
                    ushort temp = (ushort)Descritise(c.Luminance, 65535.0);
                    byte[] bits = BitConverter.GetBytes(temp);

                    data[index + 0] = bits[0];
                    data[index + 1] = bits[1];
                };

                return new PixelFormat2(16, 1, dec, enc);
            }
        }


        #endregion ///////////////////////////////////////////////////////////////////////

    }


    internal delegate Color DelDecodeColor(byte[] data, int index);

    internal delegate void DelEncodeColor(byte[] data, int index, Color c);
}
