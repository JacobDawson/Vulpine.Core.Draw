/**
 *  This file is an integral part of the Vulpine Core Library
 *  Copyright (c) 2016-2019 Benjamin Jacob Dawson
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

namespace Vulpine.Core.Draw
{
    /// <summary>
    /// For the purpouses of this library, an animation is considered to be a sequence of
    /// related textures, indexed by frame number. For the purpouses of thread safty, each
    /// frame must be independtly generated. Note that this is substantialy diffrent from
    /// a continious 3D texture, as the frame index is descrete. Because frames are indexed
    /// by frame number, rather than time, things like frame rate and duration become intrensic
    /// properties of the animaiton. Although, generaly speaking, a standard framerate of
    /// 30 FPS can be assumed.
    /// <remarks>Last Update: 2019-03-26</remarks>
    /// </summary>
    public interface Animation
    {
        /// <summary>
        /// Determins the total number of frames for the given animaiton.
        /// </summary>
        int FrameCount { get; }

        /// <summary>
        /// Constructs a texture for the given frame index, garenteed to be thread
        /// independent of the other textures generated. Frames use zero based
        /// indexing, and run until the given frame count.
        /// </summary>
        /// <param name="frame">Frame index</param>
        /// <exception cref="ArgumentOutOfRangeException">If the frame index is
        /// less than zero, or greator than the last frame index</exception>
        /// <returns>The target frame as a texture</returns>
        Texture GetFrame(int frame);
    }

        
}
