/**
 *  This file is an integral part of the Vulpine Core Library
 *  Copyright (c) 2016-2020 Benjamin Jacob Dawson
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
using Vulpine.Core.Calc.Geometry.Planer;

namespace Vulpine.Core.Draw.Textures
{
    public class TextureMatrix : Texture
    {
        //stores the inner texture and it's transfomation
        private Texture inner;
        private Trans2D trans;

        /// <summary>
        /// Builds a transformation texture, given the internal texture
        /// and the transformation to be applied.
        /// </summary>
        /// <param name="inner">The internal texture</param>
        /// <param name="trans">Transformation to be applied</param>
        public TextureMatrix(Texture inner, Trans2D trans)
        {
            this.inner = inner;
            this.trans = trans;
        }

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
            Point2D targ = trans.Transform(u, v);
            return inner.Sample(targ.X, targ.Y);
        }
    }
}
