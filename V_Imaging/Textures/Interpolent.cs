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

using Vulpine.Core.Data.Exceptions;
using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Matrices;

namespace Vulpine.Core.Draw.Textures
{
    /// <summary>
    /// By compbining a raster Image with an Interpolation scheme, one generates a continious
    /// Texture. This is how simple Images are able to be treated as Textures whenever
    /// sub-pixel precision is required.
    /// </summary>
    /// <remarks>Last Update: 2017-10-20</remarks>
    public class Interpolent : Texture
    {
        //NOTE: I should add an option to mask the texture, as well as 
        //an option to do gamma corrected interpolaiton.

        #region Class Definitions...

        //stores the image to be interpolated
        private Image raster;

        //refrences the method of interpolation
        private Intpol method;

        //determins the maximum scale of the image
        private double smax;

        //determins if the image should be tiled
        private bool tiled;

        /// <summary>
        /// Constructs a new Interpolated Texture, using the given source
        /// image and the default interpolation method to generate it's data.
        /// </summary>
        /// <param name="raster">Source image to interpolate</param>
        public Interpolent(Image raster)
        {
            this.raster = raster;
            this.method = Intpol.Default;          
            this.tiled = false;

            this.smax = Math.Min(raster.Width, raster.Height);
        }

        /// <summary>
        /// Constructs a new Interpolated Texture, using the given image and
        /// interpolation method to generate it's data.
        /// </summary>
        /// <param name="raster">Source image to interpolate</param>
        /// <param name="method">Method used for interpolation</param>
        public Interpolent(Image raster, Intpol method)
        {
            this.raster = raster;
            this.method = method;
            this.tiled = false;

            this.smax = Math.Min(raster.Width, raster.Height);
        }

        /// <summary>
        /// Constructs a new Interpolated Texture, using the given image and
        /// interpolation method to generate it's data. It will tile the
        /// image indefinatly if tiled is set to true.
        /// </summary>
        /// <param name="raster">Source image to interpolate</param>
        /// <param name="method">Method used for interpolation</param>
        /// <param name="tiled">Set to true for tiled images</param>
        public Interpolent(Image raster, Intpol method, bool tiled)
        {
            this.raster = raster;
            this.method = method;
            this.tiled = tiled;

            this.smax = Math.Min(raster.Width, raster.Height);
        }

        #endregion ///////////////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// The width of the current texture.
        /// </summary>
        public int Width 
        {
            get { return raster.Width; }
        }

        /// <summary>
        /// The height of the current texture.
        /// </summary>
        public int Height 
        {
            get { return raster.Height; } 
        }

        /// <summary>
        /// Indicates the interpolation method in use for this particular texture.
        /// </summary>
        public Intpol Method
        {
            get { return method; }
        }

        /// <summary>
        /// Determins if the curent texture is tileable
        /// </summary>
        public bool Tiled
        {
            get { return tiled; }
        }

        #endregion ///////////////////////////////////////////////////////////////////////

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
            //scales the UV cordinates as appropriate
            double x = (1.0 + u) * raster.Width * 0.5;
            double y = (1.0 - v) * raster.Height * 0.5;

            ////scales the UV cordinates as appropriate
            //double x = ((double)raster.Width + smax * u) * 0.5;
            //double y = ((double)raster.Height - smax * v) * 0.5;

            //obtains the desired sub-pixel
            return GetSubPixel(x, y);
        }

        /// <summary>
        /// Interpolates the values between pixels in the source data inorder
        /// to procude a continious image. Here the XY cordinates used corispond to
        /// the pixels of the source image, where the origin is in the top left
        /// corner, and a distance of one is equivlent to one pixel.
        /// </summary>
        /// <param name="x">The x pixel cordinate</param>
        /// <param name="y">The y pixel cordinate</param>
        /// <returns>The color sampled at the given point</returns>
        public Color GetSubPixel(double x, double y)
        {
            //this implements masking
            Color trans = new Color(0.0, 0.0, 0.0, 0.0);
            if (x < 0.0 || x > raster.Width) return trans;
            if (y < 0.0 || y > raster.Height) return trans;

            //determins which method to use
            switch (method)
            {
                case Intpol.Nearest: return GetNearest(x, y);
                case Intpol.BiLiniar: return GetBiLiniar(x, y);
                case Intpol.BiCubic: return GetBicubic(x, y, Spline);
                case Intpol.Catrom: return GetBicubic(x, y, Catrom);
                case Intpol.Mitchel: return GetBicubic(x, y, Mitchel);
                case Intpol.Sinc3: return GetSinc3(x, y);
            }

            //only the methods listed above are suported
            throw new NotSupportedException();
        }

