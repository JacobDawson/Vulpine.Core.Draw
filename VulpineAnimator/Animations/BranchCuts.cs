using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Geometry;
using Vulpine.Core.Calc.Numbers;
using Vulpine.Core.Draw;
using Vulpine.Core.Draw.Textures;

using VRectangle = Vulpine.Core.Calc.Geometry.Planer.Rectangle;
using SRectangle = System.Drawing.Rectangle;

namespace VulpineAnimator.Animations
{
    public class BranchCuts : Animation
    {
        //VFunc<Cmplx> func;
        Texture source = ColorWheel.Normal;

        public Color Sample(double u, double v, int frame)
        {
            double theta = (frame / 240.0) * VMath.TAU;
            VFunc<Cmplx> func = z => Sqrt2(z, theta);
            CmplxMap map = new CmplxMap(source, func, 2.0);

            return map.Sample(u, v);
        }

        public Texture GetFrame(int frame)
        {
            double theta = (frame / 240.0) * VMath.TAU;
            VFunc<Cmplx> func = z => Sqrt2(z, theta);
            CmplxMap map = new CmplxMap(source, func, 2.0);

            return map;
        }

        /***************************************************************************/

        public static double RadNorm3(double rad, double n)
        {
            //double ni = Math.Floor(n);
            //double nf = n - ni;

            ////this code rotates counter-clockwise
            //double sec = Math.Floor((rad + n) / VMath.TAU);
            //return ((rad / VMath.TAU) - sec) * VMath.TAU;


            double sec = Math.Floor((rad - n) / VMath.TAU);
            //return ((rad / VMath.TAU) - sec) * VMath.TAU;
            return rad - sec * VMath.TAU;

            // Mod(rad - n, VMath.Tau) + n
        }

        public static Cmplx Sqrt2(Cmplx z, double n)
        {
            //calculates the result storing temporary values
            double rad = Math.Sqrt(z.Abs);
            double arg = RadNorm3(z.Arg, n - Math.PI) / 2.0;
            double outR = rad * Math.Cos(arg);
            double outI = rad * Math.Sin(arg);

            //returns the constructed number
            return new Cmplx(outR, outI);
        }
    }
}
