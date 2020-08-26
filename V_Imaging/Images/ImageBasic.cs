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
    /// This class provides basic implementaiton of the abstract Image class. 
    /// Internaly it stores the color information as an array of bytes. How 
    /// many bites are stored per pixel is detemnined by the precice Pixel 
    /// Format given. Tipicaly the values stored are signifantly less precice
    /// than the values given in the Color struct, and so are rounded to the
    /// closest corisponding color. This is not a disadvantage in practice, 
    /// as most displays are only capable of displaying 24 bits per pixel anyway.
    /// </summary>
    /// <remarks>Last Update: 2019-04-04</remarks>
    public class ImageBasic : Image
    {
        #region Class Definitions...

        //remembers the width and height of the image
        private int width;
        private int height;

        //stores the size of one pixel in bites
        private int bite_size;

        //stores the pixel format
        private PixelFormat format;

        //stores the image data as a single bite array
        private byte[] data;

        /// <summary>
        /// Creates a new basic image with the given width and height, using
        /// the default pixel format, practal for most images.
        /// </summary>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        public ImageBasic(int width, int height)
        {
            //sets the width and the height
            this.width = width;
            this.height = height;

            //sets the format and bit-depth to default
            bite_size = 4;
            format = PixelFormat.Rgba32;

            //creates a byte arrray to store our image data
            data = new byte[width * height * bite_size];
        }

        /// <summary>
        /// Creates a new basic image with the given width and height, using
        /// the desired pixel format to store the image data.
        /// </summary>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        /// <param name="format">Desired pixel format</param>
        public ImageBasic(int width, int height, PixelFormat format)
        {
            //sets the width and the height
            this.width = width;
            this.height = height;

            //determins the format and bit-depth
            bite_size = format.BitDepth / 8;
            this.format = format;

            //creates a byte arrray to store our image data
            data = new byte[width * height * bite_size];
        }

        /// <summary>
        /// Creates a new basic image with the given width and heright, using
        /// the desired pixle format, and sets the tileability of the image. 
        /// </summary>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        /// <param name="format">Desired pixel format</param>
        /// <param name="ext">Tileability of the image</param>
        public ImageBasic(int width, int height, PixelFormat format, ImageExt ext)
        {
            //sets the width and the height
            this.width = width;
            this.height = height;

            //sets the tileablity
            base.SetTileability(ext);

            //determins the format and bit-depth
            bite_size = format.BitDepth / 8;
            this.format = format;

            //creates a byte arrray to store our image data
            data = new byte[width * height * bite_size];
        }

        /// <summary>
        /// Makes a copy of a given image, applying it to the given pixel format.
        /// </summary>
        /// <param name="other">Image to copy</param>
        /// <param name="format">Format to apply</param>
        public ImageBasic(Image other, PixelFormat format)
        {
            //copies the widht and height
            this.width = other.Width;
            this.height = other.Height;

            //copies the tileablity
            base.SetTileability(other);

            //determins the format and bit-depth
            bite_size = format.BitDepth / 8;
            this.format = format;

            //creates a byte arrray to store our image data
            data = new byte[width * height * bite_size];

            //fills the image with data from the source
            this.FillData(other);
        }

        #endregion //////////////////////////////////////////////////////////////

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
        public PixelFormat Format
        {
            get { return format; }
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
            int index = ((col * height) + row) * bite_size;

            //uses the format to decode the data
            return format.DecodeColor(data, index);
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
            int index = ((col * height) + row) * bite_size;

            //uses the format to encode the data
            format.EncodeColor(data, index, color);
        }

        #endregion //////////////////////////////////////////////////////////////

    }
}
