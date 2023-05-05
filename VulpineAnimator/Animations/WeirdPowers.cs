using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Draw;
using Vulpine.Core.Draw.Textures;
using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Numbers;

using VulpineAnimator.Properties;

namespace VulpineAnimator.Animations
{
    public class WeirdPowers : Animation
    {
        private ImageSys img;

        public WeirdPowers()
        {
            //this.img = Resources.WallClock2;
            this.img = MyRecorces.Wall_Clock;
        }

        public Color Sample(double u, double v, int frame)
        {
            Texture tex = new Interpolent(img, Intpol.Mitchel);
            Texture map = new CmplxMap(tex, z => Power(z, frame));

            return map.Sample(u, v);
        }

        public Texture GetFrame(int frame)
        {
            Texture tex = new Interpolent(img, Intpol.Mitchel);
            Texture map = new CmplxMap(tex, z => Power(z, frame));

            return map;
        }

        private Cmplx Power(Cmplx z, double frame)
        {
            Cmplx zp = z * 3.0;

            double a = frame / 1200.0;
            double exp = (-2.0 * (1 - a)) + (2.0 * a);
            double pow = Math.Exp(exp);

            Cmplx s = VMath.Sign(zp);
            return s * Math.Pow(zp.Abs, 1.0 * pow);
        }
    }
}
