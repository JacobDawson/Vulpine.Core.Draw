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

using Vulpine.Core.Calc;
using Vulpine.Core.Data.Exceptions;

namespace Vulpine.Core.Draw.Textures
{

    /// <summary>
    /// A class for various styles of color wheels. Most syles are based on the idea of
    /// maping polar cordinates to colors. The radius is used to determin the intencity
    /// of the color, while the argument determins the hue. Tipicaly, black is maped to
    /// the origin, while white mapes to a point at infinity. The unit circle is where
    /// the hues are there most vibrant.
    /// </summary>
    /// <remarks>Last Update: 2016-02-18</remarks>
    public class ColorWheel : Texture
    {
        #region Class Definitions...

        //IDEA: I should use delegats to define the various types of color wheels

        //stores the 'mode' of the color wheel
        private const int M = 3;
        private int mode;

        /// <summary>
        /// Private constructor for color wheels.
        /// </summary>
        /// <param name="mode">Type of color wheel</param>
        private ColorWheel(int mode)
        {
            this.mode = mode;
        }

        #endregion /////////////////////////////////////////////////////////////////

        #region Factory Methods...

        /// <summary>
        /// Returns the normal color wheel, with black maping to the origin,
        /// white maping to the point at infinity, and the hue rotating
        /// counter clockwise about the unit circle.
        /// </summary>
        public static ColorWheel Normal
        {
            get { return new ColorWheel(0); }
        }

        /// <summary>
        /// Similar to the normal color wheel, it modulates the intencity of
        /// the color sutch that it forms a saw tooth wave in concintric circles
        /// around the origin.
        /// </summary>
        public static ColorWheel Modulated
        {
            get { return new ColorWheel(1); }
        }

        /// <summary>
        /// Similar to the normal color wheel, except that it only distinguishes
        /// twleve distincet hues, showing a clear divide in the argument
        /// of the input function.
        /// </summary>
        public static ColorWheel Seperated
        {
            get { return new ColorWheel(2); }
        }

        #endregion /////////////////////////////////////////////////////////////////

        #region Texture Implenentation...

        /// <summary>
        /// Samples the texture at a given point, calculating the color of the 
        /// texture at that point. The sample point is provided in UV cordinats
        /// with the origin at the center and the V axis pointing up.
        /// </summary>
        /// <param name="u">The U texture cordinate</param>
        /// <param name="v">The V texture cordinate</param>
        /// <returns>The color sampled at the given point</returns>
        public Color Sample(double u, double v)
        {
            switch (mode % M)
            {
                case 0: return GetSpherical(u, v);
                case 1: return GetModulated(u, v);
                case 2: return GetPartitioned(u, v);
            }

            //we should not reach this point
            throw new NotSupportedException();
        }

        #endregion /////////////////////////////////////////////////////////////////

        #region Color Production...

        /// <summary>
        /// Generates a colored point with the origin as black, and white
        /// set to infinity. It provides no modulation or seperation.
        /// </summary>
        /// <param name="x">The X position</param>
        /// <param name="y">The Y position</param>
        /// <returns>The generated color</returns>
        private Color GetSpherical(double x, double y)
        {
            //obtains the polar cordinates
            double r = Math.Sqrt((x * x) + (y * y));
            double t = Math.Atan2(y, x);

            double hue = VMath.ToDeg(t);
            double lum = (-1.0 / (r + 1.0)) + 1.0;

            return Color.FromHSL(hue, 1.0, lum);
        }

        /// <summary>
        /// Generates a colored point with the origin as black. It modulates
        /// the colors so the intencity drops off in concentric bands.
        /// </summary>
        /// <param name="x">The X position</param>
        /// <param name="y">The Y position</param>
        /// <returns>The generated color</returns>
        private Color GetModulated(double x, double y)
        {
            //obtains the polar cordinates
            double r = Math.Sqrt((x * x) + (y * y));
            double t = Math.Atan2(y, x);

            double hue, val;

            hue = VMath.ToDeg(t);
            val = VMath.Log2(r);
            val = val - Math.Floor(val);
            val = (val * 0.5) + 0.5;

            return Color.FromHSV(hue, 1.0, val);
        }

        /// <summary>
        /// Generates a colored point with the origin as black, and white
        /// set to infinity. However, it only uses twelve hues.
        /// </summary>
        /// <param name="x">The X position</param>
        /// <param name="y">The Y position</param>
        /// <returns>The generated color</returns>
        private Color GetPartitioned(double x, double y)
        {
            //obtains the polar cordinates
            double r = Math.Sqrt((x * x) + (y * y));
            double t = Math.Atan2(y, x);

            double hue = VMath.ToDeg(t);
            double lum = (-1.0 / (r + 1.0)) + 1.0;

            hue = Math.Floor(hue / 30.0) * 30.0;

            return Color.FromHSL(hue, 1.0, lum);
        }

        #endregion /////////////////////////////////////////////////////////////////
    }
}
