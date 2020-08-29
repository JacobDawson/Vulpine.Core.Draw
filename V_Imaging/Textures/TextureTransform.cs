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
using Vulpine.Core.Calc.Geometry;

namespace Vulpine.Core.Draw.Textures
{
    /// <summary>
    /// This class represents a transformed texture by giving it a location, an orentation, and a
    /// scale, as well as a few other affine transfomations like reflection and sheer. While
    /// not every affine transformation can be constructed in this manner, it covers the most
    /// common use cases for textures. The key advantage in using this class is that it exposes
    /// the transform as a series of meta paramaters which can be tweeked individualy. This is
    /// usefull for treating textures like objects in 2D space.
    /// </summary>
    public class TextureTransform : Texture
    {
        #region Class Definitions...

        //referes to the texture being transformed
        private Texture inner;

        //stores the piece-wise components of the transformation
        private Point2D loc;
        private Point2D scale;
        private Point2D sheer;
        private double rot;
        private bool mirror;

        //stores the matrix that dose the actual transformation
        private Trans2D trans;

        //indicates if the matrix needs to be reubilt
        private bool rebuild;

        /// <summary>
        /// Creates a default texture transform, which can then be further
        /// customised by tweeking the paramaters. 
        /// </summary>
        /// <param name="other">Texture to be transformed</param>
        public TextureTransform(Texture other)
        {
            //stores a refrence to the inner texture
            this.inner = other;

            //sets the default paramaters
            loc = new Point2D(0.0, 0.0);
            scale = new Point2D(1.0, 1.0);
            sheer = new Point2D(0.0, 0.0);
            mirror = false;
            rot = 0.0;

            //starts with the identity matrix
            trans = Trans2D.Identity;
            rebuild = false;           
        }

        /// <summary>
        /// Creates a texture transform with the given location, scale, and rotation.
        /// This is suffiecnt for most types of textures, however tweeking other
        /// paramater can customise the transform further.
        /// </summary>
        /// <param name="other">Texture to be transformed</param>
        /// <param name="loc">Location of the texture</param>
        /// <param name="scale">Scale of the texture</param>
        /// <param name="rot">Orientation of the texture in degrees</param>
        public TextureTransform(Texture other, Point2D loc, double scale, double rot)
        {
            //stores a refrence to the inner texture
            this.inner = other;
            
            //sets the paramaters as indicated 
            this.loc = loc;
            this.scale = new Point2D(scale, scale);
            this.sheer = new Point2D(0.0, 0.0);
            this.mirror = false;
            this.rot = rot;

            //pre-builds the matrix
            BuildMatrix();  
        }

        /// <summary>
        /// Creates a texture transform with the given location, scale, rotation, 
        /// and mirroring. It offers greater control than the other constructor,
        /// allowing scale to be set indpend=ntly on the X and Y axis.
        /// </summary>
        /// <param name="other">Texture to be transformed</param>
        /// <param name="loc">Location of the texture</param>
        /// <param name="scale">Scaling in X and Y</param>
        /// <param name="rot">Orientation in degrees</param>
        /// <param name="mirror">True to mirror the texture</param>
        public TextureTransform(Texture other, Point2D loc, Point2D scale,
            double rot, bool mirror)
        {
            //stores a refrence to the inner texture
            this.inner = other;

            //sets the paramaters as indicated 
            this.loc = loc;
            this.scale = scale;
            this.sheer = new Point2D(0.0, 0.0);
            this.mirror = false;
            this.rot = rot;

            //pre-builds the matrix
            BuildMatrix();  
        }

