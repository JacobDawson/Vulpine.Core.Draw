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

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Matrices;
using Vulpine.Core.Calc.Exceptions;
using Vulpine.Core.Data.Exceptions;

namespace Vulpine.Core.Draw
{
    /// <summary>
    /// Represents a color as a four dimentional vector with a red, green and blue
    /// component that determin the color and an alpha component which determins
    /// it's opacity. These components are stored as floating point values clamped
    /// to be within 0.0 and 1.0, inclusive. This is mostly done to make computation
    /// easier, and mitigate round-off error. The values themselves may be liniar, or
    /// not, and the alphal values may be pre-multiplied, or not. To change between
    /// these states one can use the SetGamma, PreMultiply, and PostDeivide methods.
    /// Although, care must be taken not to gamma-correct a color that was already
    /// corrected, or pre-multiply a color that was already pre-multiplied. By default,
    /// colors are asumed to be pre-multiplied, with a gamma of 2.2, unless otherwise
    /// stated. We do not imply a fixed standard for the color struct in order to
    /// enshure maximum flexability.
    /// </summary>
    public struct Color
    {
        #region Class Definitons...

        //weights that can be used for caluclating luma
        private const double WR = 0.299;
        private const double WG = 0.587;
        private const double WB = 0.114;

        //weights used in converting from YUV
        private const double IWR = 1.402;
        private const double IWB = 1.772;

        //stores the matrix values for conversion from XYZ space
        private static readonly double[] MX =
        {
             3.2406,
            -1.5372,
            -0.4986,
            -0.9689,
             1.8758,
             0.0415,
             0.0557,
            -0.2040,
             1.0570,
        };

        //stores the matrix values for conversion to XYZ space
        private static readonly double[] IMX =
        {
            0.4123955890,
            0.3575834308,
            0.1804926474,
            0.2125862308,
            0.7151703037,
            0.0722004986,
            0.0192972155,
            0.1191838646,
            0.9504971251,
        };

        //Stores the RGB components of the color
        private double red;
        private double green;
        private double blue;

        //Stores the color's opacity
        private double alpha;

        /// <summary>
        /// Protected constructor for creating a new color. The end user
        /// should use the factory methods for creating new colors instead.
        /// </summary>
        /// <param name="r">The red channel</param>
        /// <param name="g">The green channel</param>
        /// <param name="b">The blue channel</param>
        internal Color(double r, double g, double b)
        {
            //always make shure the result are in-gambut
            red = VMath.Clamp(r);
            green = VMath.Clamp(g);
            blue = VMath.Clamp(b);

            alpha = 1.0;
        }

        /// <summary>
        /// Protected constructor for creating a new color. The end user
        /// should use the factory methods for creating new colors instead.
        /// </summary>
        /// <param name="r">The red channel</param>
        /// <param name="g">The green channel</param>
        /// <param name="b">The blue channel</param>
        /// <param name="a">The alpha channel</param>
        internal Color(double r, double g, double b, double a)
        {
            //always make shure the result are in-gambut
            red = VMath.Clamp(r);
            green = VMath.Clamp(g);
            blue = VMath.Clamp(b);

            alpha = VMath.Clamp(a);
        }

        #endregion ///////////////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// The Red Channel or the amount of redness in the color.
        /// </summary>
        public double Red
        {
            get { return red; }
        }

        /// <summary>
        /// The Green Channel or the amount of greenness in the color.
        /// </summary>
        public double Green
        {
            get { return green; }
        }

        /// <summary>
        /// The Blue Channel or the amount of blueness in the color.
        /// </summary>
        public double Blue
        {
            get { return blue; }
        }

        /// <summary>
        /// Represents the hue of the curent color, tracing a circle
        /// from 0 degrees to 360 degrees, with the zero point corisponding
        /// with the red hue.
        /// </summary>
        public double Hue
        {
            get
            {
                //finds the maximum and minimum of the chanels
                double max = VMath.Max(red, green, blue);
                double min = VMath.Min(red, green, blue);

                //if we are a grey value, then we have no hue
                if (max == min) return 0.0;

                //computes the croma
                double croma = max - min;
                double hue = 0.0;

                //determins the correct hue
                if (max == red) hue = ((green - blue) / croma) + 0.0;
                if (max == green) hue = ((blue - red) / croma) + 2.0;
                if (max == blue) hue = ((red - green) / croma) + 4.0;

                return (60.0 * hue);
            }
        }

