/**
 *  This file is an integral part of the Vulpine Core Library: 
 *  Copyright (c) 2016-2017 Benjamin Jacob Dawson. 
 *
 *      http://www.jakesden.com/corelibrary.html
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
    /// <summary>
    /// A complex map uses a function in the complex plane to map one texture into another.
    /// These are sometimes referd to as conformal maps, as angles in the input texture are
    /// preserved in the output texture. This class can also be used as a way of visualising
    /// the complex functions themselves, utilising a process known as domain coloring.
    /// </summary>
    /// <remarks>Last Update: 2017-05-05</remarks>
    public class CmplxMap : Texture
    {
        #region Class Definitions...

        //how close we can be to a pole before coloring it white
        private const double Limit = 2.8e7;

        //the texture used for coloring the map
        private Texture source;

        //the funciton to be rendered
        private VFunc<Cmplx> func;

        //stores the viewing parmaters for the resulting texture
        private Point2D center = new Point2D(0.0, 0.0);
        private double scale = 2.0;

        /// <summary>
        /// Creates a new complex map with the given source texture and function 
        /// to evaluate. It is centered at the origin with a scaling value of one.
        /// </summary>
        /// <param name="source">Source texture</param>
        /// <param name="func">Function to evaluate</param>
        public CmplxMap(Texture source, VFunc<Cmplx> func)
        {
            this.source = source;
            this.func = func;
            this.center = new Point2D(0.0, 0.0);
            this.scale = 1.0;
        }

        /// <summary>
        /// Creates a new complex map with the given source texture and function 
        /// to evaluate. It is centered at the origin with the desired scale.
        /// </summary>
        /// <param name="source">Source texture</param>
        /// <param name="func">Function to evaluate</param>
        /// <param name="scale">Size of the output</param>
        public CmplxMap(Texture source, VFunc<Cmplx> func, double scale)
        {
            this.source = source;
            this.func = func;
            this.center = new Point2D(0.0, 0.0);
            this.scale = Math.Abs(scale);
        }

        /// <summary>
        /// Creates a new complex map with the given source texture and function 
        /// to evaluate, as well as the center and scale of the resulting texture.
        /// </summary>
        /// <param name="source">Source texture</param>
        /// <param name="func">Function to evaluate</param>
        /// <param name="center">Center of the output</param>
        /// <param name="scale">Size of the output</param>
        public CmplxMap(Texture source, VFunc<Cmplx> func, Point2D center, double scale)
        {
            this.source = source;
            this.func = func;
            this.center = center;
            this.scale = Math.Abs(scale);
        }

        #endregion ///////////////////////////////////////////////////////////////////////////

        #region Class Properties... 

        /// <summary>
        /// Represents the centerpoint of the resulting texture. This can be
        /// though of as moving the origin on the complex plane.
        /// </summary>
        public Point2D Center
        {
            get { return center; }
        }

        /// <summary>
        /// Determins the scale of the resulting texture, that is, how big or
        /// small the map looks when rendered out to an image.
        /// </summary>
        public double Scale
        {
            get { return scale; }
        }

        #endregion ///////////////////////////////////////////////////////////////////////////

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
            double x = (u * scale) + center.X;
            double y = (v * scale) + center.Y;

            return EvaluateColor(x, y);
        }

        /// <summary>
        /// Evaluates the complex function, and colors the corisponding 
        /// result based on the source texture
        /// </summary>
        /// <param name="x">Real component to evaluate</param>
        /// <param name="y">Imaginary component to evaluate</param>
        /// <returns>The result of the function as a color</returns>
        private Color EvaluateColor(double x, double y)
        {
            //invokes the complex function
            Cmplx z = func.Invoke(new Cmplx(x, y));

            if (z.IsInfinity() || z.Abs > Limit)
            {
                //returns white if we are close to a pole
                return new Color(1.0, 1.0, 1.0);
            }
            else if (z.IsNaN())
            {
                //return neutral grey if the output is NaN
                return new Color(0.5, 0.5, 0.5);
            }
            else
            {
                //uses the source texture to determin our value
                return source.Sample(z.CofR, z.CofI); 
            }
        }

        #endregion ///////////////////////////////////////////////////////////////////////////

    }
}
