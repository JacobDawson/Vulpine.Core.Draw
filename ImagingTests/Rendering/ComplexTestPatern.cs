using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Draw;
using Vulpine.Core.Calc.Numbers;

namespace ImagingTests.Rendering
{
    public class ComplexTestPatern : Texture
	{
        public Color Sample(double u, double v)
        {
            //double x = (u * 0.25) - 0.25;
            //double y = ((1.0 - v) * 0.25) - 0.125;

            double x = u * 0.25;
            double y = v * 0.25;

            Cmplx c = new Cmplx(x, y);
            c = Cmplx.Sin(c.Inv());

            return CmplxToColor(c);
        }

        private static Color CmplxToColor(Cmplx z)
        {
            //takes care of the extreem cases
            //if (Cmplx.IsNaN(z) || Cmplx.IsInfinity(z))
            if (z.IsInfinity() || z.IsNaN())
            return Color.FromRGB(1.0, 1.0, 1.0);

            double lum = (-1.0 / (z.Abs + 1.0)) + 1.0;
            double hue = z.Arg * (180.0 / Math.PI);
            return Color.FromHSL(hue, 1.0, lum);
        }
    }
}