        /// <summary>
        /// Represents the saturation of the current collor, ranging from
        /// zero (fully desaturated) to one (fully saturated). In this instance
        /// it mesures the divercence from a similar grey value.
        /// </summary>
        public double Saturation
        {
            get
            {
                //finds the maximum and minimum of the chanels
                double max = VMath.Max(red, green, blue);
                double min = VMath.Min(red, green, blue);

                //if we are a grey value, then we have no saturation
                if (max == min) return 0.0;

                //calculates the croma and the saturation
                double croma = max - min;
                double sat = croma / max;

                return sat;
            }
        }

        /// <summary>
        /// Represents the value, or maximum component, of the current collor,
        /// ranging from zero (black) to one (full intencity).
        /// </summary>
        public double Value
        {
            get
            {
                //simply returns the maximum of the channels
                return VMath.Max(red, green, blue);
            }
        }

        /// <summary>
        /// Computes the luminance of the current color, or how bright the color
        /// appears, by taking a weighted average of the three channels, based
        /// on the aparent brightness of the three primaries. 
        /// </summary>
        public double Luminance
        {
            get
            {
                //computes the waited average based on aparent brightness
                return (red * WR) + (green * WG) + (blue * WB);
            }
        }

        /// <summary>
        /// Represents the opacity of the current color, ranging from zero
        /// (fully transparent) to one (fully opaque).
        /// </summary>
        public double Alpha
        {
            get { return alpha; }
        }

        #endregion ///////////////////////////////////////////////////////////////////////

        #region Factory Methods...

        /// <summary>
        /// Creates a new color, represented by the 24-bit RGB standard, where each
        /// of the red, green, and blue components are 8-bit values in the range of
        /// 0 to 255. Values given outside this range are clamped to fit.
        /// </summary>
        /// <param name="r">Red chanel ranging form 0 to 255</param>
        /// <param name="g">Green channel ranging from 0 to 255</param>
        /// <param name="b">Blue channel ranging from 0 to 255</param>
        /// <returns>The New Color</returns>
        public static Color FromRGB24(int r, int g, int b)
        {
            //divides each input by 255
            double rf = r / 255.0;
            double gf = g / 255.0;
            double bf = b / 255.0;

            //returns the new color, clamped
            return new Color(r, g, b);
        }


        /// <summary>
        /// Creates a new color from a point in the corisponding color space.
        /// </summary>
        /// <param name="v">A point inside a color space</param>
        /// <param name="cs">The color space being sampled</param>
        /// <returns>The desired color</returns>
        public static Color FromVector(Vector v, ColorSpace cs)
        {
            //extracts the individual compoents of the vector
            double v0 = v.GetExtended(0);
            double v1 = v.GetExtended(1);
            double v2 = v.GetExtended(3);

            switch (cs)
            {
                //converts the color to the desired color space
                case ColorSpace.RGB: return FromRGB(v0, v1, v2);
                case ColorSpace.HSL: return FromHSL(v0, v1, v2);
                case ColorSpace.HSV: return FromHSV(v0, v1, v2);
                case ColorSpace.YUV: return FromYUV(v0, v1, v2);
                case ColorSpace.XYZ: return FromXYZ(v0, v1, v2);
                case ColorSpace.LAB: return FromLAB(v0, v1, v2);

                //we default to RGB if no other color space is valid
                default: return FromRGB(v0, v1, v2);
            }
        }

        /// <summary>
        /// Construct a new color given it's RGB color space cordinates. All values
        /// are clamped to be within the range of zero to one.
        /// </summary>
        /// <param name="r">Red channel ranging from zero to one </param>
        /// <param name="g">Green chanel ranging from zero to one</param>
        /// <param name="b">Blue chanel ranging from zero to one</param>
        /// <returns>The new color</returns>
        public static Color FromRGB(double r, double g, double b)
        {
            //creates the color as specified
            return new Color(r, g, b);
        }

