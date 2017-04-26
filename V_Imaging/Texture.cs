using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc.Exceptions;

namespace Vulpine.Core.Draw
{
    /// <summary>
    /// A Texture is conceptualy a continious image. It can be evaluated at any point in space,
    /// not just at the individual pixels. Internaly a texture could be an interpolated image,
    /// a rendered scean, or some other proceduarly generated data. Due to there procedural 
    /// nature, all textures are read-only. Textures are prefered over Images whenever sub-pixel 
    /// percision is nessary.
    /// <remarks>Last Update: 2016-02-01</remarks>
    /// </summary>
    public interface Texture
    {
        /// <summary>
        /// Extracts the color data from the texture at an explicit point,
        /// in texture cordinates, where the principle width and height of
        /// the texture are equal to one.
        /// </summary>
        /// <param name="u">The u texture cordinate</param>
        /// <param name="v">The v texture cordinate</param>
        /// <returns>The color sampled at the given point</returns>
        Color GetValue(double u, double v);
    }
}
