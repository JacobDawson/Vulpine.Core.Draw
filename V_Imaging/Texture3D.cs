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

namespace Vulpine.Core.Draw
{
    /// <summary>
    /// A 3D Texture describes a continious, three-dimentional image as a function of 
    /// color. It is similar to a regular 2D Texture, except that there is an extra
    /// input paramater. 3D Testures include animations, where one of the input
    /// paramaters (usualy t) is taken to represent time. They also inclue true 3D
    /// images, like MRIs and procedural materials used in 3D modeling.
    /// <remarks>Last Update: 2017-08-26</remarks>
    /// </summary>
    public interface Texture3D
    {
        /// <summary>
        /// Samples the texture at a given point, calculating the color of the 
        /// texture at that point. The sample point is provided in UV cordinats
        /// with the origin at the center and the V axis pointing up.
        /// </summary>
        /// <param name="t">The T texture cordinate</param>
        /// <param name="u">The U texture cordinate</param>
        /// <param name="v">The V texture cordinate</param>
        /// <returns>The color sampled at the given point</returns>
        Color Sample(double t, double u, double v);
    }

    
}
