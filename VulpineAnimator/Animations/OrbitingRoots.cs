using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Draw;
using Vulpine.Core.Draw.Textures;
using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Numbers;
using Vulpine.Core.Calc.Algorithms;

namespace VulpineAnimator.Animations
{
    public class OrbitingRoots : Animation
    {
        private const double durmin = 4.0;
        private const double tframes = durmin * 1800.0;

        public Color Sample(double u, double v, int frame)
        {
            Texture tex = GetFrame(frame);
            return tex.Sample(u, v);

            //throw new NotImplementedException();
        }

        public Texture GetFrame(int frame)
        {
            double d0, d1, d2;

            //determins the argument of each root
            d0 = (frame / tframes) * 35.0 * VMath.TAU;
            d1 = (frame / tframes) * 21.0 * VMath.TAU;
            d2 = (frame / tframes) * 15.0 * VMath.TAU;

            double x0, x1, x2, y0, y1, y2;

            //determins the cordinate of each root
            x0 = 0.50 * Math.Cos(d0);
            x1 = 0.75 * Math.Cos(d1);
            x2 = 1.00 * Math.Cos(d2);

            y0 = 0.50 * Math.Sin(d0);
            y1 = 0.75 * Math.Sin(d1);
            y2 = 1.00 * Math.Sin(d2);

            Cmplx r0, r1, r2;

            //convertes the roots to complex numbers
            r0 = new Cmplx(x0, y0);
            r1 = new Cmplx(x1, y1);
            r2 = new Cmplx(x2, y2);

            //builds a complex funciton with the given roots
            VFunc<Cmplx> f = z => ((z - r0) * (z - r1) * (z - r2)) / z;
            Texture map = new CmplxMap(ColorWheel.Normal, f);

            return map;

            //throw new NotImplementedException();
        }
    }
}
