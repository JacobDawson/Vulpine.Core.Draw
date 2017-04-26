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
        public Color GetValue(double u, double v)
        {
            Point2D targ = trans.Trans(u, v);
            return inner.GetValue(targ.X, targ.Y);
        }
    }
}