        /// <summary>
        /// Full constructor for a texture transformation, allowing all the paramaters
        /// to be initilised. Everything is given in floating point form and paramaters
        /// can be ommited to leave them at their defaults.
        /// </summary>
        /// <param name="other">Texture to transform</param>
        /// <param name="px">X cordinate of the location</param>
        /// <param name="py">Y cordinate of the location</param>
        /// <param name="sx">Amount of scaling in X</param>
        /// <param name="sy">Amount of scaling in Y</param>
        /// <param name="zx">Amount of sheering in X</param>
        /// <param name="zy">Amount of sheering in Y</param>
        /// <param name="rot">Amount of roattion in degrees</param>
        /// <param name="mirror">True to mirror the texture</param>
        public TextureTransform(Texture other, 
            double px = 0.0, double py = 0.0,
            double sx = 1.0, double sy = 1.0,
            double zx = 0.0, double zy = 0.0,
            double rot = 0.0, bool mirror = false)
        {
            //stores a refrence to the inner texture
            this.inner = other;

            //sets the paramaters as indicated
            this.loc = new Point2D(px, py);
            this.scale = new Point2D(sx, sy);
            this.sheer = new Point2D(zx, zy);
            this.mirror = mirror;
            this.rot = rot;

            //pre-builds the matrix
            BuildMatrix();     
        }

        #endregion ///////////////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// Determins the location of the transformed texture. The origin of the
        /// previous texture will be moved to this location.
        /// </summary>
        public Point2D Location
        {
            get { return loc; }
            set { loc = value; rebuild = true; }
        }

        /// <summary>
        /// Determins the scale of the transformed texture, independently on
        /// the X and Y axis. Scaling more in one axis than the other results in
        /// streaching and squashing of the texture.
        /// </summary>
        public Point2D Scale
        {
            get { return scale; }
            set { scale = value; rebuild = true; }
        }
     
        /// <summary>
        /// Determins the orientation of the transformed texture, mesured in
        /// degrees, turning clockwise. An orentation of 90 degrees, for example,
        /// will turn the base texture a quarter-turn to the right.
        /// </summary>
        public double Orientation
        {
            get { return rot; }
            set { rot = value; rebuild = true; }
        }

        /// <summary>
        /// Determins the amount of sheering in both the X and Y axies. 
        /// </summary>
        public Point2D Sheer
        {
            get { return sheer; }
            set { sheer = value; rebuild = true; }
        }

        /// <summary>
        /// Determins if the texture should be mirrored in the transform. If true,
        /// the transform will operate on the mirror image of the original texture.
        /// </summary>
        public bool Mirrored
        {
            get { return mirror; }
            set { mirror = value; rebuild = true; }
        }

        /// <summary>
        /// Determins the scaling in both the X and Y axies simultaniously, allowing
        /// for uniform scaling without squahsing or streaching. Scaling by a negative
        /// value inverts the image.
        /// </summary>
        public double ScaleAll
        {
            get
            {
                if (scale.X != scale.Y) return Double.NaN;
                else return scale.X;
            }
            set
            {
                scale = new Point2D(value, value);
                rebuild = true;
            }
        }

        #endregion ///////////////////////////////////////////////////////////////////////

        #region Texture Implementation...

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
            //builds the matrix if nessary
            if (rebuild) BuildMatrix();

            //transforms the point by our matrix
            Point2D point = new Point2D(u, v);
            point = trans.Transform(point);

            //samples the texture at the transfomred point
            return inner.Sample(point.X, point.Y);
        }


        /// <summary>
        /// Helper method, used to build the internal matrix that stores the
        /// actual transformation. This must be called whenever any of the
        /// hyper-paramaters change.
        /// </summary>
        private void BuildMatrix()
        {
            //derives rotaiton and reflection
            trans = mirror ? Trans2D.ReflectVert : Trans2D.Identity;
            double theta = VMath.ToRad(-rot);
            
            //applies the transfomations in order
            trans += Trans2D.Shear(sheer);
            trans += Trans2D.Scale(scale);
            trans += Trans2D.Rotate(theta);
            trans += Trans2D.Translate(loc);

            //clears the rebuild flag forces garbage collection
            rebuild = false;
            GC.Collect();
        }

        #endregion ///////////////////////////////////////////////////////////////////////

    };
}
