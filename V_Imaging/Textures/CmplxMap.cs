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
using Vulpine.Core.Calc.Numbers;
using Vulpine.Core.Calc.Geometry;

namespace Vulpine.Core.Draw.Textures
{
    public class CmplxMap : Texture
    {
        private const double Limit = 2.8e7;

        //the texture used for coloring the map
        private Texture source;

        //the viewing window of the funciton
        private Rectangle window;

        //the funciton to be rendered
        private VFunc<Cmplx> func;

        /// <summary>
        /// Creates a new complex map with the given source texture
        /// and function to evaluate. The viewing window is centered
        /// on the origin with a radius of one.
        /// </summary>
        /// <param name="source">Source texture</param>
        /// <param name="func">Function to evaluate</param>
        public CmplxMap(Texture source, VFunc<Cmplx> func)
        {
            this.source = source;
            this.func = func;
            this.window = Rectangle.FromPoints(-1.0, -1.0, 1.0, 1.0);
        }

        ///// <summary>
        ///// Creates a new complex map with the given source texture
        ///// and function to evaluate. The viewing window is centered
        ///// on the origin with a radius of one.
        ///// </summary>
        ///// <param name="source">Source texture</param>
        ///// <param name="func">Function to evaluate</param>
        //public CmplxMap(Texture source, Function<Cmplx> func)
        //{
        //    this.source = source;
        //    this.func = (func.Evaluate);
        //    this.window = Rectangle.FromPoints(-1.0, -1.0, 1.0, 1.0);
        //}

        /// <summary>
        /// Creates a new complex map with the given source texture
        /// and funciton to evaluate, as well as the desired viewing
        /// window for the function.
        /// </summary>
        /// <param name="source">Source texture</param>
        /// <param name="window">Viewing window</param>
        /// <param name="func">Function to evaluate</param>
        public CmplxMap(Texture source, Rectangle window, VFunc<Cmplx> func)
        {
            this.source = source;
            this.func = func;
            this.window = window;
        }

        ///// <summary>
        ///// Creates a new complex map with the given source texture
        ///// and funciton to evaluate, as well as the desired viewing
        ///// window for the function.
        ///// </summary>
        ///// <param name="source">Source texture</param>
        ///// <param name="window">Viewing window</param>
        ///// <param name="func">Function to evaluate</param>
        //public CmplxMap(Texture source, Rectangle window, Function<Cmplx> func)
        //{
        //    this.source = source;
        //    this.func = (func.Evaluate);
        //    this.window = window;
        //}

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
            Point2D p = window.Transform(u, v);
            return EvaluateColor(p.X, p.Y);
        }

        /// <summary>
        /// Evaluates the complex function, and colors the corisponding 
        /// result based on the source texture
        /// </summary>
        /// <param name="x">Real component to evaluate</param>
        /// <param name="y">Imaginary component to evaluate</param>
        /// <returns>The result of the function as a color</returns>
        public Color EvaluateColor(double x, double y)
        {
            //invokes the complex function
            Cmplx z = func.Invoke(new Cmplx(x, -y));

            if (z.IsInfinity())
            {
                //returns white if we are at a pole
                return new Color(1.0, 1.0, 1.0);
            }
            else if (z.Abs > Limit)
            {
                //returns white if we are close to a pole
                return new Color(1.0, 1.0, 1.0);
            }
            else
            {
                //converts the output to the range [0, 1]
                double x0 = (z.CofR * 0.5) + 0.5;
                double y0 = (z.CofI * 0.5) + 0.5;

                //uses the source texture for the color
                return source.GetValue(x0, y0);
            }
        }



    }
}
