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
    /// This class contains a collection of event arguments to be passed to some 
    /// event listner handeling the rendering of some image. It contains values
    /// to indicate the number of pixles rendred so far, as well as a refrence 
    /// to the target image, purly for convience. It also provides a halting flag
    /// should the rendering needs to be terminated.
    /// </summary>
    /// <remarks>Last Update: 2017-05-24</remarks>
    public class RenderEventArgs : EventArgs
    {
        //information about the render process
        private int frame;
        private int count;

        //a refrence to the image being rendered
        private Image buffer;

        //uses a flag to indicate when to halt
        private bool halt;

        /// <summary>
        /// Creates a new collection of render event arguments, containing
        /// the given arguments passed in to the constructor.
        /// </summary>
        /// <param name="frame">Number of frames rendered</param>
        /// <param name="count">Number of pixels rendered</param>
        /// <param name="img">The target image</param>
        internal RenderEventArgs(int frame, int count, Image img)
        {
            this.frame = frame;
            this.count = count;

            buffer = img;
            halt = false;
        }

        /// <summary>
        /// Represents the total number of complete frames rendered. 
        /// Used only when rendering animations.
        /// </summary>
        public int Frame
        {
            get { return frame; }
        }

        /// <summary>
        /// Represents the total number of pixels rendered for the 
        /// curent frame. For single images, this is just the total 
        /// number of pixels rendered. 
        /// </summary>
        public int Count
        {
            get { return count; }
        }

        /// <summary>
        /// A reference to the target image being rendered, or the
        /// frame buffer for animations.
        /// </summary>
        public Image Buffer
        {
            get { return buffer; }
        }

        /// <summary>
        /// A flag indicating if the process should halt. The flag can be
        /// raised by setting it to true, but it cannot be unraised.
        /// </summary>
        public bool Halt
        {
            get { return halt; }
            set { halt |= value; }
        }
    }
}
