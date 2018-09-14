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

using Vulpine.Core.Calc;

namespace Vulpine.Core.Draw.Colors
{
    public struct ColorYUV
    {
        //stores the inverse weights for the RGB components
        private const float IBG = 0.3441362862f;
        private const float IRG = 0.7141362862f;
        private const float IWR = 1.402f;
        private const float IWB = 1.772f;

        //stores the luma and the u and v chanells
        private float luma;
        private float uchan;
        private float vchan;

        //Stores the color's opacity
        private float alpha;

        public ColorYUV(float luma, float u, float v)
        {
            this.luma = (float)VMath.Clamp(luma, 0.0f, 1.0f);
            this.uchan = (float)VMath.Clamp(u, -0.5f, 0.5f);
            this.vchan = (float)VMath.Clamp(v, -0.5f, 0.5f);

            this.alpha = 1.0f;
        }

        public ColorYUV(float luma, float u, float v, float alpha)
        {
            this.luma = (float)VMath.Clamp(luma, 0.0f, 1.0f);
            this.uchan = (float)VMath.Clamp(u, -0.5f, 0.5f);
            this.vchan = (float)VMath.Clamp(v, -0.5f, 0.5f);

            this.alpha = (float)VMath.Clamp(alpha, 0.0f, 1.0f);
        }

        public float Luma
        {
            get { return luma; }
        }

        public float U_Chan
        {
            get { return uchan; }
        }

        public float V_Chan
        {
            get { return vchan; }
        }

        /// <summary>
        /// Represents the opacity of the current color, ranging from zero
        /// (fully transparent) to one (fully opaque).
        /// </summary>
        public float Alpha
        {
            get { return alpha; }
        }

        /// <summary>
        /// Converts the current YUV color to the standard RGB color space.
        /// </summary>
        /// <returns>The color in RGB space</returns>
        public Color ToRGB()
        {
            //recalcuates the Red Blue and Green components
            float red = luma + (vchan * IWR);
            float blue = luma + (uchan * IWB);
            float green = luma - (uchan * IBG) - (vchan * IRG);

            return new Color(red, green, blue, alpha);
        }

    }
}