        /// <summary>
        /// Construct a new color given it's RGB color space cordinates. All values
        /// are clamped to be within the range of zero to one.
        /// </summary>
        /// <param name="r">Red channel ranging from zero to one </param>
        /// <param name="g">Green chanel ranging from zero to one</param>
        /// <param name="b">Blue chanel ranging from zero to one</param>
        /// <param name="a">The opacity of the color</param>
        /// <returns>The new color</returns>
        public static Color FromRGBA(double r, double g, double b, double a)
        {
            //creates the color as specified
            return new Color(r, g, b, a);
        }

        /// <summary>
        /// Construct a new color given it's RGB color space cordinates. All values
        /// are clamped to be within the range of zero to one.
        /// </summary>
        /// <param name="rgb">A vector containing the RGB cordinates</param>
        /// <returns>The new color</returns>
        public static Color FromRGBA(Vector rgb)
        {
            //extracts the color components that are available
            double r = rgb.GetExtended(0, 0.0);
            double g = rgb.GetExtended(1, 0.0);
            double b = rgb.GetExtended(2, 0.0);
            double a = rgb.GetExtended(3, 1.0);

            //creates the color as specified
            return new Color(r, g, b, a);
        }

        /// <summary>
        /// Constructs a new color given it's HSV color cordinates, where value ranges
        /// from black to full intencity. Both the saturation and the value are clamped 
        /// to the range of zero to one.
        /// </summary>
        /// <param name="hue">Angular hue, ranging from 0 to 360 degrees</param>
        /// <param name="sat">Saturation ranging from zero to one</param>
        /// <param name="val">Value ranging from zero to one</param>
        /// <returns>The new color</returns>
        public static Color FromHSV(double hue, double sat, double val)
        {
            //clamps the values to be within there expected ranges
            hue = WrapValue(hue);
            sat = VMath.Clamp(sat);
            val = VMath.Clamp(val);

            //computes the sector and temporary variable
            double sec = hue / 60.0;
            double temp = sec - Math.Floor(sec);

            //computes potential values for Red, Blue, and Green
            double p = val * (1.0 - sat);
            double q = val * (1.0 - temp * sat);
            double t = val * (1.0 - (1.0 - temp) * sat);

            //sets the components based on the current sector
            if (sec < 1.0) return new Color(val, t, p, 1.0);
            if (sec < 2.0) return new Color(q, val, p, 1.0);
            if (sec < 3.0) return new Color(p, val, t, 1.0);
            if (sec < 4.0) return new Color(p, q, val, 1.0);
            if (sec < 5.0) return new Color(t, p, val, 1.0);
            if (sec < 6.0) return new Color(val, p, q, 1.0);

            //we should never reach this part of the code
            return new Color(0.5, 0.5, 0.5);
        }

        /// <summary>
        /// Constructs a new color given it's HSV color cordinates, where value ranges
        /// from black to full intencity. Both the saturation and the value are clamped 
        /// to the range of zero to one.
        /// </summary>
        /// <param name="hsv">A vector containing the HSV cordinates</param>
        /// <returns>The new color</returns>
        public static Color FromHSV(Vector hsv)
        {
            //extracts the color components that are available
            double hue = hsv.GetExtended(0);
            double sat = hsv.GetExtended(1);
            double val = hsv.GetExtended(2);

            //calls the method above
            return Color.FromHSV(hue, sat, val);
        }

