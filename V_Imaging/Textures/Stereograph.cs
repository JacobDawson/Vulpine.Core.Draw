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

using Vulpine.Core.Data.Exceptions;
using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Exceptions;
using Vulpine.Core.Calc.Geometry;

namespace Vulpine.Core.Draw.Textures
{

    /// <summary>
    /// This texture accepts an panoramic image in the equerectangular projection and
    /// converts it to a steriographic projection, with the nadar at the origin and
    /// the zenith at infinity. This is also sometimes refered to as the Little Planet
    /// projection, due to the effect it has on outdoor panoramas. Although it can work
    /// with any image, full spherical panoramas provide the best results.
    /// </summary>
    /// <remarks>Last Update: 2016-02-17</remarks>
    public class Stereograph : Texture
    {
        #region Class Definitions...

        //NOTE: Consider adding constructors that take Images instead of Textures

        //refrences the source texture
        private Texture source;

        //stores the scale and rotation
        private double scale;
        private double rot;
        private bool inv;

        /// <summary>
        /// Creates a new Stereographic Projection with a scale of one
        /// and a rotation of zero.
        /// </summary>
        /// <param name="source">The source panorama</param>
        public Stereograph(Texture source)
        {
            this.source = source;
            this.scale = 1.0;
            this.rot = 0.0;
            this.inv = false;
        }

        /// <summary>
        /// Creates a new Sterographic Projection with the desired scale
        /// and rotaiton, given in degrees.
        /// </summary>
        /// <param name="source">The source panorama</param>
        /// <param name="scale">Scale of the output</param>
        /// <param name="rot">Rotation of the output</param>
        /// <exception cref="ArgBoundsException">If the scale is negative
        /// or the rotation is outside the range 0 to 360</exception>
        public Stereograph(Texture source, double scale, double rot)
        {
            ArgBoundsException.Atleast("scale", scale, 0.0);
            ArgBoundsException.Check("rot", rot, 0.0, 360.0);

            this.source = source;
            this.scale = scale;
            this.rot = VMath.ToRad(rot);
            this.inv = false;
        }

        /// <summary>
        /// Creates a new Sterographic Projection with the desired scale
        /// and rotaiton, given in degrees, as well as potential inverting
        /// the zenith and the nadar.
        /// </summary>
        /// <param name="source">The source panorama</param>
        /// <param name="scale">Scale of the output</param>
        /// <param name="rot">Rotation of the output</param>
        /// <param name="inv">Set True to invert the projection</param>
        /// <exception cref="ArgBoundsException">If the scale is negative
        /// or the rotation is outside the range 0 to 360</exception>
        public Stereograph(Texture source, double scale, double rot, bool inv)
        {
            ArgBoundsException.Atleast("scale", scale, 0.0);
            ArgBoundsException.Check("rot", rot, 0.0, 360.0);

            this.source = source;
            this.scale = scale;
            this.rot = VMath.ToRad(rot);
            this.inv = inv;
        }

        #endregion ///////////////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// Represents the scale of the Steriographic Projection. A scale of
        /// one places the horison on the unit circle.
        /// </summary>
        public double Scale
        {
            get { return scale; }
        }

        /// <summary>
        /// Represents the rotaiton of the Steriographic Projection 
        /// measured in degrees.
        /// </summary>
        public double Rotation
        {
            get { return VMath.ToDeg(rot); }
        }

        /// <summary>
        /// Determins if the projection is inverted, with the zenith and
        /// the nadar switching places.
        /// </summary>
        public bool Invert
        {
            get { return inv; }
        }

        #endregion ///////////////////////////////////////////////////////////////////////

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
            //converts the point and grabs a sample
            Point2D p = Convert(u, -v);
            return source.Sample(p.X, p.Y);
        }

        /// <summary>
        /// Converts a point on the plane to a point on the sphere through
        /// the reverse steriographic projection.
        /// </summary>
        /// <param name="x">The X position</param>
        /// <param name="y">The Y position</param>
        /// <returns>The point on the shpere</returns>
        private Point2D Convert(double x, double y)
        {
            //obtains the polar cordinates
            double r = (x * x) + (y * y);
            double t = Math.Atan2(y, x);

            //scales the cordinates appropriatly
            r = Math.Sqrt(r) * scale;
            t = t + rot + Math.PI;

            //calculates the spherical cordinates
            double rho = (t > VMath.TAU) ? t - VMath.TAU : t;
            double phi = 2.0 * Math.Atan(1.0 / r);

            //scales the values to the range [0, 1]
            rho = rho / VMath.TAU;
            phi = phi / Math.PI;
            phi = inv ? phi : 1.0 - phi;

            //scales the values to the range [-1, 1]
            rho = (rho * 2.0) - 1.0;
            phi = (phi * 2.0) - 1.0;

            return new Point2D(rho, phi);
        }

        #endregion ///////////////////////////////////////////////////////////////////////
    }
}
