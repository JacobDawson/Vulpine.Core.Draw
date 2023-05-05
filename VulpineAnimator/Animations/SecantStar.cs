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
    public class SecantStar : Animation
    {
        private const int MAX = 96; //64;
        private readonly Cmplx Center =
            //new Cmplx(-0.1612983645921304, 0.1612901877716581);
            //new Cmplx(-0.1966877619338244, 0.2035239404723794);
            //new Cmplx(-0.1966877619217374, 0.2035239406218188);
            new Cmplx(-0.161298364592127, 0.161290187771655);
        private readonly Cmplx Zero = new Cmplx(0.0, 0.0); 

        public Color Sample(double u, double v, int frame)
        {
            RootFinder rf = new RootFinder(MAX, VMath.TOL);


            double a = (frame / 3600.0);
            double zoom = (-10.0 * (1 - a)) + (50.0 * a);
            //Cmplx targ = (Zero * (1 - a)) + (Center * 2.0 * a);
            //targ += new Cmplx(u, v) * Math.Pow(2.0, -zoom);

            Cmplx targ = new Cmplx(u, v);
            targ = Center + (targ * Math.Pow(2.0, -zoom));

            //returns black if we are outside our bounds
            if (targ.Abs > 1.5) return Color.FromRGB(0.0, 0.0, 0.0);

            var result = rf.Secant(
                x => Pow4(x * 2.0) - 1.0,
                Zero,
                Zero,
                targ);

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
