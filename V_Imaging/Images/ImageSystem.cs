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
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using VColor = Vulpine.Core.Draw.Color;
using SColor = System.Drawing.Color;
using SPixel = System.Drawing.Imaging.PixelFormat;

namespace Vulpine.Core.Draw.Images
{
    /// <summary>
    /// This class provides a wrapper for the System.Drawing.Bitmap class, alowing system 
    /// bitmaps to be used as Images within the Vulpine Core Lirbary. This is esential for
    /// using images with certain GDI windows compnents, and saving images to universal
    /// formats such as JPEG and PNG. Note that using GDI calls can be slower in most
    /// instances, and require thread exclusivity. For this reason this class is best
    /// used as a bridge between the Vulpine Core Library and other systems.
    /// </summary>
    /// <remarks>Last Update: 2019-03-26</remarks>
    public class ImageSystem : Vulpine.Core.Draw.Image, IDisposable
    {
        //stores the internal bitmap
        private Bitmap bmp;

        //an object used for locking the bitmap
        private object key;

        //stores the dimentions of the image
        private int width;
        private int height;

        /// <summary>
        /// Creates a new wrapper for a curently existing bitmap resorce.
        /// </summary>
        /// <param name="bmp">Curent bitmap</param>
        public ImageSystem(Bitmap bmp)
        {
            this.bmp = bmp;

            key = new object();
            width = bmp.Width;
            height = bmp.Height;
        }

