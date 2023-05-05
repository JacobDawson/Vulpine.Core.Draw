using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc;

namespace Vulpine.Core.Draw.Filters
{
    public class FilterSobel : Filter
    {
        private int mode;

        private FilterSobel(int mode)
        {
            this.mode = mode;
        }

        public override int Size
        {
            get { return 3; }
        }

        public static FilterSobel Horizontal
        {
            get { return new FilterSobel(0); }
        }

        public static FilterSobel Vertical
        {
            get { return new FilterSobel(1); }
        }

        public static FilterSobel Magnitude
        {
            get { return new FilterSobel(2); }
        }

        public static FilterSobel Gradient
        {
            get { return new FilterSobel(3); }
        }

        

        public override Color Sample(Image source, int x, int y)
        {
            //used in evaluating the sobel filter
            double dx, dy, a00, a01, a02, a10, a12, a20, a21, a22;

            //obtains the points surounding the sample point
            a00 = source[x - 1, y - 1].Luminance;
            a01 = source[x - 1, y + 0].Luminance;
            a02 = source[x - 1, y + 1].Luminance;
            a10 = source[x + 0, y - 1].Luminance;
            a12 = source[x + 0, y + 1].Luminance;
            a20 = source[x + 1, y - 1].Luminance;
            a21 = source[x + 1, y + 0].Luminance;
            a22 = source[x + 1, y + 1].Luminance;

            //computes the derivitive in the X and Y directions
            dx = (a00 + a10 + a10 + a20) - (a02 + a12 + a12 + a22);
            dy = (a00 + a01 + a01 + a02) - (a20 + a21 + a21 + a22);

            switch (mode % 4)
            {
                case 0: return GetHorz(dx, dy);
                case 1: return GetVert(dx, dy);
                case 2: return GetMag(dx, dy);
                case 3: return GetGrad(dx, dy);
            }

            //we should never reach this state
            throw new NotImplementedException();
        }

        

        private Color GetHorz(double dx, double dy)
        {
            double abs = Math.Abs(dx);
            return Color.FromRGB(abs, abs, abs);
        }

        private Color GetVert(double dx, double dy)
        {
            double abs = Math.Abs(dy);
            return Color.FromRGB(abs, abs, abs);
        }

        private Color GetMag(double dx, double dy)
        {
            double mag = dx * dx + dy * dy;
            mag = Math.Sqrt(mag);
            return Color.FromRGB(mag, mag, mag);
        }

        private Color GetGrad(double dx, double dy)
        {
            double mag = dx * dx + dy * dy;
            mag = Math.Sqrt(mag);

            double arg = Math.Atan2(dy, dx);
            arg = VMath.ToDeg(arg);

            return Color.FromHSV(arg, 1.0, mag);
        }
    }
}