        /// <summary>
        /// Constructs a new color given it's HSL space cordinates, where luminance
        /// ranges from black to white. Both the saturation and the luminance are 
        /// clamped to the range of zero to one.
        /// </summary>
        /// <param name="hue">Angular hue, ranging from 0 to 360 degrees</param>
        /// <param name="sat">Saturation ranging from zero to one</param>
        /// <param name="lum">Luminosity ranging from zero to one</param>
        /// <returns>The new color</returns>
        public static Color FromHSL(double hue, double sat, double lum)
        {
            //clamps the values to be within there expected ranges
            hue = WrapValue(hue);
            sat = VMath.Clamp(sat);
            lum = VMath.Clamp(lum);

            //used for intermediat calculations
            double q = 0.0;
            double p = 0.0;

            //determins the p and q values
            if (lum < 0.5) q = lum * (1.0 + sat);
            else q = lum + sat - (lum * sat);
            p = (2.0 * lum) - q;
            q = 2.0 * (lum - p);

            //determins the temporary color values
            double tg = hue / 360.0;
            double tr = tg + (1.0 / 3.0);
            double tb = tg - (1.0 / 3.0);

            //gets the true values from the temporary values
            tr = ComponentHSL(tr, p, q);
            tg = ComponentHSL(tg, p, q);
            tb = ComponentHSL(tb, p, q);

            //returns the constructed color
            return new Color(tr, tg, tb, 1.0);
        }

        /// <summary>
        /// Constructs a new color given it's HSL space cordinates, where luminance
        /// ranges from black to white. Both the saturation and the luminance are 
        /// clamped to the range of zero to one.
        /// </summary>
        /// <param name="hsl">A vector containing the HSL cordinates</param>
        /// <returns>The new color</returns>
        public static Color FromHSL(Vector hsl)
        {
            //extracts the color components that are available
            double hue = hsl.GetExtended(0);
            double sat = hsl.GetExtended(1);
            double lum = hsl.GetExtended(2);

            //calls the method above
            return Color.FromHSL(hue, sat, lum);
        }

        /// <summary>
        /// Constructs a new color given it's YUV space cordinates. Note that luma is
        /// clamped to be within zero to one, while the U and V channels are clamped to 
        /// be within negative one-half to one-half. 
        /// </summary>
        /// <param name="luma">The luminance value</param>
        /// <param name="u">The U channel color component</param>
        /// <param name="v">The V Channel color component</param>
        /// <returns>The new color</returns>
        public static Color FromYUV(double luma, double u, double v)
        {
            //clamps the values to be within there expected ranges
            luma = VMath.Clamp(luma);
            u = VMath.Clamp(u, -0.5, 0.5);
            v = VMath.Clamp(v, -0.5, 0.5);

            //first computes the red and blue channels
            double red = luma + v * IWR;
            double blue = luma + u * IWB;

            //the computes the green channel
            double green = luma - v * ((WR * IWR) / WG);
            green = green - u * ((WB * IWB) / WG);

            return new Color(red, green, blue);
        }

        /// <summary>
        /// Constructs a new color given it's YUV space cordinates. Note that luma is
        /// clamped to be within zero to one, while the U and V channels are clamped to 
        /// be within negative one-half to one-half. 
        /// </summary>
        /// <param name="yuv">A vector containing the YUV cordinates</param>
        /// <returns>The new color</returns>
        public static Color FromYUV(Vector yuv)
        {
            //extracts the color components that are available
            double y = yuv.GetExtended(0);
            double u = yuv.GetExtended(1);
            double v = yuv.GetExtended(2);

            //calls the method above
            return Color.FromYUV(y, u, v);
        }

        public static Color FromXYZ(double x, double y, double z)
        {
            //converts from XYZ to liniar RGB
            double r = MX[0] * x + MX[1] * y + MX[2] * z;
            double g = MX[3] * x + MX[4] * y + MX[5] * z;
            double b = MX[6] * x + MX[7] * y + MX[8] * z;

            //applies a gamma curve to the liniar RGB
            r = InvGamma(r);
            g = InvGamma(g);
            b = InvGamma(b);

            //returnes the scaled RGB values
            return new Color(r, g, b);
        }

        public static Color FromXYZ(Vector xyz)
        {
            //extracts the color components that are available
            double x = xyz.GetExtended(0);
            double y = xyz.GetExtended(1);
            double z = xyz.GetExtended(2);

            //calls the method above
            return Color.FromXYZ(x, y, z);
        }

