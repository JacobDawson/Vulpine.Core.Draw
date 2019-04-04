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
}
