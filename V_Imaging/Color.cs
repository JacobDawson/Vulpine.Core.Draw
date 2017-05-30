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
using Vulpine.Core.Calc.Matrices;
using Vulpine.Core.Calc.Exceptions;
using Vulpine.Core.Data.Exceptions;

namespace Vulpine.Core.Draw
{
    public struct Color
    {
        #region Class Definitons...

        //weights that can be used for caluclating luma
        private const double WR = 0.299f;
        private const double WG = 0.587f;
        private const double WB = 0.114f;

        //weights used in converting from YUV
        private const double IWR = 1.402f;
        private const double IWB = 1.772f;

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

            alpha = 1.0f;
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
                if (max == min) return 0.0f;

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
                if (max == min) return 0.0f;

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
        public static Color FromRGB(double r, double g, double b, double a)
        {
            //creates the color as specified
            return new Color(r, g, b, a);
        }

        /// <summary>
        /// Construct a new color given it's RGB color space cordinates. All values
        /// are clamped to be within the range of zero to one.
        /// </summary>
        /// <param name="rgb">A vector containing the RGB cordinates</param>
        /// <exception cref="VectorLengthExcp">If the size of the vector is 
        /// invalid for either RGB or RGBA color cordinates</exception>
        /// <returns>The new color</returns>
        public static Color FromRGB(Vector rgb)
        {
            //makes shure the vector is an aproprate length
            VectorLengthExcp.Check(rgb, 3, 4);

            //creates the color based on the length of the vector
            if (rgb.Length == 3) return new Color(rgb[0], rgb[1], rgb[2]);
            else return new Color(rgb[0], rgb[1], rgb[2], rgb[3]);
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
            double sec = hue / 60.0f;
            double temp = Math.Floor(sec);
            temp = sec - temp;

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
        /// Constructs a new color given it's HSL space cordinates, where luminance
        /// ranges from black to white. Both the saturation and the luminance are 
        /// clamped to the range of zero to one.
        /// </summary>
        /// <param name="hue">Angular hue, ranging from 0 to 360 degrees</param>
        /// <param name="sat">Saturation ranging from zero to one</param>
        /// <param name="lum">Luminosity ranging from zero to on</param>
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
        /// <exception cref="VectorLengthExcp">If the vector is not the proper length</exception>
        /// <returns>The new color</returns>
        public static Color FromHSL(Vector hsl)
        {
            //makes shure the length of the vector is exactly 3
            VectorLengthExcp.Check(hsl, 3);

            //calls the method above
            return FromHSL(hsl[0], hsl[1], hsl[2]);
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
        /// <exception cref="VectorLengthExcp">If the vector is not the proper length</exception>
        /// <returns>The new color</returns>
        public static Color FromYUV(Vector yuv)
        {
            //makes shure the length of the vector is exactly 3
            VectorLengthExcp.Check(yuv, 3);

            //calls the method above
            return FromYUV(yuv[0], yuv[1], yuv[2]);
        }

        #endregion //////////////////////////////////////////////////////////////////

        #region Conversion Methods...

        /// <summary>
        /// Generates a representaiton of the color as a vector in the RGB color
        /// space. This is usefull for treating colors as a mathmatical construct.
        /// </summary>
        /// <returns>The color as a vector</returns>
        public Vector ToRGB()
        {
            return new Vector(red, green, blue);
        }

        /// <summary>
        /// Generates a representaiton of the color as a vector in the RGBA color
        /// space. This is usefull for treating colors as a mathmatical construct.
        /// </summary>
        /// <returns>The color as a vector</returns>
        public Vector ToRGBA()
        {
            return new Vector(red, green, blue, alpha);
        }

        /// <summary>
        /// Generates a representaiton of the color as a vector in the HSL color
        /// space, not to be confused with the HSV color space. This is usefull 
        /// when one needs a more perceptual definiton of color.
        /// </summary>
        /// <returns>The color as a vector</returns>
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
        /// <returns>The color as a vector</returns>
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

        #endregion //////////////////////////////////////////////////////////////////

        #region Color Operations...

        /// <summary>
        /// Sets the colors gama value by scaling each component by the apropriate amount.
        /// This is useful for doing gama correction on an image or output device. Note
        /// that the transformation is inherently non-liniar.
        /// </summary>
        /// <param name="gamma">The new gamma value</param>
        /// <exception cref="ArgBoundsException">If the gamma value falls outside
        /// the range of one-fourth to four</exception>
        /// <returns>The color with corrected gamma</returns>
        public Color SetGamma(double gamma)
        {
            //checks that the gamma value is within a reasonable range
            ArgBoundsException.Check("gamma", gamma, 0.25, 4.0);

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
            if (alpha == 0.0) return this;

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
        /// <param name="gm">Conversion method to use</param>
        /// <returns>The corisponding grey value</returns>
        public Color GetGrayscale(Desaturate gm)
        {
            //initialises luminace to an error value
            double lum = -1.0;

            switch (gm)
            {
                case Desaturate.Average:
                    lum = (red + green + blue) / 3.0;
                    break;
                case Desaturate.Maximum:
                    lum = VMath.Max(red, green, blue);
                    break;
                case Desaturate.Minimum:
                    lum = VMath.Min(red, green, blue);
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
        /// <exception cref="ArgBoundsException">If the interpoland falls outside
        /// the range of zero to one, inclusive</exception>
        /// <returns>The interpolated color</returns>
        public static Color Lerp(Color c1, Color c2, double x)
        {
            //checks that the interpolent is bracketed correctly
            //ArgBoundsException.Check("x", x, 0.0, 1.0);
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

        #endregion //////////////////////////////////////////////////////////////////

        #region Class Conversions...

        //allows implicit conversion to a vector without loss of data
        public static implicit operator Vector(Color c)
        { return c.ToRGBA(); }

        //allows explicit conversion from a vector with potential loss of data
        public static explicit operator Color(Vector v)
        { return Color.FromRGB(v); }

        #endregion //////////////////////////////////////////////////////////////////


    }

}