        public static Color FromLAB(double lum, double a, double b)
        {
            //used in further calculations
            double t = (lum + 16.0) / 116.0;

            //computes the CIE XYZ cordinates
            double x = 0.95047 * LabInvF(t + (a / 500.0));
            double y = 1.0 * LabInvF(t);
            double z = 1.08883 * LabInvF(t - (b / 200.0));

            //continues the conversion to RGB
            return Color.FromXYZ(x, y, z);
        }

        public static Color FromLAB(Vector lab)
        {
            //extracts the color components that are available
            double l = lab.GetExtended(0);
            double a = lab.GetExtended(1);
            double b = lab.GetExtended(2);

            //calls the method above
            return Color.FromLAB(l, a, b);
        }

        public static Color FromLUV(double lum, double u, double v)
        {
            double up = (u / (13.0 * lum)) + 0.1978398248;
            double vp = (v / (13.0 * lum)) + 0.4683363029;

            //double y = 0.0;

            //if (lum <= 8.0)
            //{
            //    y = lum * 0.001107056460 * 100.0;
            //}
            //else
            //{
            //    y = (lum + 16.0) / 116.0;
            //    y = y * y * y * 100.0;
            //}

            double y = (lum > 8.0) ? (lum + 16.0) / 116.0 : lum;
            y = (lum > 8.0) ? y * y * y : y * 0.001107056460;
            //y = 100.0 * y;

            double x = y * ((9.0 * up) / (4.0 * vp));
            double z = y * ((12.0 - (3.0 * up) - (20.0 * vp)) / (4.0 * vp));

            return Color.FromXYZ(x, y, z);
        }

        public static Color FromLUV(Vector luv)
        {
            //extracts the color components that are available
            double l = luv.GetExtended(0);
            double u = luv.GetExtended(1);
            double v = luv.GetExtended(2);

            //calls the method above
            return Color.FromLUV(l, u, v);
        }

        #endregion //////////////////////////////////////////////////////////////////

        #region Conversion Methods...

        /// <summary>
        /// Generates a representaiton of the color as a vector in the desired
        /// color space.
        /// </summary>
        /// <param name="cs">The color space to sample into</param>
        /// <returns>A vector representaton in the desired color space</returns>
        public Vector ToVector(ColorSpace cs)
        {
            switch (cs)
            {
                //converts the color to the desired color space
                case ColorSpace.RGB: return ToRGB();
                case ColorSpace.HSL: return ToHSL();
                case ColorSpace.HSV: return ToHSV();
                case ColorSpace.YUV: return ToYUV();
                case ColorSpace.XYZ: return ToXYZ();
                case ColorSpace.LAB: return ToLAB();

                //by default we convert to RGB if no other space applys
                default: return ToRGB();
            }
        }

        /// <summary>
        /// Generates a representaiton of the color as a vector in the RGB color
        /// space. This is usefull for treating colors as a mathmatical construct.
        /// </summary>
        /// <returns>The color as an RGB vector</returns>
        public Vector ToRGB()
        {
            return new Vector(red, green, blue);
        }

        /// <summary>
        /// Generates a representaiton of the color as a vector in the RGBA color
        /// space. This is usefull for treating colors as a mathmatical construct.
        /// </summary>
        /// <returns>The color as an RGBA vector</returns>
        public Vector ToRGBA()
        {
            return new Vector(red, green, blue, alpha);
        }

        /// <summary>
        /// Generates a representation of the color as a vector in the HSV color
        /// space. Using this method is more effecient than reading the corisponding
        /// properties, as it avoids redundent calculation.
        /// </summary>
        /// <returns>The color as an HSV vector</returns>
        public Vector ToHSV()
        {
            //finds the maximum and minimum of the chanels
            double max = VMath.Max(red, green, blue);
            double min = VMath.Min(red, green, blue);

            //if we are a grey value, then we have only value
            if (max == min) return new Vector(0.0, 0.0, max);

            //calculates the croma and the saturation
            double croma = max - min;
            double sat = croma / max;
            double hue = 0.0;

            //determins the correct hue
            if (max == red) hue = ((green - blue) / croma) + 0.0;
            if (max == green) hue = ((blue - red) / croma) + 2.0;
            if (max == blue) hue = ((red - green) / croma) + 4.0;

            hue = 60.0 * hue;

            return new Vector(hue, sat, max);
        }

