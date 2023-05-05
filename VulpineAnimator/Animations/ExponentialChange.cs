using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Draw;
using Vulpine.Core.Draw.Textures;
using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Numbers;
using Vulpine.Core.Calc.Algorithms;

namespace VulpineAnimator.Animations
{
    public class ExponentialChange : Animation
    {
        private const double Scale = 2.0; //2.0;
        private const double pow = 6.0; 


        private const double dursec = 80.0; //2400 frames
        private const double tframes = dursec * 30.0;


        public Color Sample(double u, double v, int frame)
        {
            Texture tex = GetFrame(frame);
            return tex.Sample(u, v);
        }

        public Texture GetFrame(int frame)
        {
            //Let Alpha run from 0 to 8
            //our base power is equal to 6

            double alpha = (frame / tframes) * 8.0;

            //double scale = 1.0;
            //double scale = 720.0 / VMath.Gamma(6.0 - alpha + 1.0);
            double scale = 1.0 / VMath.Gamma(6.0 - alpha + 1.0);

            VFunc<Cmplx> f = z => scale * Cmplx.Pow(z, 6.0 - alpha);
            Texture source = ColorWheel.Modulated;
            Texture map = new CmplxMap(source, f);

            return map;
        }
    }
}
