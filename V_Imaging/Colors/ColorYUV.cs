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