        /// <summary>
        /// Generates a representaiton of the color as a vector in the HSL color
        /// space, not to be confused with the HSV color space. This is usefull 
        /// when one needs a more perceptual definiton of color.
        /// </summary>
        /// <returns>The color as an HSL vector</returns>
        public Vector ToHSL()
        {
            //finds the maximum and minimum of the chanels
            double max = VMath.Max(red, green, blue);
            double min = VMath.Min(red, green, blue);
            double lum = (max + min) * 0.5;

            //takes care of the most basic case
            if (max == min) return new Vector(0.0, 0.0, lum); 

            //made to store the hue and saturation
            double croma = max - min;
            double hue = 0.0;
            double sat = 0.0;

            //determins the correct hue
            if (max == red) hue = ((green - blue) / croma) + 0.0;
            if (max == green) hue = ((blue - red) / croma) + 2.0;
            if (max == blue) hue = ((red - green) / croma) + 4.0;
            hue = 60.0 * hue;

            //computes the saturation
            sat = Math.Abs(2.0 * lum - 1.0);
            sat = croma / (1.0 - sat);

            return new Vector(hue, sat, lum);
        }

        /// <summary>
        /// Generates a representaiton of the color as a vector in the YUV color
        /// space, seperating the percieved brightness from the color information.
        /// Note that it is a liniar transformation of the RGB color space.
        /// </summary>
        /// <returns>The color as a YUV vector</returns>
        public Vector ToYUV()
        {
            //computes the luma and derives the other components
            double luma = (WR * red) + (WG * green) + (WB * blue);
            double uchan = (blue - luma) / (1.0 - WB);
            double vchan = (red - luma) / (1.0 - WR);

            //scales the u and v channels
            uchan = uchan * 0.5;
            vchan = vchan * 0.5;

            return new Vector(luma, uchan, vchan);
        }

        public Vector ToXYZ()
        {
            //converts the scaled RGB vlaues to liniar RGB
            double rl = Gamma(red);
            double gl = Gamma(green);
            double bl = Gamma(blue);

            double x = rl * IMX[0] + gl * IMX[1] + bl * IMX[2];
            double y = rl * IMX[3] + gl * IMX[4] + bl * IMX[5];
            double z = rl * IMX[6] + gl * IMX[7] + bl * IMX[8];

            return new Vector(x, y, z);
        }

        public Vector ToLAB()
        {
            //first converts to XYZ space
            Vector xyz = this.ToXYZ();

            double fx = LabF(xyz[0] / 0.95047);
            double fy = LabF(xyz[1] / 1.0);
            double fz = LabF(xyz[2] / 1.08883);

            double l = 116.0 * fy - 16.0;
            double a = 500.0 * (fx - fy);
            double b = 200.0 * (fy - fz);

            return new Vector(l, a, b);
        }

        public Vector ToLUV()
        {
            //first converts to XYZ space
            Vector xyz = this.ToXYZ();

            double x = xyz[0];
            double y = xyz[1];
            double z = xyz[2];

            //double yn = xyz[1] / 100.0;
            double lum = 0.0;

            if (y < 0.008856451679)
            {
                lum = 903.2962963 * y;
            }
            else
            {
                lum = Math.Pow(y, 1.0 / 3.0);
                lum = 116.0 * lum - 16.0;
            }

            double temp = x + (15.0 * y) + (3.0 * z);
            double up = (4.0 * x) / temp;
            double vp = (9.0 * y) / temp;

            double u = 13.0 * lum * (up - 0.1978398248);
            double v = 13.0 * lum * (vp - 0.4683363029);

            return new Vector(lum, u, v);

        }

        #endregion //////////////////////////////////////////////////////////////////

        #region Color Operations...

