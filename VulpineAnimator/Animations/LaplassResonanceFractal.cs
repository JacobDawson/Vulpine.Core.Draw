using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//LaplassResonanceFractal

using Vulpine.Core.Draw;
using Vulpine.Core.Draw.Textures;
using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Numbers;
using Vulpine.Core.Calc.Algorithms;

namespace VulpineAnimator.Animations
{
    public class LaplassResonanceFractal : Animation
    {
        private const double durmin = 0.5;
        private const double tframes = durmin * 1800.0;
        private const int MAX = 16; //64;
        private const double Scale = 1.0;

        private readonly Cmplx Zero = new Cmplx(0.0, 0.0);

        public Color Sample(double u, double v, int frame)
        {
            RootFinder rf = new RootFinder(MAX, VMath.TOL);
            Cmplx start = new Cmplx(u, v) * Scale;

            var result = rf.Newton(
                BuildFunc(frame),
                BuildDx(frame),
                Zero,
                start);

            if (result.Iterations < MAX)
            {
                double hue = 0.0;
                double abs = result.Value.Abs;

                if (abs > 0.875) hue = 0.0; //red
                else if (abs > 0.625) hue = 60.0; //yellow
                else if (abs > 0.25) hue = 120.0; //green
                else hue = 180.0; //cyan

                double val = result.Iterations / (double)MAX;

                //val = 1.0 - val; //tent           
                val = Math.Cos(Math.PI / 2.0 * val);
                //val = VMath.Sinc(Math.PI * val);
                //val = val * val;
                //val = Math.Sqrt(1.0 - (val * val)); //circle


                return Color.FromHSV(hue, 1.0, val);

            }
            else
            {
                //returns black if we exaust our number of tries
                return Color.FromRGB(0.0, 0.0, 0.0);
            }

            throw new NotImplementedException();
        }

        public Texture GetFrame(int frame)
        {
            return new Frame(this, frame);

            //throw new NotImplementedException();
        }


        public VFunc<Cmplx> BuildFunc(int frame)
        {
            double d0, d1, d2;

            //determins the argument of each root
            d0 = (frame / tframes) * 4.0 * VMath.TAU;
            d1 = (frame / tframes) * 2.0 * VMath.TAU;
            d2 = (frame / tframes) * 1.0 * VMath.TAU;

            double x0, x1, x2, y0, y1, y2;

            //determins the cordinate of each root
            x0 = 0.50 * Math.Cos(d0 + Math.PI);
            x1 = 0.75 * Math.Cos(d1);
            x2 = 1.00 * Math.Cos(d2);

            y0 = 0.50 * Math.Sin(d0 + Math.PI);
            y1 = 0.75 * Math.Sin(d1);
            y2 = 1.00 * Math.Sin(d2);

            Cmplx r0, r1, r2;

            //convertes the roots to complex numbers
            r0 = new Cmplx(x0, y0);
            r1 = new Cmplx(x1, y1);
            r2 = new Cmplx(x2, y2);

            //builds a complex funciton with the given roots
            VFunc<Cmplx> f = z => ((z - r0) * (z - r1) * (z - r2)) / z;

            return f;
        }

        public VFunc<Cmplx> BuildDx(int frame)
        {
            double d0, d1, d2;

            //determins the argument of each root
            d0 = (frame / tframes) * 4.0 * VMath.TAU;
            d1 = (frame / tframes) * 2.0 * VMath.TAU;
            d2 = (frame / tframes) * 1.0 * VMath.TAU;

            double x0, x1, x2, y0, y1, y2;

            //determins the cordinate of each root
            x0 = 0.50 * Math.Cos(d0 + Math.PI);
            x1 = 0.75 * Math.Cos(d1);
            x2 = 1.00 * Math.Cos(d2);

            y0 = 0.50 * Math.Sin(d0 + Math.PI);
            y1 = 0.75 * Math.Sin(d1);
            y2 = 1.00 * Math.Sin(d2);

            Cmplx r0, r1, r2;

            //convertes the roots to complex numbers
            r0 = new Cmplx(x0, y0);
            r1 = new Cmplx(x1, y1);
            r2 = new Cmplx(x2, y2);


            Cmplx prod = r0 * r1 * r2;
            Cmplx sum = r0 + r1 + r2;

            //builds a complex funciton with the given roots
            VFunc<Cmplx> dx = z => (prod / (z * z)) + 2.0 * z - sum;

            return dx;
        }
    }
}

