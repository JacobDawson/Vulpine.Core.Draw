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
using Vulpine.Core.Calc.Geometry;
using Vulpine.Core.Calc.Matrices;
using Vulpine.Core.Calc.RandGen;

namespace Vulpine.Core.Draw
{
    /// <summary>
    /// The purpous of Rendering is to convert a continious texture into a rasterised
    /// image of pixels. This is nessary, for instance, to save textures to disk or
    /// display them to the screen. Rendering works on a pixel by pixel basis, and
    /// may imploy anti-alising teniques inorder to acheve a more realistic value for
    /// each pixel. Furthermore, a scaling mehtod must be provided to determin how
    /// the texture should be scaled (or streched) to fit the target image.
    /// </summary>
    /// <remarks>Last Update: 2017-05-24</remarks>
    public class Renderor
    {
        #region Class Definitons...

        //uses a PRNG for anti-aliasing
        private VRandom rng;

        //determins how the texture should be scaled to fit
        private Scaling scale = Scaling.Vertical;

        //determins if anti-ailising should be preformed
        private bool aa_flag = true;

        //paramaters that describe the anti-ailising
        private Window win = Window.Gausian;
        private double sup = VMath.R2;
        private bool jitter = true;
        private int nsamp = 4;

        //stores the delegate to handel render events
        private EventHandler<RenderEventArgs> e_render;

        private EventHandler e_start;
        private EventHandler e_finish;

        /// <summary>
        /// Creates a basic, no-frills renderor that dosent provide any
        /// Anti-Alising and suports only a generic PRNG.
        /// </summary>
        public Renderor()
        {
            this.rng = new RandXOR();
            this.aa_flag = false;
        }

        /// <summary>
        /// Creates a new renderor using the given aporximate number of 
        /// sample points per pixel. A generic PRNG is provided for 
        /// generating sample points.
        /// </summary>
        /// <param name="sample">Aprox number of sub-samples</param>
        public Renderor(int sample)
        {
            this.rng = new RandXOR();
            this.nsamp = Math.Max(sample, 4);
        }

        /// <summary>
        /// Creates a new renderor using the given aproximate number of
        /// sample points per pixel. The internal PRNG is initialised
        /// with the given seed for generating sample points.
        /// </summary>
        /// <param name="sample">Aprox number of sub-samples</param>
        /// <param name="seed">Seed for sample generation</param>
        public Renderor(int sample, int seed)
        {
            this.rng = new RandXOR(seed);
            this.nsamp = Math.Max(sample, 4);
        }

        /// <summary>
        /// Creates a new renderor using the given aproximate number of
        /// sample points per pixel. The provided PRNG is used for the
        /// generation of sample points.
        /// </summary>
        /// <param name="sample">Aprox number of sub-samples</param>
        /// <param name="rng">Generator for sub-samples</param>
        public Renderor(int sample, VRandom rng)
        {
            this.rng = rng; //clone this!
            this.nsamp = Math.Max(sample, 4);
        }

        #endregion ////////////////////////////////////////////////////////////////