        /// <summary>
        /// Sets the colors gama value by scaling each component by the apropriate amount.
        /// This is useful for doing gama correction on an image or output device. Note
        /// that the transformation is inherently non-liniar.
        /// </summary>
        /// <param name="gamma">The new gamma value</param>
        /// <returns>The color with corrected gamma</returns>
        public Color SetGamma(double gamma)
        {
            //clips the most extream gamma values
            if (gamma < 0.001) return new Color(1.0, 1.0, 1.0, alpha);
            if (gamma > 1000.0) return new Color(0.0, 0.0, 0.0, alpha);

            //raises the color components to the gamma level
            double rn = Math.Pow(red, gamma);
            double gn = Math.Pow(green, gamma);
            double bn = Math.Pow(blue, gamma);

            //returns the corrected color with the same alpha
            return new Color(rn, gn, bn, this.alpha);
        }

        /// <summary>
        /// Converts the color from standard form to pre-multiplied form. This allows
        /// for faster layering of images, and reduces artifacts when transforming
        /// images that contain transparent alphas.
        /// </summary>
        /// <returns>The pre-multiplied form of the color</returns>
        public Color PreMultiply()
        {
            //multiplies the color components by the alpha
            double rn = red * alpha;
            double gn = green * alpha;
            double bn = blue * alpha;

            //returns the premultiplied color
            return new Color(rn, gn, bn, alpha);
        }

        /// <summary>
        /// Converts the color from pre-multiplied form to standard form. Note that this
        /// transformation is only nessary when the resulting image contains transparent
        /// alpha values, and the stardard form is expected.
        /// </summary>
        /// <returns>The standard form of the color</returns>
        public Color PostDivide()
        {
            //avoids potential division by zero
            if (alpha.IsZero()) return this;

            //divides the color components by alpha
            double rn = red / alpha;
            double gn = green / alpha;
            double bn = blue / alpha;

            //returns the premultiplied color
            return new Color(rn, gn, bn, alpha);
        }

        /// <summary>
        /// Converts the curtent color to a corisponding grey value, using
        /// the default method spesified by the library.
        /// </summary>
        /// <returns>The corisponding grey value</returns>
        public Color GetGrayscale()
        {
            //uses the method below
            return GetGrayscale(Desaturate.Default);
        }

        /// <summary>
        /// Converts the curent color to a corisponding grey value, using
        /// the desired method. See GreyMethod for more detales.
        /// </summary>
        /// <param name="method">Conversion method to use</param>
        /// <returns>The corisponding grey value</returns>
        public Color GetGrayscale(Desaturate method)
        {
            //initialises luminace to an error value
            double lum = -1.0;

            switch (method)
            {
                case Desaturate.Average:
                    lum = (red + green + blue) / 3.0;
                    break;
                case Desaturate.Maximum:
                    lum = VMath.Max(red, green, blue);
                    break;
                case Desaturate.Lumanince:
                    double max = VMath.Max(red, green, blue);
                    double min = VMath.Min(red, green, blue);
                    lum = (max + min) / 2.0;
                    break;
                case Desaturate.Natural:
                    lum = (red * WR) + (green * WG) + (blue * WB);
                    break;
            }

            //makes certain that the lumanocity was set
            if (lum < 0.0) throw new NotSupportedException();

            return new Color(lum, lum, lum, alpha);
        }

        #endregion //////////////////////////////////////////////////////////////////

        #region Special Methods...    

        /// <summary>
        /// Blends two colors using there respective alpha values, effectivly laying
        /// the top color over the bottom color. Note that this method assumes the color
        /// compnents have been pre-multiplied with there alpha values.
        /// </summary>
        /// <param name="top">Color to lay on top</param>
        /// <param name="bot">Color to be overlane</param>
        /// <returns>The blended color</returns>
        public static Color Blend(Color top, Color bot)
        {
            //computes the inverse alpha to save time
            double inva = 1.0 - top.alpha;

            //blends each of the color components acordingly
            double an = top.alpha + bot.alpha * inva;
            double rn = top.red + bot.red * inva;
            double gn = top.green + bot.green * inva;
            double bn = top.blue + bot.blue * inva;

            return new Color(rn, gn, bn, an);
        }

