using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Draw;
using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Numbers;
using Vulpine.Core.Calc.Algorithms;

namespace VulpineAnimator.Animations
{
    public class SecantRotation : Animation
    {
        private const int MAX = 64; //64;
        private readonly Cmplx Zero = new Cmplx(0.0, 0.0);

        private const double Scale = 2.0; //2.0;

        public Color Sample(double u, double v, int frame)
        {
            RootFinder rf = new RootFinder(MAX, VMath.TOL);


            //double a = (frame / 3600.0);
            //double zoom = (-10.0 * (1 - a)) + (50.0 * a);
            ////Cmplx targ = (Zero * (1 - a)) + (Center * 2.0 * a);
            ////targ += new Cmplx(u, v) * Math.Pow(2.0, -zoom);


            //we select our z1 to be on the unit circle
            double rot = (frame / 1800.0) * VMath.TAU;
            double z1_r = Math.Sin(rot);
            double z1_i = Math.Cos(rot);
            Cmplx z1 = new Cmplx(z1_r, z1_i);

            //we use our input point as our z2
            Cmplx z2 = new Cmplx(u, v) * Scale;

            var result = rf.Secant(
                x => Pow4(x * 2.0) - 1.0,
                Zero,
                z1,
                z2);

            if (result.Iterations < MAX)
            {
                double hue = 0.0;
                double x = result.Value.CofR;
                double y = result.Value.CofI;

                if (x > y)
                {
                    if (x > -y) hue = 0.0; //red
                    else hue = 90.0; //grenish
                }
                else
                {
                    if (x < -y) hue = 180.0; //cyan
                    else hue = 270.0; //purple
                }

                double val = result.Iterations / (double)MAX;
                return Color.FromHSV(hue, 1.0, 1.0 - val);

            }
            else
            {
                //returns black if we exaust our number of tries
                return Color.FromRGB(0.0, 0.0, 0.0);
            }
        }

        public Texture GetFrame(int frame)
        {
            return new Frame(this, frame);

            //throw new NotImplementedException();
        }

        private Cmplx Pow4(Cmplx z)
        {
            return z * z * z * z;
        }
    }


}
