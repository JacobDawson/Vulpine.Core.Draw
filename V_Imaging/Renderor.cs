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
using Vulpine.Core.Calc.Geometry;
using Vulpine.Core.Calc.Matrices;
using Vulpine.Core.Calc.RandGen;

namespace Vulpine.Core.Draw
{

    /// <summary>
    /// The purpous of Rendering is to convert a continious texture into a rasterised
    /// image of pixels. This is nessary, for instance, to print out textures or
    /// display them to the screen. Rendering works on a pixel by pixel basis, and
    /// may imploy anti-alising teniques inorder to acheve a more acurate value for
    /// each pixel. This is in contrast to Rasterisation, which operates on a perimitive
    /// by perimitive basis, and requires a more detailed description.
    /// </summary>
    /// <remarks>Last Update: 2016-02-14</remarks>
    public class Renderor
    {
        #region Class Definitons...


        //IMPORTANT: I still need to update the various AA methods to use the new
        //texture cordinate system. Curently only AA.None works as intended.


        //uses a PRNG for anti-aliasing
        private VRandom rng;

        //refrences the subsampeling method used
        private AntiAilis method;
        private int samples;


        private Scaling scale = Scaling.Vertical;

        /// <summary>
        /// Creates a basic, no-frills renderor that dosent provide any
        /// Anti-Alising and suports only a generic PRNG.
        /// </summary>
        public Renderor()
        {
            rng = new RandXOR();
            method = AntiAilis.None;
            samples = 4;
        }

        /// <summary>
        /// Creates a new renderor utilising the desired Anti-Aliasing method,
        /// with the desired number of samples. A generic PRNG is provided
        /// for the generation of sub-samples.
        /// </summary>
        /// <param name="meth">Anti-Aliasing  method to use</param>
        /// <param name="samp">Number of sub-samples per pixel</param>
        /// <exception cref="ArgRangeExp">If the number of samples is 
        /// less than four</exception>
        public Renderor(AntiAilis meth, int samp)
        {
            //checks that our sample size is apropriate
            ArgRangeExcp.Atleast("samp", samp, 4);

            this.rng = new RandXOR();
            this.method = meth;
            this.samples = samp;
        }

        /// <summary>
        /// Creates a new renderor utilising the desired Anti-Aliasing method,
        /// with the desired number of samples. A generic PRNG is intialised
        /// with the given seed for generating sub-samples.
        /// </summary>
        /// <param name="meth">Anti-Aliasing  method to use</param>
        /// <param name="samp">Number of sub-samples per pixel</param>
        /// <param name="seed">Seed of the internal PRNG</param>
        /// <exception cref="ArgRangeExp">If the number of samples is 
        /// less than four</exception>
        public Renderor(AntiAilis meth, int samp, int seed)
        {
            //checks that our sample size is apropriate
            ArgRangeExcp.Atleast("samp", samp, 4);

            this.rng = new RandXOR(seed);
            this.method = meth;
            this.samples = samp;
        }

        /// <summary>
        /// Creates a new renderor utilising the desired Anti-Aliasing method,
        /// with the desired number of samples. The PRNG used to generate
        /// sub-samples is given by the invoker.
        /// </summary>
        /// <param name="meth">Anti-Aliasing  method to use</param>
        /// <param name="samp">Number of sub-samples per pixel</param>
        /// <param name="rng">Reference to the internal PRNG</param>
        /// <exception cref="ArgRangeExp">If the number of samples is 
        /// less than four</exception>
        public Renderor(AntiAilis meth, int samp, VRandom rng)
        {
            //checks that our sample size is apropriate
            ArgRangeExcp.Atleast("samp", samp, 4);

            this.rng = rng;
            this.method = meth;
            this.samples = samp;
        }

        #endregion ////////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// Represents the curent AntiAliasing method in use by the Renderor.
        /// </summary>
        public AntiAilis Method
        {
            get { return method; }
        }

        /// <summary>
        /// Represents the ideal number of sub-samples to generate per pixel.
        /// The actual number of sub-samples generated may be less.
        /// </summary>
        public int Samples
        {
            get { return samples; }
        }

