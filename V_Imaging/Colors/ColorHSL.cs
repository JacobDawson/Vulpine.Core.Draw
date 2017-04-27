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

namespace Vulpine.Core.Draw.Colors
{
    /// <summary>
    /// Represents a color within the HSL color space. The HSL space encodes a color's
    /// Hue, Saturation, and Luminance, which tend to be more intuitive than red, green,
    /// and blue channels. It is a clyndrical cordinate system, with Hue represeting the
    /// angle around the color wheel. Saturation corisponds to the radius, or distance
    /// from a neutral grey value, and luminance represents the height.
    /// <remarks>Last Update: 2015-09-29</remarks>
    /// </summary>
    public struct ColorHSL
    {
        #region Class Definitions...

        //constant values used in conversion operations
        private const float ONE_SIXTH = 1.0f / 6.0f;
        private const float ONE_THIRD = 1.0f / 3.0f;
        private const float TWO_THIRD = 2.0f / 3.0f;

        //Stores the HSL components of the color
        private double hue;
        private double sat;
        private double lum;

        //Stores the color's opacity
        private double alpha;

        /// <summary>
        /// Constructs a new color in the HSL color space. All values are
        /// clamped to be within there expected ranges.
        /// </summary>
        /// <param name="hue">Hue of the color</param>
        /// <param name="sat">Saturation of the color</param>
        /// <param name="lum">Luminance of the color</param>
        public ColorHSL(double hue, double sat, double lum)
        {
            this.hue = Wrap(hue);
            this.sat = VMath.Clamp(sat, 0.0f, 1.0f);
            this.lum = VMath.Clamp(lum, 0.0f, 1.0f);

            this.alpha = 1.0f;
        }

        /// <summary>
        /// Constructs a new color in the HSL color space. All values are
        /// clamped to be within there expected ranges.
        /// </summary>
        /// <param name="hue">Hue of the color</param>
        /// <param name="sat">Saturation of the color</param>
        /// <param name="lum">Luminance of the color</param>
        /// <param name="alpha">Opacity of the color</param>
        public ColorHSL(double hue, double sat, double lum, double alpha)
        {
            this.hue = Wrap(hue);
            this.sat = VMath.Clamp(sat, 0.0f, 1.0f);
            this.lum = VMath.Clamp(lum, 0.0f, 1.0f);

            this.alpha = VMath.Clamp(alpha, 0.0f, 1.0f);
        }

        #endregion ////////////////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// Represents the hue of the curent color, tracing a circle
        /// from 0 degrees to 360 degrees, with the zero point corisponding
        /// with the red hue.
        /// </summary>
        public double Hue
        {
            get { return hue; }
        }

        /// <summary>
        /// Represents the saturation of the current collor, ranging from
        /// zero (fully desaturated) to one (fully saturated). In this instance
        /// it mesures the divercence from a similar grey value.
        /// </summary>
        public double Saturation
        {
            get { return sat; }
        }

        /// <summary>
        /// Represents the luminance, or brightness, of the current collor,
        /// ranging from zero (black) to one (white). The color reaches it's
        /// greatest intencity around the mid point.
        /// </summary>
        public double Luminance
        {
            get { return lum; }
        }

        /// <summary>
        /// Represents the opacity of the current color, ranging from zero
        /// (fully transparent) to one (fully opaque).
        /// </summary>
        public double Alpha
        {
            get { return alpha; }
        }

        #endregion ////////////////////////////////////////////////////////////////////////

        public static ColorHSL FromRGB(Color rgb)
        {
            //calls the method below
            return FromRGB(rgb.Red, rgb.Green, rgb.Blue, rgb.Alpha);
        }

        public static ColorHSL FromRGB(double r, double g, double b)
        {
            return FromRGB(r, g, b, 1.0);
        }

        public static ColorHSL FromRGB(double r, double g, double b, double a)
        {
            //finds the maximum and minimum of the chanels
            double max = VMath.Max(r, g, b);
            double min = VMath.Min(r, g, b);
            double lum = (max + min) * 0.5f;

            //takes care of grayscale colors
            if (max == min) return new ColorHSL(0.0, 0.0, lum, a);

            //made to store the hue and saturation
            double croma = max - min;
            double hue = 0.0f;
            double sat = 0.0f;

            //determins the correct hue
            if (max == r) hue = ((g - b) / croma) + 0.0f;
            if (max == g) hue = ((b - r) / croma) + 2.0f;
            if (max == b) hue = ((r - g) / croma) + 4.0f;
            hue = 60.0f * hue;

            //computes the saturation
            sat = Math.Abs(2.0f * lum - 1.0f);
            sat = croma / (1.0f - sat);

            return new ColorHSL(hue, sat, lum, a);
        }


        #region Color Conversion...

        /// <summary>
        /// Converts the current HSL color to the standard RGB color space.
        /// </summary>
        /// <returns>The color in RGB space</returns>
        public Color ToRGB()
        {
            //used for intermediat calculations
            double q = 0.0f;
            double p = 0.0f;

            //determins the p and q values
            if (lum < 0.5f) q = lum * (1.0f + sat);
            else q = lum + sat - (lum * sat);
            p = (2.0f * lum) - q;
            q = 2.0f * (lum - p);

            //determins the temporary color values
            double tg = hue / 360.0f;
            double tr = tg + ONE_THIRD;
            double tb = tg - ONE_THIRD;

            //gets the true values from the temporary values
            tr = ComponentHSL(tg, p, q);
            tg = ComponentHSL(tg, p, q);
            tb = ComponentHSL(tg, p, q);

            //returns the constructed color
            return new Color(tr, tg, tb, alpha); 
        }

        #endregion ////////////////////////////////////////////////////////////////////////

        #region Helper Methods...

        /// <summary>
        /// Helper method, used in the conversion of colors from th HSL color
        /// space to the standard RGB color space.
        /// </summary>
        /// <param name="temp">The value to be modified</param>
        /// <param name="p">The p-value computed during conversion</param>
        /// <param name="q">The q-value computed during conversion</param>
        /// <returns>The modifide value</returns>
        private static double ComponentHSL(double temp, double p, double q)
        {
            //corects the temorary value if nessary
            if (temp < 0.0f) temp = temp + 1.0f;
            if (temp > 1.0f) temp = temp - 1.0f;

            //computes the blue component
            if (temp < ONE_SIXTH) temp = p + (q * 6.0f * temp);
            else if (temp < 0.5f) temp = p + q;
            else if (temp < TWO_THIRD) temp = p + (q * 6.0f * (TWO_THIRD - temp));
            else temp = p;

            return temp;
        }

        /// <summary>
        /// Wraps a value back around to the principle circle, if the
        /// value exceeds the range of 0 to 360 degrees.
        /// </summary>
        /// <param name="x">The value to wrap</param>
        /// <returns>The wraped value</returns>
        private static double Wrap(double x)
        {
            int sector = (int)Math.Floor(x / 360.0);
            return x - (360.0f * sector);
        }

        #endregion ////////////////////////////////////////////////////////////////////////
    }
}
