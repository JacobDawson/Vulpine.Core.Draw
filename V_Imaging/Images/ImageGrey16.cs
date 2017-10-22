/**
 *  This file is an integral part of the Vulpine Core Library: 
 *  Copyright (c) 2016-2017 Benjamin Jacob Dawson. 
 *
 *      http://www.jakesden.com/corelibrary.html
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

namespace Vulpine.Core.Draw.Images
{
    /// <summary>
    /// This subclass stores a greyscale image utilising 16 bits per pixel. This allows 
    /// it to contain a total of 65,536 shades of grey, which is mutch more precice than
    /// most typical monotone images. This is useful when you need precice values for
    /// each of the pixels, but do not care about the color components.
    /// </summary>
    /// <remarks>Last Update: 2017-10-22</remarks>
    public class ImageGrey16 : Image
    {
        //weights that can be used for caluclating luma
        private const double WR = 19595.0;
        private const double WG = 38469.0;
        private const double WB =  7471.0;

        //remembers the width and height of the image
        private int width;
        private int height;

        //stores each channel in a separate array
        private ushort[] value;

        /// <summary>
        /// Creates a new image with the given width and height.
        /// </summary>
        /// <param name="width">Widht of the image in pixels</param>
        /// <param name="height">Height of the image in pixels</param>
        public ImageGrey16(int width, int height)
        {
            this.width = width;
            this.height = height;

            int total = width * height;
            this.value = new ushort[total];
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

            //obtains the value and converts it to double
            double v = (double)value[index] / 65535.0;

            //returns the corisponding color
            return new Color(v, v, v);
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

            //extracts the three color channels
            double r = color.Red;
            double g = color.Green;
            double b = color.Blue;

            //computes the weighted luiminance
            double lum = (r * WR) + (g * WG) + (b * WB);
            value[index] = (ushort)Math.Floor(lum + 0.5);
        }

        #endregion //////////////////////////////////////////////////////////////

    }
}