        /// <summary>
        /// Creates a new instance of a system bitmap, with the given widht and
        /// height, to be used inside this wrapper class.
        /// </summary>
        /// <param name="width">Width of the bitmap in pixels</param>
        /// <param name="height">Height of the bitmap in pixels</param>
        public ImageSystem(int width, int height)
        {
            bmp = new Bitmap(width, height, SPixel.Format32bppArgb);
            key = new object();

            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Loads a given image file from disk, creates a system bitmap for
        /// that image file, and wraps it inside this wrapper class.
        /// </summary>
        /// <param name="file">Location to load</param>
        public ImageSystem(string file)
        {
            bmp = new Bitmap(file);
            key = new object();

            width = bmp.Width;
            height = bmp.Height;
        }

        #region Class Properties...

        /// <summary>
        /// The width of the current image in pixels.
        /// </summary>
        public override int Width
        {
            get { return width; }
        }

        /// <summary>
        /// The height of the current image in pixels.
        /// </summary>
        public override int Height
        {
            get { return height; }
        }

        /// <summary>
        /// An object used to lock the internal bitmap resource. Use this
        /// lock if you intend to use the bitmap outside this wraper.
        /// </summary>
        public object Key
        {
            get { return key; }
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
        protected override VColor GetPixelInit(int col, int row)
        {
            SColor sc;

            lock (key)
            {
                sc = bmp.GetPixel(col, row);
            }

            double r = sc.R / 255.0;
            double g = sc.G / 255.0;
            double b = sc.B / 255.0;
            double a = sc.A / 255.0;

            return VColor.FromRGBA(r, g, b, a);
        }

        /// <summary>
        /// Provides access to the internal pixel data. This method should
        /// only ever be called with bounded indicies.
        /// </summary>
        /// <param name="col">Column from the left</param>
        /// <param name="row">Row from the top</param>
        /// <param name="color">New color of the pixel</param>
        protected override void SetPixelInit(int col, int row, VColor vc)
        {
            byte r = (byte)Math.Floor((vc.Red * 255.0) + 0.5);
            byte g = (byte)Math.Floor((vc.Green * 255.0) + 0.5);
            byte b = (byte)Math.Floor((vc.Blue * 255.0) + 0.5);
            byte a = (byte)Math.Floor((vc.Alpha * 255.0) + 0.5);

            SColor sc = SColor.FromArgb(a, r, g, b);

            lock (key)
            {
                bmp.SetPixel(col, row, sc);
            }
        }

        /// <summary>
        /// Returns access to the internal bitmap resource. This must be
        /// cast to a system bitmap before it can be used.
        /// </summary>
        /// <returns>An object refrence to the internal bitmap</returns>
        public override object GetInternalData()
        {
            return bmp;
        }

        /// <summary>
        /// Saves the image data to an external file, whos location is 
        /// spesified by the file paramater.
        /// </summary>
        /// <param name="file">Location to save</param>
        public void Save(string file)
        {
            bmp.Save(file);
        }

        /// <summary>
        /// Saves the image data to an external file, whos location is
        /// spesified by the file paramater, and whos format is spesified
        /// by the format paramater
        /// </summary>
        /// <param name="file">Location to save</param>
        /// <param name="format">Format to use</param>
        public void Save(string file, ImageFormat format)
        {
            bmp.Save(file, format);
        }

        /// <summary>
        /// We provide a dispose method to clear up any system resorces that
        /// the internal bitmap structor may be holding.
        /// </summary>
        public void Dispose()
        {
            //disposes of the internal object
            bmp.Dispose();
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Advanced Mehtods...

        //NOTE: I still need to test if the specialised FillData methods are 
        //actualy faster than the default implementation.

        /// <summary>
        /// Fills the image with pixel data taken from a stream. Any pixels
        /// that fall outside the bounds of the image are discarded.
        /// </summary>
        /// <param name="data">Image data stream</param>
        /// <exception cref="InvalidOperationException">If the internal bitmap
        /// format is not 32-bits per pixle</exception>
        public override void FillData(IEnumerable<Pixel> data)
        {
            if (bmp.PixelFormat != SPixel.Format32bppArgb)
                throw new InvalidOperationException("Format Not Suported");

            //creates a bit array to hold our data
            int size = width * height * 4;
            byte[] bits = new byte[size];

            //copies the data into the temporary bit array
            foreach (Pixel pix in data)
            {
                int index = ((pix.Row * height) + pix.Col) * 4;
                if (index + 3 >= bits.Length) continue;

                bits[index + 0] = (byte)(pix.Color.Blue * 255.0);
                bits[index + 1] = (byte)(pix.Color.Green * 255.0);
                bits[index + 2] = (byte)(pix.Color.Red * 255.0);
                bits[index + 3] = (byte)(pix.Color.Alpha * 255.0);
            }

            //moves all the data into the bitmap in one step
            lock (key)
            {
                Rectangle rect = new Rectangle(0, 0, width, height);
                BitmapData bdata = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);

                Marshal.Copy(bits, 0, bdata.Scan0, bits.Length);
                bmp.UnlockBits(bdata);
            }

            //base.FillData(data);
        }

        /// <summary>
        /// Fills the current image with pixel data obtained from another image.
        /// Note that only the intersection between the two images is filled.
        /// Any pixels outside the current image are discarded, while any pixels
        /// outside the source image are left unchanged.
        /// </summary>
        /// <param name="data">Image with pixel data</param>
        /// <exception cref="InvalidOperationException">If the current
        /// image is marked as read-only</exception>
        public override void FillData(Image data)
        {
            if (bmp.PixelFormat != SPixel.Format32bppArgb)
                throw new InvalidOperationException("Format Not Suported");

            //creates a bit array to hold our data
            int size = width * height * 4;
            byte[] bits = new byte[size];

            //computes the intersection of both images
            int w = Math.Min(this.Width, data.Width);
            int h = Math.Min(this.Height, data.Height);

            //fills the temproary array with new data
            for (int row = 0; row < h; row++)
            {
                for (int col = 0; col < w; col++)
                {
                    Color c = data.GetPixel(col, row);
                    int index = ((row * height) + col) * 4;
                    if (index + 3 >= bits.Length) continue;

                    bits[index + 0] = (byte)(c.Blue * 255.0);
                    bits[index + 1] = (byte)(c.Green * 255.0);
                    bits[index + 2] = (byte)(c.Red * 255.0);
                    bits[index + 3] = (byte)(c.Alpha * 255.0);
                }
            }

            //moves all the data into the bitmap in one step
            lock (key)
            {
                Rectangle rect = new Rectangle(0, 0, width, height);
                BitmapData bdata = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);

                Marshal.Copy(bits, 0, bdata.Scan0, bits.Length);
                bmp.UnlockBits(bdata);
            }

            //base.FillData(data);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Class Conversions...

        /// <summary>
        /// Allows for implicit boxing of system bitmap.
        /// </summary>
        public static implicit operator ImageSystem(Bitmap bmp)
        { return new ImageSystem(bmp); }

        /// <summary>
        /// Allows for explicit unboxing of the internal bitmap.
        /// </summary>
        public static explicit operator Bitmap(ImageSystem img)
        { return img.bmp; }

        #endregion //////////////////////////////////////////////////////////////

    }
}
