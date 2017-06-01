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
using Vulpine.Core.Calc.Geometry;

namespace Vulpine.Core.Draw.Textures
{
    public class Transformation : Texture
    {
        //stores the inner texture and it's transfomation
        private Texture inner;
        private Transform2D trans;

        /// <summary>
        /// Builds a transformation texture, given the internal texture
        /// and the transformation to be applied.
        /// </summary>
        /// <param name="inner">The internal texture</param>
        /// <param name="trans">Transformation to be applied</param>
        public Transformation(Texture inner, Transform2D trans)
        {
            this.inner = inner;
            this.trans = new Transform2D(trans);
        }

        /// <summary>
        /// Extracts the color data from the texture at an explicit point,
        /// in texture cordinates, where the principle width and height of
        /// the texture are equal to one.
        /// </summary>
        /// <param name="u">The u texture cordinate</param>
        /// <param name="v">The v texture cordinate</param>
        /// <returns>The color sampled at the given point</returns>
        public Color Sample(double u, double v)
        {
            Point2D targ = trans.Trans(u, v);
            return inner.Sample(targ.X, targ.Y);
        }
    }
}
