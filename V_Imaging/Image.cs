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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Data.Exceptions;
using Vulpine.Core.Draw.Colors;
using Vulpine.Core.Draw.Images;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Matrices;

namespace Vulpine.Core.Draw
{
    /// <summary>
    /// An image is conceptualy a two dimentionl grid (also known as a raster) of colors.
    /// The individual cells of the grid are called pixels. The colors are treated as belonging
    /// to a continuious space, although the internal implementation may be diffreent. It
    /// may be overloaded in other assemblys to refer to images in local memory, or on the GPU, 
    /// or it may refer to the output display itself. By providing this abstraciton, we can
    /// maniulate images iregardless of where or how they are stored.
    /// </summary>
    /// <remarks>Last Update: 2015-10-08</remarks>
    public abstract class Image : IEnumerable<Pixel>
    {
        #region Class Definitions...

        //Stores the constents used in Floyd-Steinberg dithering
        private const double FS_0 = 7.0 / 16.0;
        private const double FS_1 = 3.0 / 16.0;
        private const double FS_2 = 5.0 / 16.0;
        private const double FS_3 = 1.0 / 16.0;

        //Stores the Bayer Matrix used for dithering
        private static readonly byte[] bayer =
        {
            00, 48, 12, 60, 03, 51, 15, 63,
            32, 16, 44, 28, 35, 19, 47, 31,
            08, 56, 04, 52, 11, 59, 07, 55,
            40, 24, 36, 20, 43, 27, 39, 23,
            02, 50, 14, 62, 01, 49, 13, 61,
            34, 18, 46, 30, 33, 17, 45, 29,
            10, 58, 06, 54, 09, 57, 05, 53,
            42, 26, 38, 22, 41, 25, 37, 21,
        };

        //determins how to extend the image
        private bool tile_x;
        private bool tile_y;

        public Image()
        {
            //uses the default values
            tile_x = false;
            tile_y = false;
        }