        #endregion ///////////////////////////////////////////////////////////////////////

        #region Image Interpolation...

        /// <summary>
        /// Preforms nearest neighbor interpolation.
        /// </summary>
        /// <param name="x">The x coridnate</param>
        /// <param name="y">The y coridnate</param>
        /// <returns>The interpolated color</returns>
        private Color GetNearest(double x, double y)
        {
            //finds the nearest pixel to our sample point
            int xs = (int)Math.Floor(x);
            int ys = (int)Math.Floor(y);

            return GetPixel(xs, ys);
        }

        /// <summary>
        /// Preforms bi-liniar interpolation.
        /// </summary>
        /// <param name="x">The x coridnate</param>
        /// <param name="y">The y coridnate</param>
        /// <returns>The interpolated color</returns>
        private Color GetBiLiniar(double x, double y)
        {
            //computes the upper-left pixel of the bounding square
            int x0 = (int)Math.Floor(x - 0.5);
            int y0 = (int)Math.Floor(y - 0.5);

            //computes our location within the square
            double xs = x - (x0 + 0.5);
            double ys = y - (y0 + 0.5);

            //gets the pixels at each of the four corners of the square
            Color pix00 = GetPixel(x0, y0);
            Color pix01 = GetPixel(x0, y0 + 1);
            Color pix10 = GetPixel(x0 + 1, y0);
            Color pix11 = GetPixel(x0 + 1, y0 + 1);

            //interpolates first in the x direction then y direction
            Color m0 = Color.Lerp(pix00, pix10, xs);
            Color m1 = Color.Lerp(pix01, pix11, xs);
            Color fin = Color.Lerp(m0, m1, ys);

            return fin;
        }

        /// <summary>
        /// Preforms bi-cubic interpolation, using the given filter function
        /// to weight the samples generated by the algorythim.
        /// </summary>
        /// <param name="x">The x coridnate</param>
        /// <param name="y">The y coridnate</param>
        /// <param name="f">Filter Funciton</param>
        /// <returns>The interpolated color</returns>
        private Color GetBicubic(double x, double y, VFunc f)
        {
            //computes the closest neihboring pixel
            int x0 = (int)Math.Floor(x - 0.5);
            int y0 = (int)Math.Floor(y - 0.5);

            //computes our displacment
            double xs = x - x0 - 0.5;
            double ys = y - y0 - 0.5;

            //used in caluclating the interpolated color
            Vector temp = new Vector(4);

            //itterates over all contributing points
            for (int i = -1; i <= 2; i++)
            {
                for (int j = -1; j <= 2; j++)
                {
                    Vector color = GetPixel(x0 + i, y0 + j);
                    double s = f(xs - i) * f(ys - j);
                    temp += (color * s);
                }
            }

            //returns the caluclated color
            return Color.FromRGBA(temp);
        }

        /// <summary>
        /// Preforms Sinc3 (Lanczos) Interpolation.
        /// </summary>
        /// <param name="x">The x coridnate</param>
        /// <param name="y">The y coridnate</param>
        /// <returns>The interpolated color</returns>
        private Color GetSinc3(double x, double y)
        {
            //computes the closest neihboring pixel
            int x0 = (int)Math.Floor(x - 0.5);
            int y0 = (int)Math.Floor(y - 0.5);

            //computes our displacment
            double xs = x - x0 - 0.5;
            double ys = y - y0 - 0.5;

            //used in caluclating the interpolated color
            Vector temp = new Vector(4);

            //itterates over all contributing points
            for (int i = -2; i <= 3; i++)
            {
                for (int j = -2; j <= 3; j++)
                {
                    Vector color = GetPixel(x0 + i, y0 + j);
                    double s = Lanczos(xs - i) * Lanczos(ys - j);
                    temp += (color * s);
                }
            }

            //returns the caluclated color
            return Color.FromRGBA(temp);
        }

