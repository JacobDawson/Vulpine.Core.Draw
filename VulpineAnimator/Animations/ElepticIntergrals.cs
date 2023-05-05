using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Draw;
using Vulpine.Core.Draw.Textures;
using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Numbers;
using Vulpine.Core.Calc.Algorithms;
using Vulpine.Core.Calc.Matrices;

namespace VulpineAnimator.Animations
{
    class ElepticIntergrals : Animation
    {
        private const double Scale = 12.0; //6.0; // 20.0;
        private const double MaxFrames = 1800.0;



        public Color Sample(double u, double v, int frame)
        {
            Cmplx phi = new Cmplx(u, v);
            phi = phi * Scale;


            //Cmplx m = GetM_Real(frame);
            Cmplx m = GetM_Unit(frame);

            //Cmplx x = F_Basic(phi, m);
            //Cmplx x = Jacobi.SN(phi, m);
            Cmplx x = Jacobi.SN(m, phi);


            //return GetSpherical(x.CofR, x.CofI);
            return GetModulated(x.CofR, x.CofI);

            //throw new NotImplementedException();
        }

        public Texture GetFrame(int frame)
        {
            return new Frame(this, frame);

            //throw new NotImplementedException();
        }


        public Cmplx GetM_Real(int frame)
        {
            //converts to range 0 .. 1
            double x = (double)frame / MaxFrames;

            x = x * Math.PI * 2.0;
            x = Math.Sin(x) * 2.0;

            ////converts to range -2 .. 2
            //x = (x * 4.0) - 2.0;

            return new Cmplx(x, 0.0);
        }

        public Cmplx GetM_Imag(int frame)
        {
            //converts to range 0 .. 1
            double x = (double)frame / MaxFrames;

            x = x * Math.PI * 2.0;
            x = Math.Sin(x) * 2.0;

            ////converts to range -2 .. 2
            //x = (x * 4.0) - 2.0;

            return new Cmplx(0.0, x);
        }

        public Cmplx GetM_Unit(int frame)
        {
            //converts to range 0 .. 1
            double x = (double)frame / MaxFrames;

            //converts to range -1 .. 1
            x = (x * 2.0) - 1.0;

            //converts to -pi .. pi
            x = x * Math.PI; // Math.Asin(x);

            double real = Math.Sin(x);
            double imag = Math.Cos(x);

            return new Cmplx(real, imag);
        }

        public static Cmplx F_Basic(Cmplx phi, Cmplx m)
        {
            Cmplx x = Cmplx.Sin(phi);

            //uses the Legerande normal form
            VFunc<Cmplx> intgnd = delegate(Cmplx t)
            {
                Cmplx t2 = t * t;
                Cmplx a = 1.0 - t2;
                Cmplx b = 1.0 - (m * t2);

                //splits the radical to avoid branch cuts
                a = Cmplx.Sqrt(a);
                b = Cmplx.Sqrt(b);

                return 1.0 / (a * b);
            };

            Cmplx intg = Integrator.Kronrod(intgnd, 0.0, x);


            return intg;
        }


        public static Cmplx F_Wolfram(Cmplx phi, Cmplx m)
        {
            int rx = (int)Math.Round(phi.CofR / Math.PI);
            Cmplx k2 = 2.0 * Jacobi.K(m);

            Cmplx x = Cmplx.Sin(phi);

            //uses the Legerande normal form
            VFunc<Cmplx> intgnd = delegate(Cmplx t)
            {
                Cmplx t2 = t * t;
                Cmplx a = 1.0 - t2;
                Cmplx b = 1.0 - (m * t2);

                //splits the radical to avoid branch cuts
                a = Cmplx.Sqrt(a);
                b = Cmplx.Sqrt(b);

                return 1.0 / (a * b);
            };

            Cmplx intg = Integrator.Kronrod(intgnd, 0.0, x);

            if (rx % 2 != 0) intg = -intg;
            return intg + (k2 * rx);
        }


        private Color GetSpherical(double x, double y)
        {
            //obtains the polar cordinates
            double r = Math.Sqrt((x * x) + (y * y));
            double t = Math.Atan2(y, x);

            double hue = VMath.ToDeg(t);
            double lum = (-1.0 / (r + 1.0)) + 1.0;

            return Color.FromHSL(hue, 1.0, lum);

            //Color c = Color.FromHSL(hue, 1.0, lum);
            //return c.SetGamma(1.0 / 2.22);
        }


        private Color GetModulated(double x, double y)
        {
            //obtains the polar cordinates
            double r = Math.Sqrt((x * x) + (y * y));
            double t = Math.Atan2(y, x);

            double hue, val;

            hue = VMath.ToDeg(t);
            val = VMath.Log2(r);
            val = val - Math.Floor(val);
            val = (val * 0.5) + 0.5;

            return Color.FromHSV(hue, 1.0, val);
        }
    }
}
