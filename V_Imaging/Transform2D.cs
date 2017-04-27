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

using Vulpine.Core.Data;
using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Matrices;
using Vulpine.Core.Calc.Geometry;

namespace Vulpine.Core.Draw
{

    /// <summary>
    /// This class can represent any affine transformaiton in two dimentional space,
    /// allowing for more complex transformations to be build sequentualy from
    /// several core transformations, sutch as translation, rotation, and scale.
    /// Although this class can be thought of as a series of transfomations, in effect,
    /// only a single combined transformation is stored and used.
    /// </summary>
    /// <remarks>Last Update: 2016-02-17</remarks>
    public class Transform2D //: Cloneable<Transform2D>
    {
        #region Class Definitions...

        //the transformation matrix
        private Matrix trans;
        private int count; 

        /// <summary>
        /// Constructs an empty transformation. Points processed by this
        /// transformation remain unchanged.
        /// </summary>
        public Transform2D()
        {
            trans = Matrix.Ident(3);
            count = 0;
        }

        /// <summary>
        /// Copy Constructor: Creates a new transform by copying all
        /// the contents of a second transform.
        /// </summary>
        /// <param name="other">The transform to copy</param>
        public Transform2D(Transform2D other)
        {
            //makes a copy of the other transform
            this.trans = new Matrix(other.trans);
            count = other.count;
        }

        /// <summary>
        /// Generates a deep copy of the current transformation by 
        /// invoking the corisponding copy constructor.
        /// </summary>
        /// <returns>A copy of the transformation</returns>
        public Transform2D Clone()
        {
            //invokes the copy constructor
            return new Transform2D(this);
        }

        #endregion //////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// Determins the number of core transforms that have been
        /// combined thus far.
        /// </summary>
        public int Count
        {
            get { return count; }
        }

        #endregion //////////////////////////////////////////////////////////

        #region Core Opperations...

        /// <summary>
        /// Applies the curent transform to a two dimentional point.
        /// No prerspective division is preformed for non-affine
        /// transformations. 
        /// </summary>
        /// <param name="x">X cordinate of the input</param>
        /// <param name="y">Y cordinate of the input</param>
        /// <returns>The transformed point</returns>
        public Point2D Trans(double x, double y)
        {
            //converts to homognous cordinates and transforms
            Vector v = new Vector(x, y, 1.0);
            v = trans.Mult(v);

            //returns the resultant vector
            return new Point2D(v[0], v[1]);
        }

        /// <summary>
        /// Applies the curent transform to a two dimentional point.
        /// No prerspective division is preformed for non-affine
        /// transformations. 
        /// </summary>
        /// <param name="p">Point to transformt</param>
        /// <returns>The transformed point</returns>
        public Point2D Trans(Point2D p)
        {
            //converts to homognous cordinates and transforms
            Vector v = new Vector(p.X, p.Y, 1.0);
            v = trans.Mult(v);

            //returns the resultant vector
            return new Point2D(v[0], v[1]);
        }

        /// <summary>
        /// Appends another constructed transformation to the current
        /// transformation. The result is that the new transformation is
        /// applied after the current transformation.
        /// </summary>
        /// <param name="other">Transformation to append</param>
        public void Append(Transform2D other)
        {
            //multiplies by the other transform
            trans = other.trans * trans;
            count = count + other.count;
        }

        /// <summary>
        /// Resets the transformation, returning it to it's intial 
        /// state. All points already processed by the transformation 
        /// will remain unchanged.
        /// </summary>
        public void Reset()
        {
            trans = Matrix.Ident(3);
            count = 0;
        }

        /// <summary>
        /// Obtains the 3x3 augmented transformation matrix pretaning
        /// to the curently constructed transformation.
        /// </summary>
        /// <returns>The transformation matrix</returns>
        public Matrix GetMatrix()
        {
            //returns a copy of the transform matrix
            return new Matrix(trans);
        }

        #endregion //////////////////////////////////////////////////////////

        #region Translation...

        /// <summary>
        /// Appends a translation to the matrix, resulting in the 
        /// movment of objects from there current position to 
        /// their new designated position.
        /// </summary>
        /// <param name="x">New X cordinate</param>
        /// <param name="y">New Y cordinate</param>
        public void MoveTo(double x, double y)
        {
            //generates the translation matrix
            Matrix m = Matrix.Ident(3);
            m[0, 2] = x;
            m[1, 2] = y;

            //applies it to the transformation
            trans = m * trans;
            count++;
        }

        /// <summary>
        /// Appends a translation to the matrix, resulting in the 
        /// movment of objects from there current position to 
        /// their new designated position.
        /// </summary>
        /// <param name="p">New position</param>
        public void MoveTo(Point2D p)
        {
            //calls the method above
            MoveTo(p.X, p.Y);
        }

