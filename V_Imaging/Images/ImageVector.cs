/**
 *  This file is an integral part of the Vulpine Core Library
 *  Copyright (c) 2016-2018 Benjamin Jacob Dawson
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
    /// This subclass stores an image as a two dimentional array of floating point values.
    /// It uses a whoping 128 bits per pixel, making it the largest image format available.
    /// The values it stores can be extremly precice, especialy near the black point. It
    /// also avoids the issue of descritising the image data, by storing it in a native
    /// floating point format. It can be useful for High Dynamic Range (HDR) images, or
    /// processes that need to avoid loss of precision. 
    /// </summary>
    /// <remarks>Last Update: 2017-10-22</remarks>
    public class ImageVector : Image
    {
        //remembers the width and height of the image
        private int width;
        private int height;

        //stores each channel in a separate array
        private float[] red;
        private float[] green;
        private float[] blue;
        private float[] alpha;

        /// <summary>
        /// Creates a new image with the given width and height.
        /// </summary>
        /// <param name="width">Widht of the image in pixels</param>
        /// <param name="height">Height of the image in pixels</param>
        public ImageVector(int width, int height)
        {
            this.width = width;
            this.height = height;

            int total = width * height;

            red = new float[total];
            green = new float[total];
            blue = new float[total];
            alpha = new float[total];
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
            int index = (col * height) + row;

            //grabes the values converting them to doubles
            double r = (double)red[index];
            double g = (double)green[index];
            double b = (double)blue[index];
            double a = (double)alpha[index];

            //returns the new color
            return new Color(r, g, b, a);
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
            int index = (col * height) + row;

            //decreases the precision to 32-bit and stores them
            red[index] = (float)color.Red;
            green[index] = (float)color.Green;
            blue[index] = (float)color.Blue;
            alpha[index] = (float)color.Alpha;
        }

        #endregion //////////////////////////////////////////////////////////////

    }
}