        #endregion ////////////////////////////////////////////////////////////////

        #region Rendering...

        /// <summary>
        /// Renders the texture to the output image, overwriting any data in
        /// the output image in the process. 
        /// </summary>
        /// <param name="t">Texture to be rendered</param>
        /// <param name="output">Output image to hold the rendering</param>
        /// <exception cref="ReadOnlyExcp">If the output image is
        /// marked as read-only</exception>
        public void Render(Texture t, Image output)
        {
            //checks that the image is wrightable
            if (output.IsReadOnly) throw new Exception();

            double w = output.Width;
            double h = output.Height;

            for (int y = 0; y < output.Height; y++)
            {
                for (int x = 0; x < output.Width; x++)
                output[x, y] = GetSampled(t, w, h, x, y);
            }
        }

        /// <summary>
        /// Renders the texture one pixel at a time, in a top-down fassion. This
        /// allows for aditional processing to occor, sutch as reporting the
        /// progress on long running renders.
        /// </summary>
        /// <param name="t">Texture to be rendered</param>
        /// <param name="width">Widht of the output image</param>
        /// <param name="height">Height of the output image</param>
        /// <returns>Each pixel in the image</returns>
        /// <exception cref="ArgRangeExcp">If either the width or the
        /// height is less than one</exception>
        public IEnumerable<Color> Render(Texture t, int width, int height)
        {
            //checks that the dementions are positive
            ArgRangeExcp.Atleast("width", width, 1);
            ArgRangeExcp.Atleast("height", height, 1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                yield return GetSampled(t, width, height, x, y);
            }
        }

        #endregion ////////////////////////////////////////////////////////////////

        #region Sub-Sampeling...