        #region Class Properties...

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
        /// Determins the aporximate number of samples to generate per pixel.
        /// The more samples generated, the smoother the image, but the longer
        /// the render time. This has no effect if anti-ailising is disabled.
        /// </summary>
        public int Samples
        {
            get { return nsamp; }
            set { nsamp = Math.Max(value, 4); }
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
        /// Indicates the radius of the sampeling area, measured in pixels. 
        /// For instance, a radius of 0.5 would be completly contained inside 
        /// the pixel. Higher numbers produce smoother edges, but blurier 
        /// images overall. This has no effect if anti-ailising is disabled.
        /// </summary>
        public double Support
        {
            get { return sup; }
            set { sup = Math.Abs(value); }
        }

        /// <summary>
        /// Enables jittering by adding a tiny displacment to each sample 
        /// point. This helps prevent Moire paterns from appearing, but can 
        /// cause flickering when animating. This has no effect if 
        /// anti-ailising is disabled.
        /// </summary>
        public bool Jitter
        {
            get { return jitter; }
            set { jitter = value; }
        }

        /// <summary>
        /// The seed of the internal random number generator. This determins
        /// how the datapoints are jitterd when jittering is enabled.
        /// </summary>
        public int Seed
        {
            get { return rng.Seed; }
        }

        #endregion ////////////////////////////////////////////////////////////////

        #region Object Events...

        /// <summary>
        /// The render event is envoked after each pixel is rendered. This
        /// can be used to show a progress bar, or display the results of
        /// the render thus far.
        /// </summary>
        public event EventHandler<RenderEventArgs> RenderEvent
        {
            add { e_render += value; }
            remove { e_render -= value; }
        }

        public event EventHandler StartEvent
        {
            add { e_start += value; }
            remove { e_start -= value; }
        }

        /// <summary>
        /// The finish event is envoked once the render is complete. This
        /// allows the end user to prefrom any post processing, like 
        /// displaying the rendered image, or saving it to disk.
        /// </summary>
        public event EventHandler FinishEvent
        {
            add { e_finish += value; }
            remove { e_finish -= value; }
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
            bool halt = false;
            int count = 0;

            OnStart();

            for (int y = 0; y < output.Height; y++)
            {
                for (int x = 0; x < output.Width; x++)
                {
                    count = y * output.Width + x;
                    output[x, y] = RenderPixel(t, x, y, w, h);
                    halt = OnRender(count, output);

                    if (halt) return;
                }
            }

            //invokes any finishing events that are regesterd
            if (e_finish != null) e_finish(this, EventArgs.Empty);
        }

        /// <summary>
        /// Renders the texture one pixel at a time, in a top-down fassion. This
        /// allows for aditional processing to occor, sutch as reporting the
        /// progress on long running renders.
        /// </summary>
        /// <param name="t">Texture to be rendered</param>
        /// <param name="width">Width of the output image</param>
        /// <param name="height">Height of the output image</param>
        /// <returns>Each pixel in the image</returns>
        /// <exception cref="ArgRangeExcp">If either the width or the
        /// height is less than one</exception>
        public IEnumerable<Pixel> Render(Texture t, int width, int height)
        {
            int w = Math.Max(width, 16);
            int h = Math.Max(height, 16);
            //bool halt = false;

            for (int y = 0; y < w; y++)
            {
                for (int x = 0; x < h; x++)
                {
                    Color c = RenderPixel(t, x, y, w, h);
                    yield return new Pixel(x, y, c);

                    //halt = OnRender(y * w + x, null);
                    //if (halt) yield break;
                }
            }

            ////invokes any finishing events that are regesterd
            //if (e_finish != null) e_finish(this, EventArgs.Empty);
        }

        private bool OnRender(int count, Image img)
        {
            //checks that we actualy have someone listening
            if (e_render == null) return false;

            //creates new event args and invokes the event
            var args = new RenderEventArgs(0, count, img);
            e_render(this, args); return args.Halt;
        }

        private void OnStart()
        {
            //invokes any finishing events that are regesterd
            if (e_start != null) e_start(this, EventArgs.Empty);
        }


        /**
         *  Atempt to make a threaded renderor. This works! It also runs faster for
         *  purly computaitonal renderers. I suspect that this is because bitmaps
         *  can only be accessed by one thread at a time.
         */
        public IEnumerable<Pixel> RenderParallel(Texture t, int width, int height)
        {
            //var rendering =
            //    from x in ParallelEnumerable.Range(0, width)
            //    from y in ParallelEnumerable.Range(0, height)
            //    select new { x, y, c = RenderPixel(t, x, y, width, height) };

            //foreach (var item in rendering)
            //{
            //    yield return new Pixel(item.x, item.y, item.c);
            //}

            var rendering =
                from x in ParallelEnumerable.Range(0, width)
                from y in ParallelEnumerable.Range(0, height)
                select new Pixel(x, y, RenderPixel(t, x, y, width, height));

            return rendering.AsEnumerable();
        }

        /**
         *  This sort-of works, but it actualy runs slower than the single threaded
         *  version, and I don't know why! Multi-threading is weird. 
         */
        public void RenderParallel(Texture t, Image output)
        {
            int count = 0;
            int w = output.Width;
            int h = output.Height;   
            bool halt = false; 

            var rendering =
                from x in ParallelEnumerable.Range(0, w)
                from y in ParallelEnumerable.Range(0, h)
                select new Pixel(x, y, RenderPixel(t, x, y, w, h));    

            foreach (var pixel in rendering)
            {
                int x = pixel.X;
                int y = pixel.Y;
                Color c = pixel.Color;

                output.SetPixel(x, y, c);
                halt = OnRender(count, output);

                if (halt) break;
            }

            //invokes any finishing events that are regesterd
            if (e_finish != null) e_finish(this, EventArgs.Empty);
        }

        #endregion ////////////////////////////////////////////////////////////////

        #region Sub-Sampeling

        /// <summary>
        /// Generates the color of a single pixel in the target image, given the 
        /// texture to render, the locaiton of the pixel, and the dimentions of 
        /// the target image. It dose this by generating multiple samples around
        /// the center of the pixel, and computing a weighted average based on the
        /// current windowing function.
        /// </summary>
        /// <param name="t">Texture to be rendered</param>
        /// <param name="x">X cordinate of the pixel to render</param>
        /// <param name="y">Y cordinate of the pixel to render</param>
        /// <param name="w">Width of the target image</param>
        /// <param name="h">Height of the target image</param>
        /// <returns>The final color of the given pixel in the rendered image</returns>
        private Color RenderPixel(Texture t, double x, double y, double w, double h)
        {
            if (!aa_flag)
            {
                //samples only the center of the pixel
                Point2D p = ToTexture(x, y, w, h);
                return t.Sample(p.X, p.Y);
            }

            //generates the sub-samples for the pixel
            double a = 1.903476229 / Math.Sqrt(nsamp);
            var samples = GetSamples(a);

            //used in computing the pixel's color
            Vector sum = new Vector(4);
            double weight = 0.0;

            foreach (Point2D sample in samples)
            {
                double dx = (sample.X * sup) + x;
                double dy = (sample.Y * sup) + y;
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

            //only the windowing functions above are suported
            throw new NotSupportedException();
        }

        /// <summary>
        /// Generates sample points in a hexoginal lattus, filling the
        /// unit square. It optionaly jitters the points if enabled.
        /// </summary>
        /// <param name="a">Distance between samples</param>
        /// <returns>A listing of all sample points</returns>
        private IEnumerable<Point2D> GetSamples(double a)
        {
            double b = a * 0.86602540378443864676;
            double x, y;

            int n = (int)Math.Floor(1.0 / a);
            int m = (int)Math.Floor(1.0 / b);

            for (int j = -m; j <= m; j++)
            {
                for (int i = -n; i <= n; i++)
                {
                    x = a * i;
                    y = b * j;

                    //shifts by half for odd rows
                    if (j % 2 != 0) x += a / 2.0;

                    //jitters each point
                    if (jitter)
                    {
                        x += rng.RandGauss(0.0, a / 4.0);
                        y += rng.RandGauss(0.0, a / 4.0);
                    }

                    yield return new Point2D(x, y);
                }
            }
        }

        #endregion ////////////////////////////////////////////////////////////////
    }
}