        #endregion ///////////////////////////////////////////////////////////////////////

        #region Helper Methods...

        /// <summary>
        /// Helper method, provides a shorthand notation for accesing
        /// the pixels inside the internal image.
        /// </summary>
        /// <param name="x">The x coridnate</param>
        /// <param name="y">The y coridnate</param>
        /// <returns>The color of the desired pixel</returns>
        private Color GetPixel(int x, int y)
        {
            int dx = Recal(x, raster.Width, tiled);
            int dy = Recal(y, raster.Height, tiled);

            return raster.GetPixel(dx, dy);
        }

        /// <summary>
        /// Helper method, used to recalculate the pixel index when trying
        /// to access data outside the bounds of the image.
        /// </summary>
        /// <param name="x">Value to recalculate</param>
        /// <param name="max">Maximum value the value can take</param>
        /// <param name="rep">True if the values should repeate</param>
        /// <returns>The recaluclated value</returns>
        private int Recal(int x, int max, bool rep)
        {
            int dw = rep ? max : max + max;
            int dx = ((x % dw) + dw) % dw;
            if (dx >= max) dx = dw - dx - 1;

            return dx;
        }

        /// <summary>
        /// Helper method that evaluates the B-Spline filter funciton. 
        /// </summary>
        /// <param name="x">Input to filter funciton</param>
        /// <returns>The value of the filter funciton</returns>
        private double Spline(double x)
        {
            double abs = Math.Abs(x);

            if (abs < 1.0)
            {
                double p = 3.0;
                p = (p * abs) - 6.0;
                p = (p * abs * abs) + 4.0;
                return p / 6.0;
             }
            else if (abs < 2.0)
            {
                double t = (abs - 2.0);
                return -(t * t * t) / 6.0;
            }
            else
            {
                return 0.0;
            }
        }

        /// <summary>
        /// Helper method that evaluates the Catmull–Rom filter funciton.
        /// </summary>
        /// <param name="x">Input to filter funciton</param>
        /// <returns>The value of the filter funciton</returns>
        private double Catrom(double x)
        {
            double abs = Math.Abs(x);

            if (abs < 1.0)
            {
                double p = 9.0;
                p = (p * abs) - 15.0;
                p = (p * abs * abs) + 6.0;
                return p / 6.0;

            }
            else if (abs < 2.0)
            {
                double p = -3.0;
                p = (p * abs) + 15.0;
                p = (p * abs) - 24.0;
                p = (p * abs) + 12.0;
                return p / 6.0;
            }
            else
            {
                return 0.0;
            }
        }

        /// <summary>
        /// Helper method that evaluates the Mitchell-Netravali filter funciton.
        /// </summary>
        /// <param name="x">Input to filter funciton</param>
        /// <returns>The value of the filter funciton</returns>
        private double Mitchel(double x)
        {
            double abs = Math.Abs(x);

            if (abs < 1.0)
            {
                double p = 21.0;
                p = (p * abs) - 36.0;
                p = (p * abs * abs) + 16.0;
                return p / 18.0;

            }
            else if (abs < 2.0)
            {
                double p = -7.0;
                p = (p * abs) + 36.0;
                p = (p * abs) - 60.0;
                p = (p * abs) + 32.0;
                return p / 18.0;
            }
            else
            {
                return 0.0;
            }
        }

        /// <summary>
        /// Computes the Lancozos Window Function.
        /// </summary>
        /// <param name="x">Input to the window function</param>
        /// <returns>Evaluation of the window function</returns>
        private double Lanczos(double x)
        {
            if (Math.Abs(x) > 3.0)
            {
                return 0.0;
            }
            else
            {
                double temp = x * Math.PI;
                temp = VMath.Sinc(temp) * Math.Cos(temp / 6.0);
                return temp;
            }
        }

        #endregion ///////////////////////////////////////////////////////////////////////

    }

}
