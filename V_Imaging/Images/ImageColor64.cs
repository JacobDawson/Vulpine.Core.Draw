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
    /// This subclass stores a full color image utilising 64 bits per pixel. It uses the
    /// standard RGBA colorspace with 16 bits per chanel, as opposed to the 8 bits per
    /// channel used for most image sources. This leasds to very large but very precice
    /// image buffers in memory, useful for intermediat calculaitons, or for when greator
    /// precission is required.
    /// </summary>
    /// <remarks>Last Update: 2017-10-22</remarks>
    public class ImageColor64 : Image
    {
        //the maximum value of each channel as a double
        private const double MAX = 65535.0;

        //remembers the width and height of the image
        private int width;
        private int height;

        //stores each channel in a separate array
        private ushort[] red;
        private ushort[] green;
        private ushort[] blue;
        private ushort[] alpha;

        /// <summary>
        /// Creates a new image with the given width and height.
        /// </summary>
        /// <param name="width">Widht of the image in pixels</param>
        /// <param name="height">Height of the image in pixels</param>
        public ImageColor64(int width, int height)
        {
            this.width = width;
            this.height = height;

            int total = width * height;

            red = new ushort[total];
            green = new ushort[total];
            blue = new ushort[total];
            alpha = new ushort[total];
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

            //converts the components to the range [0..1]
            double r = red[index] / MAX;
            double g = green[index] / MAX;
            double b = blue[index] / MAX;
            double a = alpha[index] / MAX;

            //returns the corisponding color
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

            //descretises the components to be 16 bits long
            red[index] = Desc(color.Red);
            green[index] = Desc(color.Green);
            blue[index] = Desc(color.Blue);
            alpha[index] = Desc(color.Alpha);
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Helper Mehtods...

        /// <summary>
        /// Converts a continious, floating-point value between zero and one to
        /// a descrete, interger value between zero and the max value.
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns>A descrete version of the continious variable</returns>
        private ushort Desc(double value)
        {
            return (ushort)Math.Floor((value * MAX) + 0.5);
        }

        #endregion //////////////////////////////////////////////////////////////

    }
}
