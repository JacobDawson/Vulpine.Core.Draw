using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Draw;

namespace VulpineAnimator
{
    public interface Animation
    {
        Color Sample(double u, double v, int frame);

        Texture GetFrame(int frame);
    }

    public class Frame : Texture
    {
        private Animation ani;
        private int frame;

        public Frame(Animation ani, int frame)
        {
            this.ani = ani;
            this.frame = frame;
        }

        public Color Sample(double u, double v)
        {
            return ani.Sample(u, v, frame);
        }
    }
}
