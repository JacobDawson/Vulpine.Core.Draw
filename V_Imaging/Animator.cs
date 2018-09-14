/**
 *  This file is an integral part of the Vulpine Core Library
 *  Copyright (c) 2016-2018 Benjamin Jacob Dawson
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
using Vulpine.Core.Calc.Geometry;
using Vulpine.Core.Calc.Matrices;
using Vulpine.Core.Calc.RandGen;

namespace Vulpine.Core.Draw
{
    public class Animator
    {
        //determins how the texture should be scaled to fit
        private Scaling scale = Scaling.Vertical;

        //determins if anti-ailising should be preformed
        private bool aa_flag = true;

        //paramaters that describe the anti-ailising
        private Window win = Window.Gausian;
        private double sup = VMath.R2 / 2.0;
        private bool jitter = true;
        private int nsamp = 16;

        //stores the delegate to handel render events
        private EventHandler<RenderEventArgs> e_render;

        //stores the start and finish events
        private EventHandler e_start;
        private EventHandler e_finish;

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
        private Color RenderPixel(Texture3D t, int x, int y, double w, double h, double tau)
        {
            if (!aa_flag)
            {
                //samples only the center of the pixel
                Point2D p = ToTexture(x, y, w, h);
                return t.Sample(tau, p.X, p.Y);
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
                Vector temp = (Vector)t.Sample(tau, p.X, p.Y);

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
            //b = a * sqrt(3) / 2;
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

                    yield return new Point2D(x, y);
                }
            }
        }
    }
}