        /// <summary>
        /// Blends two colors using there respective alpha values, effectivly laying
        /// the top color over the bottom color. Note that this method assumes the color
        /// components have not been pre-multiplied.
        /// </summary>
        /// <param name="top">Color to lay on top</param>
        /// <param name="bot">Color to be overlane</param>
        /// <returns>The blended color</returns>
        public static Color BlendLong(Color top, Color bot)
        {
            //frist computes intermediat values and the new alpha
            double alt = bot.alpha * (1.0 - top.alpha);
            double an = top.alpha + alt;

            //blends each of the color components acordingly
            double rn = (top.red * top.alpha + bot.red * alt) / an;
            double gn = (top.green * top.alpha + bot.green * alt) / an;
            double bn = (top.blue * top.alpha + bot.blue * alt) / an;

            return new Color(rn, gn, bn, an);
        }

        /// <summary>
        /// Preforms liniar interpolaiton between two colors in RGB space. When
        /// the interpoland is zero, the first color is returned, when it is one
        /// the second color is returned. For all other values it produces a color
        /// that is in-between the two colors.
        /// </summary>
        /// <param name="c1">The first color</param>
        /// <param name="c2">The second color</param>
        /// <param name="x">The interpoland</param>
        /// <returns>The interpolated color</returns>
        public static Color Lerp(Color c1, Color c2, double x)
        {
            //asserts that the interpolent is bracketed correctly
            x = VMath.Clamp(x);

            //computes the inverse of x to save time
            double invx = 1.0 - x;

            //interpolates each component linearly
            double rn = (c1.red * invx) + (c2.red * x);
            double gn = (c1.green * invx) + (c2.green * x);
            double bn = (c1.blue * invx) + (c2.blue * x);
            double an = (c1.alpha * invx) + (c2.alpha * x);

            return new Color(rn, gn, bn, an);
        }

        #endregion //////////////////////////////////////////////////////////////////

        #region Helper Methods...

        /// <summary>
        /// Helper method, used to enshure that angular values lie between zero
        /// and 360 degrees. Angles outisde this range are wraped around.
        /// </summary>
        /// <param name="value">The value to wrap</param>
        /// <returns>The wraped value</returns>
        private static double WrapValue(double value)
        {
            double sector = Math.Floor(value / 360.0);
            return value - (360.0 * sector);
        }

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
            if (temp < 0.0) temp = temp + 1.0;
            if (temp > 1.0) temp = temp - 1.0;

            //computes the modified component
            if (temp < (1.0 / 6.0)) temp = p + (q * 6.0 * temp);
            else if (temp < 0.5) temp = p + q;
            else if (temp < (2.0 / 3.0))
                temp = p + (q * 6.0 * ((2.0 / 3.0) - temp));
            else temp = p;

            return temp;
        }

        private static double Gamma(double u)
        {
            if (u < 0.04045)
            {
                //uses a liniar function near zero
                return u / 12.92;
            }
            else
            {
                //uses the augmented gamma adjustment
                double temp = (u + 0.055) / 1.055;
                return Math.Pow(temp, 2.4);
            }
        }

        private static double InvGamma(double u)
        {
            if (u < 0.003130807283) return 12.92 * u;
            else return 1.055 * Math.Pow(u, 1.0 / 2.4) - 0.055; 
        }


        private static double LabF(double t)
        {
            if (t > 0.008856451679) return Math.Pow(t, 1.0 / 3.0);
            else return t * 0.1284185493 + 0.1379310345;
        }

        private static double LabInvF(double t)
        {
            if (t > 0.2068965517) return t * t * t;
            else return 0.1284185493 * (t - 0.1379310344);
        }

        #endregion //////////////////////////////////////////////////////////////////

        #region Class Conversions...

        //allows implicit conversion to a vector without loss of data
        public static implicit operator Vector(Color c)
        { return c.ToRGBA(); }

        //allows explicit conversion from a vector with potential loss of data
        public static explicit operator Color(Vector v)
        { return Color.FromRGBA(v); }

        #endregion //////////////////////////////////////////////////////////////////


    }

}
