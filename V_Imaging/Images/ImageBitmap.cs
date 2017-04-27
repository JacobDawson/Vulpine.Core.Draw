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

using Vulpine.Core.Draw.Colors;

namespace Vulpine.Core.Draw.Images
{
    public class ImageBitmap : Image
    {
        private int width;
        private int height;

        private byte[] red;
        private byte[] green;
        private byte[] blue;

        private ImageBitmap(int width, int height)
        {
            this.width = width;
            this.height = height;

            int total = width * height;
            red = new byte[total];
            green = new byte[total];
            blue = new byte[total];
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
        /// Provides access to the internal pixel data. It is not required
        /// to check the validity of it's arguments. 
        /// </summary>
        /// <param name="col">Column from the left</param>
        /// <param name="row">Row from the top</param>
        /// <returns>The color of the desired pixel</returns>
        protected override Color GetPixelInternal(int col, int row)
        {
            //locates the desiered pixel
            int index = (col * height) + row;

            //converts the byte values to floating points
            double r = red[index] / 255.0;
            double g = green[index] / 255.0;
            double b = blue[index] / 255.0;

            return new Color(r, g, b, 1.0);
        }

        /// <summary>
        /// Provides access to the internal pixel data. It is not required
        /// to check the validity of it's arguments. 
        /// </summary>
        /// <param name="col">Column from the left</param>
        /// <param name="row">Row from the top</param>
        /// <param name="color">New color of the pixel</param>
        protected override void SetPixelInternal(int col, int row, Color color)
        {
            //locates the desiered pixel
            int index = (col * height) + row;

            //descretises the components to be within on byte
            red[index] = Descritise(color.Red);
            green[index] = Descritise(color.Green);
            blue[index] = Descritise(color.Blue);
        }

        #endregion //////////////////////////////////////////////////////////////

        private byte Descritise(double value)
        {
            return (byte)Math.Floor((value * 255.0) + 0.5);
        }

    }
}
