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
    public class FountainSpiral : Animation
    {
        private ImageSys img;

        public FountainSpiral()
        {
            //this.img = Resources.WallClock2;
            this.img = MyRecorces.Wall_Clock;
        }

        public Color Sample(double u, double v, int frame)
        {
            Texture tex = new Interpolent(img, Intpol.Mitchel);
            Texture map = new CmplxMap(tex, z => LogLog(z, frame));

            return map.Sample(u, v);
        }

        public Texture GetFrame(int frame)
        {
            Texture tex = new Interpolent(img, Intpol.Mitchel);
            Texture map = new CmplxMap(tex, z => LogLog(z, frame));

            return map;
        }

        private Cmplx LogLog(Cmplx z, int frame)
        {
            double a = frame / 2400.0;
            double zoom = (30.0 * (1 - a)) + (-10.0 * a);

            Cmplx zz = z * Math.Pow(2.0, zoom);
            return Cmplx.Log(Cmplx.Log(zz + 1.0));
        }

        private Cmplx Pow4(Cmplx z)
        {
            return z * z * z * z;
        }
    }
}
