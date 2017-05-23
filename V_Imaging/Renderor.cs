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
        private int num_samples;



        //determins how the texture should be scaled to fit
        private Scaling scale = Scaling.Vertical;

        //determins if anti-ailising should be preformed
        private bool aa_flag = true;

        //paramaters that describe the anti-ailising
        private Window win = Window.Lanczos;
        private double sup = 1.0;


        /// <summary>
        /// Creates a basic, no-frills renderor that dosent provide any
        /// Anti-Alising and suports only a generic PRNG.
        /// </summary>
        public Renderor()
        {
            rng = new RandXOR();
            method = AntiAilis.None;
            num_samples = 4;
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
            this.num_samples = samp;
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
            this.num_samples = samp;
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
            this.num_samples = samp;
        }

        #endregion ////////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// DEPRECATED
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
            get { return num_samples; }
        }


        /// <summary>
        /// Determins weather or not the renderor should preform anti-ailising
        /// on the rendered texture. The nature of the anti-ailising tenique
        /// used, is determined by the other properties.
        /// </summary>
        public bool AitiAilising
        {
            get { return aa_flag; }
            set { aa_flag = value; }
        }


        /// <summary>
        /// Determins how the source texture should be scaled, or streched, to
        /// fit the target image. See the Scaling enumeration for more details.
        /// </summary>
        public Scaling Scaling
        {
            get { return scale; }
            set { scale = value; }
        }

        /// <summary>
        /// Selects the windowing function used to determin the weights of each
        /// sub-sample, when computing the final color value for a given pixel.
        /// This property has no effect if anti-ailising is disabled.
        /// </summary>
        public Window Window
        {
            get { return win; }
            set { win = value; }
        }

        /// <summary>
        /// Indicates the diamater of the sampeling area, measured in pixels.
        /// Higher numbers produce smoother edges, but blurier images overall.
        /// This has no effect if anti-ailising is disabled.
        /// </summary>
        public double Support
        {
            get 
            { 
                return sup; 
            }
            set 
            {
                if (value < 1.0) sup = 1.0;
                else sup = value;
            }
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
                output[x, y] = RenderPixel(t, x, y, w, h);
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
                yield return RenderPixel(t, x, y, width, height);
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
            ////grabs the boundries of the pixel
            //double u0 = x / w;
            //double v0 = y / h;
            //double u1 = (x + 1.0) / w;
            //double v1 = (y + 1.0) / h;

            //grabs the boundries of the pixel
            Point2D p0 = ToTexture(x, y, w, h);
            Point2D p1 = ToTexture(x + 1, y + 1, w, h);

            Vector temp = new Vector(4);

            //takes the sum of all the random points
            for (int i = 0; i < num_samples; i++)
            {
                double u = rng.RandDouble(p0.X, p1.X);
                double v = rng.RandDouble(p0.Y, p1.Y);
                temp += t.Sample(u, v);
            }

            //returns the 'average' color
            return Color.FromRGB(temp / num_samples);
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
            int rc = (int)Math.Ceiling(Math.Sqrt(num_samples));

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
                    double u0 = (x + ((double)i / rc));  // w;
                    double v0 = (y + ((double)j / rc));  // h;

                    Point2D p0 = ToTexture(u0, v0, w, h);

                    double u = rng.RandDouble(p0.X, p0.X + us);
                    double v = rng.RandDouble(p0.Y, p0.Y + vs);
                    temp += t.Sample(u, v);
                }
            }

            //for (int i = 0; i < rc; i++)
            //{
            //    for (int j = 0; j < rc; j++)
            //    {
            //        double u0 = (double)i / rc;
            //        double u1 = (double)(i + 1) / rc;

            //        double v0 = (double)j / rc;
            //        double v1 = (double)(j + 1) / rc;

            //        u = rng.RandDouble(u0, u1);
            //        v = rng.RandDouble(v0, v1);
            //        yield return new Point2D(u, v);
            //    }
            //}


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
            Point2D[] points = new Point2D[num_samples];
            double mindist = 0.75 / Math.Sqrt(num_samples);

            //generates a random point to start with
            double u = rng.NextDouble();
            double v = rng.NextDouble();
            points[0] = new Point2D(u, v);

            int i = 1;
            int z = 0;

            while ((i < num_samples) && (z < 100))
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


        /// <summary>
        /// Generates the color of a single pixel in the target image, given the 
        /// texture to render, the locaiton of the pixel, and the dimentions of 
        /// the target image. It dose this by generating multiple samples around
        /// the centr of the pixel, and computing a weighted average based on some
        /// windowing function. This method could be considered the core of the
        /// rendor class.
        /// </summary>
        /// <param name="t">Texture to be rencered</param>
        /// <param name="x">X cordinate of the pixel to render</param>
        /// <param name="y">Y cordinate of the pixel to render</param>
        /// <param name="w">Width of the target image</param>
        /// <param name="h">Height of the target image</param>
        /// <returns>The final color of the given pixel in the rendered image</returns>
        private Color RenderPixel(Texture t, double x, double y, double w, double h)
        {
            if (method == AntiAilis.None)
            {
                //samples only the center of the pixel
                Point2D p = ToTexture(x, y, w, h);
                return t.Sample(p.X, p.Y);
            }

            //generates the sub-samples for the pixel
            var samples = GenSamples();

            //used in computing the pixel's color
            Vector sum = new Vector(4);
            double range = sup * 0.5;
            double weight = 0.0;

            foreach (Point2D sample in samples)
            {
                range = 1.5;
                double dx = (sample.X * range) + x;
                double dy = (sample.Y * range) + y;
                double dw = sample.Radius;

                //only consider samples within the unit disk
                if (dw >= 1.0) continue;

                //computes the weight from the windowing funciton
                dw = CalWeight(dw);


                Point2D p = ToTexture(dx, dy, w, h);
                Vector temp = (Vector)t.Sample(p.X, p.Y);

                sum = sum + temp * dw;
                weight = weight + dw;
            }

            //returns the 'average' color
            return Color.FromRGB(sum / weight);
        }

        /// <summary>
        /// Converts pixel and sub-pixel cordinates to UV texture cordinates,
        /// using the scaling method prefered by the curent renderor.
        /// </summary>
        /// <param name="x">X cordinate of the Pixel</param>
        /// <param name="y">Y cordinate of the Pixel</param>
        /// <param name="w">Width of the render target</param>
        /// <param name="h">Height of the render target</param>
        /// <returns>The corisponding UV texture cordinates</returns>
        private Point2D ToTexture(double x, double y, double w, double h)
        {
            //determins the scaling factor in the X and Y direction
            double sx = (scale == Scaling.Vertical) ? h : w;
            double sy = (scale == Scaling.Horizontal) ? w : h;

            //scales the texture to fit the target image
            double u = ((2.0 * (x + 0.5)) - w) / sx;
            double v = ((2.0 * (y + 0.5)) - h) / sy;

            return new Point2D(u, -v);
        }

        /// <summary>
        /// Calculates the weight for a given sample, given its distance from 
        /// the center of the sample set. It uses the curently selected window
        /// function to calculate the weights.
        /// </summary>
        /// <param name="rad">Distance from the center</param>
        /// <returns>The weight of the given sample</returns>
        private double CalWeight(double rad)
        {
            //calculates the weight based on the window function
            switch (win)
            {
                case Window.Box: 
                    return 1.0;
                case Window.Tent: 
                    return 1.0 - rad;
                case Window.Cosine:
                    return Math.Cos(Math.PI / 2.0 * rad);
                case Window.Gausian:
                    return Math.Exp(-2.0 * rad * rad);
                case Window.Sinc:
                    return VMath.Sinc(Math.PI * rad);
                case Window.Lanczos:
                    double temp = VMath.Sinc(Math.PI * rad);
                    return temp * temp;
            }

            //only the windowing funcitos above are suported
            throw new NotSupportedException();
        }

        #region Sample Gemeration...

        private IEnumerable<Point2D> GenSamples()
        {
            switch (method)
            {
                case AntiAilis.Random: return RandomPoints();
                case AntiAilis.Jittred: return JittredPoints();          
            }

            //defaults to using random points
            return RandomPoints();
        }


        private IEnumerable<Point2D> RandomPoints()
        {
            for (int i = 0; i < num_samples; i++)
            {
                double u = rng.RandDouble(-1.0, 1.0);
                double v = rng.RandDouble(-1.0, 1.0);
                yield return new Point2D(u, v);
            }
        }

        private IEnumerable<Point2D> JittredPoints()
        {
            double u, v;

            double rc = Math.Ceiling(Math.Sqrt(num_samples));
            double inv = 1.0 / rc;

            ////TEST: Predetermined samples
            //rng.Reset();

            //selects a random point inside each cell
            for (int i = 0; i < rc; i++)
            {
                for (int j = 0; j < rc; j++)
                {
                    double u0 = (double)i / rc;
                    double v0 = (double)j / rc;

                    u = rng.RandDouble(u0, u0 + inv);
                    v = rng.RandDouble(v0, v0 + inv);

                    u = (u * 2.0) - 1.0;
                    v = (v * 2.0) - 1.0;

                    yield return new Point2D(u, v);
                }
            }
        }

        #endregion ////////////////////////////////////////////////////////////////


        /*
         * Consider the windowed sinc functions:
         * 
         * Sinc Functions [0 .. 3]
         * sinc(pi * x / 3) * sinc(pi * x) : Lancaoze 
         * e^(-1/4 * x^2) * sinc(pi * x) : Gausian
         * cos(pi * x / 6) * sinc(pi * x) : Cosine
         * 
         * Gause Funcitons [0 .. 2]
         * cos(pi * x / 4) * e^(-2x^2) : GauseCosine
         * sinc(pi * x / 2) * e^(-2x^2) : GauseLancoze
         * 
         */

    }
}