        public Image(ImageExt ext)
        {
            //calls the set method
            SetTileability(ext);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// The width of the current image in pixels.
        /// </summary>
        public abstract int Width { get; }

        /// <summary>
        /// The height of the current image in pixels.
        /// </summary>
        public abstract int Height { get; }

        /// <summary>
        /// The total number of pixels in the image.
        /// </summary>
        public virtual int Size
        {
            get { return Width * Height; }
        }

        /// <summary>
        /// Returns true if one can only read the image, and false 
        /// if one can both read and wright to the image.
        /// </summary>
        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Determins if the image is tilable or not, along either the x or y
        /// axis, or both, or if the image should be mirrored instead.
        /// </summary>
        public ImageExt Tileability
        {
            get
            {
                if (tile_x)
                {
                    if (tile_y) return ImageExt.TileXY;
                    else return ImageExt.TileX_MirrorY;
                }
                else
                {
                    if (tile_y) return ImageExt.MirrorX_TileY;
                    else return ImageExt.MirrorXY;
                }
            }
        }

        /// <summary>
        /// Accesses the pixels of the image by column and row. See the GetPixel()
        /// and SetPixel() methods for more details.
        /// </summary>
        /// <param name="col">Column from the left</param>
        /// <param name="row">Row from the top</param>
        /// <returns>The color of the desired pixel</returns>
        public Color this[int col, int row]
        {
            get { return GetPixel(col, row); }
            set { SetPixel(col, row, value); }
        }       

        #endregion //////////////////////////////////////////////////////////////

        #region Pixel Axcess...

        /// <summary>
        /// Selects a given pixel in the current image. It allows indexing
        /// outside the given bounds by tiling the image.  
        /// </summary>
        /// <param name="col">Column from the left</param>
        /// <param name="row">Row from the top</param>
        /// <returns>The color of the desired pixel</returns>
        public Color GetPixel(int col, int row)
        {
            //obtains the widht and height
            int dw = Width;
            int dh = Height;

            ////computes the true modulous of the width and height
            //int dx = ((col % dw) + dw) % dw;
            //int dy = ((row % dh) + dh) % dh;

            //recalculates the index based on the images tileability
            int dx = Recal(col, dw, tile_x);
            int dy = Recal(row, dh, tile_y);

            //queries the abstract method
            return GetPixelInit(dx, dy);
        }

        /// <summary>
        /// Updates a given pixel in the current image. It allows indexing
        /// outside the given bounds by tiling the image. 
        /// </summary>
        /// <param name="col">Column from the left</param>
        /// <param name="row">Row from the top</param>
        /// <param name="color">New color of the pixel</param>
        /// <exception cref="InvalidOperationException">If the current
        /// image is marked as read-only</exception>
        public void SetPixel(int col, int row, Color color)
        {
            //checks that the image is wrightable
            if (this.IsReadOnly) throw new InvalidOperationException();

            //obtains the widht and height
            int dw = Width;
            int dh = Height;

            //computes the true modulous of the width and height
            int dx = ((col % dw) + dw) % dw;
            int dy = ((row % dh) + dh) % dh;

            //queries the abstract method
            SetPixelInit(dx, dy, color);
        }

        /// <summary>
        /// Reads all of the image data as a continious stream of pixels.
        /// Normaly image data is read from right to left and from top to bottom.
        /// </summary>
        /// <returns>An enumeration of the pixel data</returns>
        public IEnumerator<Pixel> GetEnumerator()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Color c = GetPixelInit(x, y);
                    yield return new Pixel(x, y, c);
                }
            }
        }

        /// <summary>
        /// Clears the image of all color data, setting every pixel in the 
        /// image to the given background color. 
        /// </summary>
        /// <param name="bg">Background Color</param>
        /// <exception cref="InvalidOperationException">If the current
        /// image is marked as read-only</exception>
        public void Clear(Color bg)
        {
            //checks that the image is wrightable
            if (this.IsReadOnly) throw new InvalidOperationException();

            //sets each pixel to the background color
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                    this.SetPixelInit(i, j, bg);
            }
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Abstract Methods...

        /// <summary>
        /// Provides access to the internal pixel data. This method should
        /// only ever be called with bounded indicies.
        /// </summary>
        /// <param name="col">Column from the left</param>
        /// <param name="row">Row from the top</param>
        /// <returns>The color of the desired pixel</returns>
        protected abstract Color GetPixelInit(int col, int row);

        /// <summary>
        /// Provides access to the internal pixel data. This method should
        /// only ever be called with bounded indicies.
        /// </summary>
        /// <param name="col">Column from the left</param>
        /// <param name="row">Row from the top</param>
        /// <param name="color">New color of the pixel</param>
        protected abstract void SetPixelInit(int col, int row, Color color);

        /// <summary>
        /// Returns a refrence to the object responcable for the image's internal
        /// state, or to itself it the image manages it's own state. This is nessary 
        /// to take full advantage of graphics acceleration.
        /// </summary>
        /// <returns>A refrence to the image's internal state</returns>
        public virtual Object GetInternalData()
        {
            //by default, we manage our own state
            return this;
        }     

        #endregion //////////////////////////////////////////////////////////////

        #region Advanced Mehtods...

        /// <summary>
        /// Fills the image with pixel data taken from a stream. Any pixels
        /// that fall outside the bounds of the image are discarded.
        /// </summary>
        /// <param name="data">Image data stream</param>
        /// <exception cref="InvalidOperationException">If the current
        /// image is marked as read-only</exception>
        public virtual void FillData(IEnumerable<Pixel> data)
        {
            //checks that the image is wrightable
            if (this.IsReadOnly) throw new InvalidOperationException();

            foreach (Pixel p in data)
            {
                //obtains the row and column
                int col = p.Col;
                int row = p.Row;

                //adds the data if it fits within our image
                if (col < Width && row < Height)
                {
                    Color c = p.Color;
                    SetPixelInit(col, row, c); 
                }
            }
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
        public virtual void FillData(Image data)
        {
            //checks that the image is wrightable
            if (this.IsReadOnly) throw new InvalidOperationException();

            //computes the intersection of both images
            int w = Math.Min(this.Width, data.Width);
            int h = Math.Min(this.Height, data.Height);

            //fills the image with new data
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    Color c = data.GetPixelInit(i, j);
                    this.SetPixelInit(i, j, c);
                }
            }
        }

        public void FillDither(Image data, double amount)
        {
            //checks that the image is wrightable
            if (this.IsReadOnly) throw new InvalidOperationException();

            //computes the intersection of both images
            int w = Math.Min(this.Width, data.Width);
            int h = Math.Min(this.Height, data.Height);

            //fills the image with new data
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    //obtains the offset from the bayer matrix
                    int index = (j % 8) + (i % 8) * 8;
                    double offset = (double)bayer[index];
                    offset = (offset / 64.0) - 0.5;

                    //offset = offset * (1.0 / 1.0); //0.125;
                    offset = offset * VMath.Clamp(amount);

                    //extracts the color with the given offset
                    Color c = data.GetPixelInit(j, i);
                    double r = c.Red + offset;
                    double g = c.Green + offset;
                    double b = c.Blue + offset;

                    //applies the offset color to the image
                    Color cn = Color.FromRGB(r, g, b);
                    this.SetPixelInit(j, i, cn);
                }
            }
        }

        public void FillDitherFS(Image data)
        {
            //checks that the image is wrightable
            if (this.IsReadOnly) throw new InvalidOperationException();

            //computes the intersection of both images
            int w = Math.Min(this.Width, data.Width);
            int h = Math.Min(this.Height, data.Height);

            //uses an array to store the cumulative errors
            double[] r0_error = new double[w];
            double[] g0_error = new double[w];
            double[] b0_error = new double[w];

            double[] r1_error = new double[w];
            double[] g1_error = new double[w];
            double[] b1_error = new double[w];

            double r, g, b;

            //initialises the error values to zero
            for (int i = 0; i < w; i++)
            {
                r0_error[i] = 0.0;
                g0_error[i] = 0.0;
                b0_error[i] = 0.0;

                r1_error[i] = 0.0;
                g1_error[i] = 0.0;
                b1_error[i] = 0.0;
            }

            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    //extracts the color information from the source
                    Color c = data.GetPixelInit(j, i);

                    //shifts the color by the computed error
                    r = c.Red + r0_error[j];
                    g = c.Green + g0_error[j];
                    b = c.Blue + b0_error[j];

                    //sets the corrected color into the image
                    Color cp = Color.FromRGB(r, g, b);
                    this.SetPixelInit(j, i, cp);

                    //obtains the relitive error between the pixels
                    cp = this.GetPixelInit(j, i);

                    r = c.Red - cp.Red;
                    g = c.Green - cp.Green;
                    b = c.Blue - cp.Blue;

                    if (j > 0)
                    {
                        r1_error[j - 1] += r * FS_1;
                        g1_error[j - 1] += g * FS_1;
                        b1_error[j - 1] += b * FS_1;
                    }

                    if (j < w - 1)
                    {
                        r0_error[j + 1] += r * FS_0;
                        g0_error[j + 1] += g * FS_0;
                        b0_error[j + 1] += b * FS_0;

                        r1_error[j + 1] += r * FS_3;
                        g1_error[j + 1] += g * FS_3;
                        b1_error[j + 1] += b * FS_3;
                    }

                    r1_error[j] += r * FS_2;
                    g1_error[j] += g * FS_2;
                    b1_error[j] += b * FS_2;
                    
                }

                for (int j = 0; j < w; j++)
                {
                    //coppies one row up
                    r0_error[j] = r1_error[j];
                    g0_error[j] = g1_error[j];
                    b0_error[j] = b1_error[j];

                    //clears the next row
                    r1_error[j] = 0.0;
                    g1_error[j] = 0.0;
                    b1_error[j] = 0.0;
                }
            }
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Helper Methods...

        /// <summary>
        /// Helper method, used to recalculate the pixel index when trying
        /// to access data outside the bounds of the image.
        /// </summary>
        /// <param name="x">Value to recalculate</param>
        /// <param name="max">Maximum value the value can take</param>
        /// <param name="tiled">True if tieling should be used</param>
        /// <returns>The recaluclated value</returns>
        private static int Recal(int x, int max, bool tiled)
        {
            int dw = tiled ? max : max + max;
            int dx = ((x % dw) + dw) % dw;
            if (dx >= max) dx = dw - dx - 1;

            return dx;
        }

        /// <summary>
        /// Sets the tileability of the current image. That is whether or not
        /// the image can be tiled allong the x axis, or the y axis, or both.
        /// Idealy this should be called within the class constructor.
        /// </summary>
        /// <param name="ext">Tileability of the image</param>
        internal void SetTileability(ImageExt ext)
        {
            switch (ext)
            {
                case ImageExt.TileXY:
                    tile_x = true;
                    tile_y = true;
                    return;
                case ImageExt.TileX_MirrorY:
                    tile_x = true;
                    tile_y = false;
                    return;
                case ImageExt.MirrorX_TileY:
                    tile_x = false;
                    tile_y = true;
                    return;
                default:
                    tile_x = false;
                    tile_y = false;
                    return;
            }
        }

        /// <summary>
        /// Sets the tileablity to match that of another source image.
        /// This is usefull for the implementaiton of copy constructors.
        /// </summary>
        /// <param name="other">Source image</param>
        internal void SetTileability(Image other)
        {
            this.tile_x = other.tile_x;
            this.tile_y = other.tile_y;
        }

        #endregion //////////////////////////////////////////////////////////////

        IEnumerator IEnumerable.GetEnumerator()
        { return GetEnumerator(); }
    }

}
