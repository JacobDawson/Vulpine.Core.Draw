using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Draw;
using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Numbers;
using Vulpine.Core.Calc.Algorithms;

namespace ImagingTests.Rendering
{
    public class NewtonFractal : Texture 
    {

        private const int MAX = 64;

        //private RootFinder rf = new RootFinder(MAX, VMath.TOL);

        public Color Sample(double u, double v)
        {
            RootFinder rf = new RootFinder(MAX, VMath.TOL);

            //var result = rf.Newton(
            //    x => Pow4(x * 2.0) - 1.0,
            //    dx => 64.0 * dx * dx * dx,
            //    new Cmplx(u, v));

            var result = rf.Newton(
                x => Pow4(x) - 1.0,
                dx => 4.0 * dx * dx * dx,
                new Cmplx(u, v));

            if (result.Iterations < MAX)
            {
                double hue = 0.0;        
                double x = result.Value.CofR;
                double y = result.Value.CofI;

                if (x > y)
                {
                    if (x > -y) hue = 0.0; //red
                    else hue = 60.0; //yellow
                }
                else
                {
                    if (x < -y) hue = 120.0; //green
                    else hue = 180.0; //blue
                }

                double val = result.Iterations / (double)MAX;
                return Color.FromHSV(hue, 1.0, 1.0 - val);

            }
            else
            {
                //returns black if we exaust our number of tries
                return Color.FromRGB(0.0, 0.0, 0.0);
            }


            throw new NotImplementedException();
        }

        private Cmplx Pow4(Cmplx z)
        {
            return z * z * z * z;
        }
    }
}
