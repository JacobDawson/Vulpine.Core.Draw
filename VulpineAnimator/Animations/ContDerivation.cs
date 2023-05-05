using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Vulpine.Core.Draw;
using Vulpine.Core.Draw.Textures;
using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Numbers;
using Vulpine.Core.Calc.Algorithms;

using VulpineAnimator.Properties;


namespace VulpineAnimator.Animations
{
    public class ContDerivation : Animation
    {
        private const double Scale = 2.0; //2.0;
        private const double pow = 6.0;


        private const double dursec = 60.0; //1800 frames
        private const double tframes = dursec * 30.0;


        private ImageSys img;
        private Texture sphere;

        public ContDerivation()
        {
            //img = Resources.Rocket_Alley;
            img = MyRecorces.Rocket_Alley;
            sphere = new Interpolent(img, Intpol.Mitchel);
        }


        public Color Sample(double u, double v, int frame)
        {
            Texture tex = GetFrame(frame);
            return tex.Sample(u, v);
        }

        public Texture GetFrame(int frame)
        {
            //Let Alpha run from 0 to 5
            //our base power is equal to 6

            double alpha = (frame / tframes) * 5.0;

            VFunc<Cmplx> f = z => Function(z, alpha);

            //Texture source = new Stereograph(sphere);
            Texture source = ColorWheel.Normal;
            Texture map = new CmplxMap(source, f);

            return map;
        }


        private Cmplx Function(Cmplx x, double a)
        {
            Cmplx d4 = EvalDeriv2(x, new Cmplx(1.0), 4.0, a);
            Cmplx d3 = EvalDeriv2(x, new Cmplx(-0.4), 3.0, a);
            Cmplx d2 = EvalDeriv2(x, new Cmplx(0.08, 0.2), 2.0, a);
            Cmplx d1 = EvalDeriv2(x, new Cmplx(-0.376, -0.28), 1.0, a);
            Cmplx d0 = EvalDeriv2(x, new Cmplx(0.1536, 0.288), 0.0, a);

            return d4 + d3 + d2 + d1 + d0;
        }

        private Cmplx EvalDeriv(Cmplx x, Cmplx c, double n, double a)
        {
            //D^a [c x^n] == (c * gamma(n + 1) / gamma(n - a + 1)) * x^(n - a)

            if ((n - a) > -1.0)
            {
                double g = VMath.Gamma(n + 1.0) / VMath.Gamma(n - a + 1.0);
                return Cmplx.Pow(x, n - a) * g * c;
            }
            else
            {
                return new Cmplx(0.0);
            }

            //double g = VMath.Gamma(n + 1.0) / VMath.Gamma(n - a + 1.0);
            //return Cmplx.Pow(x, n - a) * g * c;
        }

        private Cmplx EvalDeriv2(Cmplx x, Cmplx c, double n, double a)
        {
            //D^a [c x^n] == (c * gamma(n + 1) / gamma(n - a + 1)) * x^(n - a)

            double t = n - a;

            if (t > 0.0)
            {
                double g = VMath.Gamma(n + 1.0) / VMath.Gamma(t + 1.0);
                return Cmplx.Pow(x, t) * g * c;
            }
            else if (t > -1.0)
            {
                double g = VMath.Gamma(n + 1.0) / VMath.Gamma(t + 1.0);
                return 1.0 * g * c;
            }
            else
            {
                return new Cmplx(0.0);
            }
        }
    }
}
