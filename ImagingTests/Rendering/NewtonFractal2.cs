﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Draw;
using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Numbers;
using Vulpine.Core.Calc.Algorithms;

namespace ImagingTests.Rendering
{
    public class NewtonFractal2 : Texture
    {

        private const int MAX = 64;

        public Color Sample(double u, double v)
        {
            RootFinder rf = new RootFinder(MAX, VMath.TOL);

            var result = rf.Secant(
                x => Pow4(x * 2.0) - 1.0,
                new Cmplx(0.0, 0.0),
                new Cmplx(0.0, 0.0),
                new Cmplx(u, v));

            if (result.Count < MAX)
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

                double val = result.Count / (double)MAX;
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
