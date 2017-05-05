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

using Vulpine.Core.Calc.Exceptions;

namespace Vulpine.Core.Draw
{
    /// <summary>
    /// A Texture describes a continious, two-dimentional image as a function of color. 
    /// This could be an interpolated image, a rendered scean, a transformation of another 
    /// texture, or any 2D function that returns a color value. Textures are evaluated 
    /// laisly, in that no work is done until you sample the texture at a given point. 
    /// Textures also use there own UV cordinate system, with the origin at the center of
    /// the texture, and the V axis pointing up. Textures are infinite in scope and do not
    /// have an absolute scale. Only the relitive scale between textures is important.
    /// <remarks>Last Update: 2017-05-05</remarks>
    /// </summary>
    public interface Texture
    {
        /// <summary>
        /// Samples the texture at a given point, calculating the color of the 
        /// texture at that point. The sample point is provided in UV cordinats
        /// with the origin at the center and the V axis pointing up.
        /// </summary>
        /// <param name="u">The U texture cordinate</param>
        /// <param name="v">The V texture cordinate</param>
        /// <returns>The color sampled at the given point</returns>
        Color Sample(double u, double v);
    }
}