        /// <summary>
        /// Generates sub-samples, per the anti-ailising method of the renderor,
        /// and returns theire average value as the color of the pixel.
        /// </summary>
        /// <param name="t">Texture to sample</param>
        /// <param name="w">Width of the render target</param>
        /// <param name="h">Height of the render target</param>
        /// <param name="x">X cordinate of the pixel</param>
        /// <param name="y">Y cordinate of the pixel</param>
        /// <returns>The average color</returns>
        private Color GetSampled(Texture t, double w, double h, int x, int y)
        {
            switch (method)
            {
                case AntiAilis.None:
                    return GetCenter(t, w, h, x, y);
                case AntiAilis.Random:
                    return GetRandom(t, w, h, x, y);
                case AntiAilis.Jittred:
                    return GetJittred(t, w, h, x, y);
                case AntiAilis.Poisson:
                    return GetPoisson(t, w, h, x, y);
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Maps each pixel in the image to it's center
        /// </summary>
        /// <param name="t">Texture to sample</param>
        /// <param name="w">Width of the render target</param>
        /// <param name="h">Height of the render target</param>
        /// <param name="x">X cordinate of the pixel</param>
        /// <param name="y">Y cordinate of the pixel</param>
        /// <returns>The average color</returns>
        private Color GetCenter(Texture t, double w, double h, int x, int y)
        {
            //double u = (x + 0.5) / w;
            //double v = (y + 0.5) / h;

            Point2D p = ToTexture(x + 0.5, y + 0.5, w, h);
            return t.Sample(p.X, p.Y);
        }

        /// <summary>
        /// Generates subsamples at random and returns the average color
        /// for the given pixel
        /// </summary>
        /// <param name="t">Texture to sample</param>
        /// <param name="w">Width of the render target</param>
        /// <param name="h">Height of the render target</param>
        /// <param name="x">X cordinate of the pixel</param>
        /// <param name="y">Y cordinate of the pixel</param>
        /// <returns>The average color</returns>
        private Color GetRandom(Texture t, double w, double h, int x, int y)
        {
            //grabs the boundries of the pixel
            double u0 = x / w;
            double v0 = y / h;
            double u1 = (x + 1.0) / w;
            double v1 = (y + 1.0) / h;

            Vector temp = new Vector(4);

            //takes the sum of all the random points
            for (int i = 0; i < samples; i++)
            {
                double u = rng.RandDouble(u0, u1);
                double v = rng.RandDouble(v0, v1);
                temp += t.Sample(u, v);
            }

            //returns the 'average' color
            return Color.FromRGB(temp / samples);
        }

        /// <summary>
        /// Generates subsamples in a jittred pattern, and returns the
        /// average color for the given pixel
        /// </summary>
        /// <param name="t">Texture to sample</param>
        /// <param name="w">Width of the render target</param>
        /// <param name="h">Height of the render target</param>
        /// <param name="x">X cordinate of the pixel</param>
        /// <param name="y">Y cordinate of the pixel</param>
        /// <returns>The average color</returns>
        private Color GetJittred(Texture t, double w, double h, int x, int y)
        {
            Vector temp = new Vector(4);
            int rc = (int)Math.Floor(Math.Sqrt(samples));

            //TEST: Make all pixels use the same samples:
            rng.Reset();

            //calcualtes the width and height of each cell
            double us = 1.0 / (w * rc);
            double vs = 1.0 / (h * rc);

            //selects a random point inside each cell
            for (int i = 0; i < rc; i++)
            {
                for (int j = 0; j < rc; j++)
                {
                    double u0 = (x + ((double)i / rc)) / w;
                    double v0 = (y + ((double)j / rc)) / h;

                    double u = rng.RandDouble(u0, u0 + us);
                    double v = rng.RandDouble(v0, v0 + vs);
                    temp += t.Sample(u, v);
                }
            }

            //returns the 'average' color
            return Color.FromRGB(temp / (rc * rc));
        }

        /// <summary>
        /// Generates subsamples in a poisson distribution, and returns the
        /// average color for the given pixel.
        /// </summary>
        /// <param name="t">Texture to sample</param>
        /// <param name="w">Width of the render target</param>
        /// <param name="h">Height of the render target</param>
        /// <param name="x">X cordinate of the pixel</param>
        /// <param name="y">Y cordinate of the pixel</param>
        /// <returns>The average color</returns>
        private Color GetPoisson(Texture t, double w, double h, int x, int y)
        {
            Point2D[] points = new Point2D[samples];
            double mindist = 0.75 / Math.Sqrt(samples);

            //generates a random point to start with
            double u = rng.NextDouble();
            double v = rng.NextDouble();
            points[0] = new Point2D(u, v);

            int i = 1;
            int z = 0;

            while ((i < samples) && (z < 100))
            {
                //generates a random canidate point
                u = rng.NextDouble();
                v = rng.NextDouble();
                Point2D can = new Point2D(u, v);
                bool pass = true;

                z++;

                //checks that the canidate is not too close
                for (int k = 0; k < i; k++)
                {
                    pass = (can.Dist(points[k]) > mindist);
                    if (!pass) break;
                }

                //includes the point only if it passes
                if (pass)
                {
                    points[i] = can;
                    i++; z = 0;
                }
            }

            Vector temp = new Vector(4);

            //converts each point to the range we need after the fact
            for (int k = 0; k < i; k++)
            {
                u = (points[k].X + x) / w;
                v = (points[k].Y + y) / h;
                temp += t.Sample(u, v);
            }

            //returns the 'average' color
            return Color.FromRGB(temp / i);
        }

        #endregion ////////////////////////////////////////////////////////////////


        private Point2D ToTexture(double x, double y, double w, double h)
        {
            //double sx = 1.0;
            //double sy = 1.0;

            //switch (scale)
            //{
            //    case Scaling.Horizontal:
            //        sx = w;
            //        sy = w;
            //        break;
            //    case Scaling.Vertical:
            //        sx = h;
            //        sy = h;
            //        break;
            //    case Scaling.Streach:
            //        sx = w;
            //        sy = h;
            //        break;
            //}

            //determins the scaling factor in the X and Y direction
            double sx = (scale == Scaling.Vertical) ? h : w;
            double sy = (scale == Scaling.Horizontal) ? w : h;

            //scales the texture to fit the target image
            double u = ((2.0 * x) - w) / sx;
            double v = ((2.0 * y) - h) / sy;

            return new Point2D(u, -v);
        }


    }
}
