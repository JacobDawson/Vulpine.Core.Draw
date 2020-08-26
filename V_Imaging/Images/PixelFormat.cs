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
    /// <summary>
    /// This class represents a pixel format for use in basic images which store their
    /// color data as an array of bite values. It contains methods for both encoding
    /// and decoding colors from an array of bite values, as well as information on
    /// the format itself, sutch as the number of bits per pixel.
    /// </summary>
    /// <remarks>Last Update: 2019-04-04</remarks>
    public struct PixelFormat
    {
        #region Class Definitions...

        //TODO: refactor hard coded values into constants
        //TODO: consider making the index values long

        //recordes metrics on the pixel format
        private int bpp;
        private int chan;

        //stores methods that can handel the encoding and decoding
        private DelDecodeColor dec;
        private DelEncodeColor enc;

        /// <summary>
        /// Internal constructor for creating instances of pixel format.
        /// </summary>
        /// <param name="bpp">Number of bits per pixel</param>
        /// <param name="chan">Number of chanels suported</param>
        /// <param name="dec">Delegate for the decoding procedure</param>
        /// <param name="enc">Delegate for the encoding procedure</param>
        internal PixelFormat(int bpp, int chan, DelDecodeColor dec, DelEncodeColor enc)
        {
            this.bpp = bpp;
            this.chan = chan;
            this.dec = dec;
            this.enc = enc;
        }

        /// <summary>
        /// Determins the number of bits used per pixel in this format. Note that
        /// there are eight bits in a byte, so this should always be a multiple
        /// of eight.
        /// </summary>
        public int BitDepth
        {
            get { return bpp; }
        }

        /// <summary>
        /// Determins the number of chanels this format suports. Greyscale images
        /// need only a single channel, while full color images require three. If
        /// transparancy informaiton is required, an extra chanel must be added.
        /// </summary>
        public int NumChanels
        {
            get { return chan; }
        }

        /// <summary>
        /// Decodes a color value from an array of bytes, assuming the curent format.
        /// </summary>
        /// <param name="data">Data array from which to decode</param>
        /// <param name="index">Starting index of the desired color vlaue</param>
        /// <returns>The decoded color vlaue</returns>
        public Color DecodeColor(byte[] data, int index)
        {
            return dec.Invoke(data, index);
        }

        /// <summary>
        /// Encodes a color value and writes it to an array of bytes, 
        /// using the curent format.
        /// </summary>
        /// <param name="data">Data array to contain the encoded values</param>
        /// <param name="index">Starting index to encode the values</param>
        /// <param name="c">Color value to encode</param>
        public void EncodeColor(byte[] data, int index, Color c)
        {
            enc.Invoke(data, index, c);
        }

        /// <summary>
        /// Helper method for converting a continious floating point value, to 
        /// a descrete range of interger values.
        /// </summary>
        /// <param name="value">Continious value to convert</param>
        /// <param name="max">Maximum output value</param>
        /// <returns>The descritised value</returns>
        private static int Descritise(double value, double max)
        {
            return (int)Math.Floor((value * max) + 0.5);
        }

        #endregion ///////////////////////////////////////////////////////////////////////

        #region Common Pixel Formats...

        /// <summary>
        /// Uses 16 bits per pixel to encode RGBA values, using 4 bits for each of
        /// the red, green, and blue channels, plus 4 extra bits for the alpha channel.
        /// Thus this can be thought of as 12-bit RGB plus Alpha.
        /// </summary>
        public static PixelFormat Rgba16
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

                    return Color.FromRGBA(r, g, b, a);
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

                return new PixelFormat(16, 4, dec, enc);
            }
        }

        /// <summary>
        /// Uses 32 bits per pixel to encode RGBA values, using 8 bits for each of
        /// the Red, Green, Blue, and Alpha channels. This is the default format
        /// for images that include transpanancy, as it is close to the limit of 
        /// what human beings are able to pecieve. It can be thought of as Truecolor 
        /// plus Alpha.
        /// </summary>
        public static PixelFormat Rgba32
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

                    return Color.FromRGBA(r, g, b, a);
                };

                //defines the encoder
                DelEncodeColor enc = delegate(byte[] data, int index, Color c)
                {
                    data[index + 0] = (byte)Descritise(c.Red, 255.0);
                    data[index + 1] = (byte)Descritise(c.Green, 255.0);
                    data[index + 2] = (byte)Descritise(c.Blue, 255.0);
                    data[index + 3] = (byte)Descritise(c.Alpha, 255.0);
                };

                return new PixelFormat(32, 4, dec, enc);
            }
        }

        /// <summary>
        /// Uses 64 bits per pixel to encode RGBA values, using 16 bits for each
        /// of the Red, Green, Blue, and Alpha channels. This is mostly useful for
        /// intermediate storage to avoid round-off errors, as most human beings
        /// are incabable of ditinguishing such minute diffrences. 
        /// </summary>
        public static PixelFormat Rgba64
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

                    return Color.FromRGBA(r, g, b, a);
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

                return new PixelFormat(64, 4, dec, enc);
            }
        }

        /// <summary>
        /// Uses 16 bits per pixel to store RGB values, using 5 bits for each of
        /// the Red, Green, and Blue channels. The last bit is not used. This is
        /// commonly refered to as Highcolor, and can be useful where less precision
        /// than Truecolor is required.
        /// </summary>
        public static PixelFormat Rgb16
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

                return new PixelFormat(16, 3, dec, enc);
            }
        }

        /// <summary>
        /// Uses 24 bits per pixel to store RGB values, using 8 bits for each of
        /// the Red, Green, and Blue channels. This is the default format for images
        /// that do not include transparancy, as it is close to the limit of what
        /// human beings are able to pecieve. It is refered to as Truecolor.
        /// </summary>
        public static PixelFormat Rgb24
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

                return new PixelFormat(24, 3, dec, enc);
            }
        }

        /// <summary>
        /// Uses 48 bits per pixel to store RGB values, using 16 bits for each of
        /// the Red, Green, and Blue channels. This is mostly useful for intermediate 
        /// storage to avoid round-off errors, as most human beings are incabable 
        /// of ditinguishing such minute diffrences. 
        /// </summary>
        public static PixelFormat Rgb48
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

                return new PixelFormat(48, 3, dec, enc);
            }
        }

        /// <summary>
        /// This uses 16 bits per pixel to encode both a Red and a Cyan chanel, using
        /// 8 bits each. This is useful when only two chanels of informaiton need to
        /// be stored, such as with Anaglyphic images, for example.
        /// </summary>
        public static PixelFormat Rc16
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

                return new PixelFormat(16, 2, dec, enc);
            }
        }

        /// <summary>
        /// This uses 32 bits per pixel to encode both a Red and a Cyan chanel, using
        /// 16 bits each. This is useful when only two chanels of informaiton need to
        /// be stored, such as with Anaglyphic images, for example.
        /// </summary>
        public static PixelFormat Rc32
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

                return new PixelFormat(32, 2, dec, enc);
            }
        }

        /// <summary>
        /// Uses 8 bits per pixel to store a single greyscale value. This is most useful
        /// when one is working with greyscale or monotone images, where one only cares
        /// about the value of each pixel, and not the color.
        /// </summary>
        public static PixelFormat Grey8
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

                return new PixelFormat(8, 1, dec, enc);
            }
        }

        /// <summary>
        /// Uses 16 bits per pixel to store a single greyscale value. This is most useful
        /// when one is working with greyscale or monotone images, where one only cares
        /// about the value of each pixel, and not the color.
        /// </summary>
        public static PixelFormat Grey16
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

                return new PixelFormat(16, 1, dec, enc);
            }
        }

        #endregion ///////////////////////////////////////////////////////////////////////

    }

    /// <summary>
    /// A delegate for provideing decoder methods for pixel formats.
    /// </summary>
    /// <param name="data">Data array from which to decode</param>
    /// <param name="index">Starting index of the desired color vlaue</param>
    /// <returns>The decoded color vlaue</returns>
    internal delegate Color DelDecodeColor(byte[] data, int index);

    /// <summary>
    /// A delegate for providing encoder methods for pixel formats.
    /// </summary>
    /// <param name="data">Data array to contain the encoded values</param>
    /// <param name="index">Starting index to encode the values</param>
    /// <param name="c">Color value to encode</param>
    internal delegate void DelEncodeColor(byte[] data, int index, Color c);
}
