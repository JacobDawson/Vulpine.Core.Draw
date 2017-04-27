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
    /// <remarks>Last Update: 2016-02-16</remarks>
    public class Interpolent : Texture
    {
        #region Class Definitions...

        //stores the image to be interpolated
        private Image raster;

        //refrences the method of interpolation
        private Intpol method;

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
        /// Extracts the color data from the texture at an explicit point,
        /// in texture cordinates, where the principle width and height of
        /// the texture are equal to one.
        /// </summary>
        /// <param name="u">The u texture cordinate</param>
        /// <param name="v">The v texture cordinate</param>
        /// <returns>The color sampled at the given point</returns>
        public Color GetValue(double u, double v)
        {
            //scales the UV cordinates as appropriate
            double x = u * raster.Width;
            double y = v * raster.Height;

            //obtains the desired sub-pixel
            return GetSubPixel(x, y);
        }

        /// <summary>
        /// Allows for sub-pixel access into the internal image. This is diffrent
        /// from standard texturer cordinates, as each pixel corisponds to a single
        /// unit in the cordinate space.
        /// </summary>
        /// <param name="x">The x pixel cordinate</param>
        /// <param name="y">The y pixel cordinate</param>
        /// <returns>The color sampled at the given point</returns>
        public Color GetSubPixel(double x, double y)
        {
            //determins which method to use
            switch (method)
            {
                case Intpol.Nearest: return GetNearest(x, y);
                case Intpol.BiLiniar: return GetBiLiniar(x, y);
                case Intpol.BiCubic: return GetBicubic(x, y);
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

            //gets the piels at each of the four corners of the square
            Color pix00 = GetPixel(x0, y0);
            Color pix01 = GetPixel(x0, y0 + 1);
            Color pix10 = GetPixel(x0 + 1, y0);
            Color pix11 = GetPixel(x0 + 1, y0 + 1);

            Color m0 = Color.Lerp(pix00, pix10, xs);
            Color m1 = Color.Lerp(pix01, pix11, xs);
            Color fin = Color.Lerp(m0, m1, ys);

            return fin;
        }

        /// <summary>
        /// Preformes bi-cubic interpolation.
        /// </summary>
        /// <param name="x">The x coridnate</param>
        /// <param name="y">The y coridnate</param>
        /// <returns>The interpolated color</returns>
        private Color GetBicubic(double x, double y)
        {
            //computes the upper-left pixel of the bounding square
            int x1 = (int)Math.Floor(x - 0.5);
            int y1 = (int)Math.Floor(y - 0.5);

            //computes our location within the square
            double xs = x - (x1 + 0.5);
            double ys = y - (y1 + 0.5);
           
            //collects the neighboring grid of 16 points
            Vector pix00 = GetPixel(x1 - 1, y1 - 1);
            Vector pix01 = GetPixel(x1 - 1, y1);
            Vector pix02 = GetPixel(x1 - 1, y1 + 1);
            Vector pix03 = GetPixel(x1 - 1, y1 + 2);

            Vector pix10 = GetPixel(x1, y1 - 1);
            Vector pix11 = GetPixel(x1, y1);
            Vector pix12 = GetPixel(x1, y1 + 1);
            Vector pix13 = GetPixel(x1, y1 + 2);

            Vector pix20 = GetPixel(x1 + 1, y1 - 1);
            Vector pix21 = GetPixel(x1 + 1, y1);
            Vector pix22 = GetPixel(x1 + 1, y1 + 1);
            Vector pix23 = GetPixel(x1 + 1, y1 + 2);

            Vector pix30 = GetPixel(x1 + 2, y1 - 1);
            Vector pix31 = GetPixel(x1 + 2, y1);
            Vector pix32 = GetPixel(x1 + 2, y1 + 1);
            Vector pix33 = GetPixel(x1 + 2, y1 + 2);

            //interpolates over the x-axis
            Vector m0 = Cubic(pix00, pix10, pix20, pix30, xs);
            Vector m1 = Cubic(pix01, pix11, pix21, pix31, xs);
            Vector m2 = Cubic(pix02, pix12, pix22, pix32, xs);
            Vector m3 = Cubic(pix03, pix13, pix23, pix33, xs);

            //finishes interpolating over the y-axis
            Vector fin = Cubic(m0, m1, m2, m3, ys);
            return Color.FromRGB(fin);        
        }

        /// <summary>
        /// Preforms Sinc3 (Lanczos) Interpolation.
        /// </summary>
        /// <param name="x">The x coridnate</param>
        /// <param name="y">The y coridnate</param>
        /// <returns>The interpolated color</returns>
        private Color GetSinc3(double x, double y)
        {
            int x0 = (int)Math.Floor(x - 0.5);
            int y0 = (int)Math.Floor(y - 0.5);

            //used in caluclating the interpolated color
            Vector temp = new Vector(4);

            //itterates over all contributing points
            for (int i = x0 - 2; i <= x0 + 3; i++)
            {
                for (int j = y0 - 2; j <= y0 + 3; j++)
                {
                    Vector color = GetPixel(i, j);
                    double s = Lanczos(x - i - 0.5) * Lanczos(y - j - 0.5);
                    temp += (color * s);
                }
            }

            //returns the caluclated color
            return Color.FromRGB(temp);
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
        /// Helper method that preforms cubic interpolation on a set of vectors.
        /// </summary>
        /// <param name="p0">First vector point</param>
        /// <param name="p1">Second vector point</param>
        /// <param name="p2">Third vector point</param>
        /// <param name="p3">Fourth vector point</param>
        /// <param name="x">Point of interploation</param>
        /// <returns>The interpolated vector</returns>
        private Vector Cubic(Vector p0, Vector p1, Vector p2, Vector p3, double x)
        {          
            Vector calc = (3.0 * (p1 - p2)) + p3 - p0;
            calc = (x * calc) + (2.0 * p0) - (5.0 * p1) + (4.0 * p2) - p3;
            calc = (0.5 * x * ((x * calc) - p0 + p2)) + p1; 

            return calc; 
        }

        /// <summary>
        /// Computes the Lancozos Windoing Function.
        /// </summary>
        /// <param name="x">Input to the windoing function</param>
        /// <returns>Evaluation of the windowing function</returns>
        private double Lanczos(double x)
        {
            if (Math.Abs(x) > 3.0)
            {
                return 0.0;
            }
            else
            {
                double temp = x * Math.PI;
                temp = VMath.Sinc(temp) * VMath.Sinc(temp / 3.0);
                return temp;
            }
        }

        #endregion ///////////////////////////////////////////////////////////////////////
    }

}
