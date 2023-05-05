using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Draw;
using Vulpine.Core.Draw.Filters;
using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Matrices;

namespace ImagingTests.Filters
{
    public class FilterSobel2 : Filter
    {
        private readonly Matrix vert = new Matrix(3, 3,
            +1, +0, -1,
            +2, +0, -2,
            +1, +0, -1);

        private readonly Matrix horz = new Matrix(3, 3,
            +1, +2, +1,
            +0, +0, +0,
            -1, -2, -1);

        public override int Size
        {
            get { return 3; }
        }

        public override Color Sample(Image source, int x, int y)
        {
            double dx = 0.0;
            double dy = 0.0;

            double a00 = source[x - 1, y - 1].Luminance;
            double a01 = source[x - 1, y + 0].Luminance;
            double a02 = source[x - 1, y + 1].Luminance;
            double a10 = source[x + 0, y - 1].Luminance;
            //double a11 = source[x + 0, y + 0].Luminance;
            double a12 = source[x + 0, y + 1].Luminance;
            double a20 = source[x + 1, y - 1].Luminance;
            double a21 = source[x + 1, y + 0].Luminance;
            double a22 = source[x + 1, y + 1].Luminance;

            dx = (a00 + a10 + a10 + a20) - (a02 + a12 + a12 + a22);
            dy = (a00 + a01 + a01 + a02) - (a20 + a21 + a21 + a22);

            double mag = dx * dx + dy * dy;
            mag = Math.Sqrt(mag);

            double arg = Math.Atan2(dy, dx);
            arg = VMath.ToDeg(arg);

            //if (mag > 1.0) return Color.Black;
            //return Color.White;

            return Color.FromHSV(arg, 1.0, mag);


            //throw new NotImplementedException();
        }
    }
}
