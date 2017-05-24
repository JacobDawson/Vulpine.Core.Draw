/**
 *  This file is an integral part of the Vulpine Core Library: 
 *  Copyright (c) 2016-2017 Benjamin Jacob Dawson. 
 *
 *      https://www.jacobs-den.org/projects/core-library/
 *
 *  This file is licensed under the Apache License, Version 2.0 (the "License"); 
 *  you may not use this file except in compliance with the License. You may 
 *  obtain a copy of the License at:
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Data.Exceptions;
using Vulpine.Core.Draw.Colors;

namespace Vulpine.Core.Draw
{
    /// <summary>
    /// An image is conceptualy a two dimentionl grid (also known as a raster) of colors.
    /// The individual cells of the grid are called pixels. The colors are treated as belonging
    /// to a continuious space, although the internal implementation may be diffreent. It
    /// may be overloaded in other assemblys to refer to images in local memory, or on the GPU, 
    /// or it may refer to the output display itself. By providing this abstraciton, we can
    /// maniulate images iregardless of where or how they are stored.
    /// <remarks>Last Update: 2015-10-08</remarks>
    /// </summary>
    public abstract class Image
    {
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
        /// The total size of the image in pixles.
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
        /// Selects a given pixel in the current image.
        /// </summary>
        /// <param name="col">Column from the left</param>
        /// <param name="row">Row from the top</param>
        /// <exception cref="ArgRangeExcp">If the column
        /// or row falls outside the bounds of the image</exception>
        /// <returns>The color of the desired pixel</returns>
        public Color GetPixel(int col, int row)
        {
            //checks for a valid column and row
            ArgRangeExcp.Check("col", col, Width - 1);
            ArgRangeExcp.Check("row", row, Height - 1);

            //queries the abstract method
            return GetPixelInternal(col, row);
        }

        /// <summary>
        /// Updates a given pixel in the current image.
        /// </summary>
        /// <param name="col">Column from the left</param>
        /// <param name="row">Row from the top</param>
        /// <param name="color">New color of the pixel</param>
        /// <exception cref="ArgRangeExcp">If the column
        /// or row falls outside the bounds of the image</exception>
        /// <exception cref="ReadOnlyExcp">If the current
        /// image is marked as read-only</exception>
        public void SetPixel(int col, int row, Color color)
        {
            //checks that the image is wrightable
            if (this.IsReadOnly) throw new InvalidOperationException();

            //checks for a valid column and row
            ArgRangeExcp.Check("col", col, Width - 1);
            ArgRangeExcp.Check("row", row, Height - 1);

            //queries the abstract method
            SetPixelInternal(col, row, color);
        }

        /// <summary>
        /// Allows access to pixels outside the bounds of the image by mirroring
        /// the image across the X and Y axies. This is important for interpolation
        /// and filtering, and generlay offers the best results.
        /// </summary>
        /// <param name="col">Column from the left</param>
        /// <param name="row">Row from the top</param>
        /// <returns>The color of the desired pixel</returns>
        public Color GetExtended(int col, int row)
        {
            //mirrors across the X axis
            int dw = Width + Width;
            int dx = ((col % dw) + dw) % dw;
            if (dx >= Width) dx = dw - dx - 1;

            //mirrors across the Y axis
            int dh = Width + Width;
            int dy = ((row % dh) + dh) % dh;
            if (dy >= Height) dy = dh - dy - 1;

            return GetPixelInternal(dx, dy);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Abstract Methods...

        /// <summary>
        /// Provides access to the internal pixel data. It is not required
        /// to check the validity of it's arguments. 
        /// </summary>
        /// <param name="col">Column from the left</param>
        /// <param name="row">Row from the top</param>
        /// <returns>The color of the desired pixel</returns>
        protected abstract Color GetPixelInternal(int col, int row);

        /// <summary>
        /// Provides access to the internal pixel data. It is not required
        /// to check the validity of it's arguments. 
        /// </summary>
        /// <param name="col">Column from the left</param>
        /// <param name="row">Row from the top</param>
        /// <param name="color">New color of the pixel</param>
        protected abstract void SetPixelInternal(int col, int row, Color color);

        /// <summary>
        /// Returns a refrence to the object responcable for the image's internal
        /// state, or null if no sutch object exists. This is nessary to take full
        /// advantage of graphics acceleration.
        /// </summary>
        /// <returns>A refrence to the image's internal state</returns>
        public virtual Object GetInternalData()
        {
            //by default, we do not store internal data
            return null;
        }     

        #endregion //////////////////////////////////////////////////////////////

        


        private Color GetExPanorama(int x, int y)
        {
            int xmax = this.Width;
            int ymax = 2 * this.Height;

            int dx = ((x % xmax) + xmax) % xmax;
            int dy = ((y % ymax) + ymax) % ymax;

            if (dy >= this.Height)
            {
                dy = ymax - dy - 1;
                dx = (dx + (xmax / 2)) % xmax;
            }

            //queries the abstract method
            return GetPixelInternal(dx, dy);
        }

    }

}