        /// <summary>
        /// Appends the inverse translation to the matrix, resulting in
        /// objects moving from the point given to the origin. This is
        /// useful for undoing the action of a MoveTo after some other
        /// action, like a rotation.
        /// </summary>
        /// <param name="x">Old X cordinate</param>
        /// <param name="y">Old Y cordinate</param>
        public void MoveFrom(double x, double y)
        {
            //generates the translation matrix
            Matrix m = Matrix.Ident(3);
            m[0, 2] = -x;
            m[1, 2] = -y;

            //applies it to the transformation
            trans = m * trans;
            count++;
        }

        /// <summary>
        /// Appends the inverse translation to the matrix, resulting in
        /// objects moving from the point given to the origin. This is
        /// useful for undoing the action of a MoveTo after some other
        /// action, like a rotation.
        /// </summary>
        /// <param name="p">Old position</param>
        public void MoveFrom(Point2D p)
        {
            //calls the method above
            MoveFrom(p.X, p.Y);
        }

        #endregion //////////////////////////////////////////////////////////

        #region Rotation And Reflection...

        /// <summary>
        /// Rotates the transformation counter-clockwise about the origin, 
        /// assuming the y-axis points up. In systems where the y-axis 
        /// points down, it rotates clockwise. The angle of rotation is
        /// given in radiens.
        /// </summary>
        /// <param name="theta">Angle of rotation</param>
        public void Rotate(double theta)
        {
            //extracts the sin and cos
            double cos = Math.Cos(theta);
            double sin = Math.Sin(theta);

            //generates the rotation matrix
            Matrix m = Matrix.Ident(3);
            m[0, 0] = cos;
            m[0, 1] = -sin;
            m[1, 0] = sin;
            m[1, 1] = cos;

            //applies it to the transformation
            trans = m * trans;
            count++;
        }

        /// <summary>
        /// Reflects the transformaiton across an arbitrary line that passes 
        /// through the origin. The angle given mesures the displacment from 
        /// the x-axis, mesured in radians.
        /// </summary>
        /// <param name="theta">Angle of reflection</param>
        public void Reflect(double theta)
        {
            //extracts the sin and cos
            double x = Math.Cos(theta);
            double y = Math.Sin(theta);

            //intermediate calculations
            double xx = x * x;
            double yy = y * y;
            double xy = 2.0 * x * y;

            //generates the reflection matrix
            Matrix m = Matrix.Ident(3);
            m[0, 0] = xx - yy;
            m[0, 1] = xy;
            m[1, 0] = xy;
            m[1, 1] = yy - xx;

            //applies it to the transformation
            trans = m * trans;
            count++;
        }

        #endregion //////////////////////////////////////////////////////////

        #region Scaling...

        /// <summary>
        /// Appends a scaling transformation, causing objects processed by the 
        /// transformation to be scaled by a uniform factor.
        /// </summary>
        /// <param name="s">Scaling factor</param>
        public void Scale(double s)
        {
            //generates the scaling matrix
            Matrix m = Matrix.Ident(3);
            m[0, 0] = s;
            m[1, 1] = s;

            //applies it to the transformation
            trans = m * trans;
            count++;
        }

        /// <summary>
        /// Appends a scaling transformation, causing points processed by the
        /// transformation to be scaled allong each axis independently.
        /// </summary>
        /// <param name="x">Scaling in X</param>
        /// <param name="y">Scaling in Y</param>
        public void Scale(double x, double y)
        {
            //generates the scaling matrix
            Matrix m = Matrix.Ident(3);
            m[0, 0] = x;
            m[1, 1] = y;

            //applies it to the transformation
            trans = m * trans;
            count++;
        }

        /// <summary>
        /// Appends a scaling transformation, causing points processed by the
        /// transformation to be scaled allong each axis independently.
        /// </summary>
        /// <param name="s">Scaling factors</param>
        public void Scale(Point2D s)
        {
            //calls the method above
            Scale(s.X, s.Y);
        }       

        #endregion //////////////////////////////////////////////////////////

        #region Shearing...

        /// <summary>
        /// Appends a shearing transformation, causing points to be moved sx
        /// units along the the x axis for each unit along the y axis.
        /// </summary>
        /// <param name="sx">Shearing factor</param>
        public void ShearX(double sx)
        {
            //generates the scaling matrix
            Matrix m = Matrix.Ident(3);
            m[0, 1] = sx;

            //applies it to the transformation
            trans = m * trans;
            count++;
        }

        /// <summary>
        /// Appends a shearing transformation, causing points to be moved sy
        /// units along the the y axis for each unit along the x axis.
        /// </summary>
        /// <param name="sx">Shearing factor</param>
        public void ShearY(double sy)
        {
            //generates the scaling matrix
            Matrix m = Matrix.Ident(3);
            m[1, 0] = sy;

            //applies it to the transformation
            trans = m * trans;
            count++;
        }

        #endregion //////////////////////////////////////////////////////////

        //object ICloneable.Clone()
        //{ return new Transform2D(this); }
    }
}
