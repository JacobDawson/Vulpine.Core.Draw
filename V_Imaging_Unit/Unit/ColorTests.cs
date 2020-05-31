using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using NUnit.Framework.Constraints;
using Vulpine_Core_Draw_Tests.AddOns;

using Vulpine.Core.Draw;
using Vulpine.Core.Draw.Colors;
using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Matrices;

namespace Vulpine_Core_Draw_Tests.Unit
{
    [TestFixture]
    public class ColorTests
    {
        public const double TOL = 1.0e-06;

        private dynamic GetColor(int index)
        {
            switch (index)
            {
                case 1: return Color.FromRGB(0.1, 0.1, 0.1);
                case 2: return Color.FromRGB(0.0, 0.0, 0.5);
                case 3: return Color.FromRGB(0.0, 0.0, 1.0);
                case 4: return Color.FromRGB(0.0, 0.5, 0.0);
                case 5: return Color.FromRGB(0.0, 0.5, 0.5);
                case 6: return Color.FromRGB(0.0, 0.5, 1.0);
                case 7: return Color.FromRGB(0.0, 1.0, 0.0);
                case 8: return Color.FromRGB(0.0, 1.0, 0.5);
                case 9: return Color.FromRGB(0.0, 1.0, 1.0);
                case 10: return Color.FromRGB(0.5, 0.0, 0.0);
                case 11: return Color.FromRGB(0.5, 0.0, 0.5);
                case 12: return Color.FromRGB(0.5, 0.0, 1.0);
                case 13: return Color.FromRGB(0.5, 0.5, 0.0);
                case 14: return Color.FromRGB(0.5, 0.5, 0.5);
                case 15: return Color.FromRGB(0.5, 0.5, 1.0);
                case 16: return Color.FromRGB(0.5, 1.0, 0.0);
                case 17: return Color.FromRGB(0.5, 1.0, 0.5);
                case 18: return Color.FromRGB(0.5, 1.0, 1.0);
                case 19: return Color.FromRGB(1.0, 0.0, 0.0);
                case 20: return Color.FromRGB(1.0, 0.0, 0.5);
                case 21: return Color.FromRGB(1.0, 0.0, 1.0);
                case 22: return Color.FromRGB(1.0, 0.5, 0.0);
                case 23: return Color.FromRGB(1.0, 0.5, 0.5);
                case 24: return Color.FromRGB(1.0, 0.5, 1.0);
                case 25: return Color.FromRGB(1.0, 1.0, 0.0);
                case 26: return Color.FromRGB(1.0, 1.0, 0.5);
                case 27: return Color.FromRGB(1.0, 1.0, 1.0);
            }

            Assert.Inconclusive("INVALID INDEX GIVEN!!");
            throw new InvalidOperationException();
        }
        
        [Test]
        public void ToHSL_BackToRGB_Match([Range(1, 27)] int index)
        {
            Color c = GetColor(index);
            
            Vector hsl = c.ToHSL();
            Color back = Color.FromHSL(hsl);

            Vector cv = c.ToRGB();
            Vector bv = back.ToRGB();

            Assert.That(bv, Ist.WithinTolOf(cv, TOL));
        }

        [Test]
        public void ToHSV_BackToRGB_Match([Range(1, 27)] int index)
        {
            Color c = GetColor(index);

            Vector hsv = c.ToHSV();
            Color back = Color.FromHSV(hsv);

            Vector cv = c.ToRGB();
            Vector bv = back.ToRGB();

            Assert.That(bv, Ist.WithinTolOf(cv, TOL));
        }

        [Test]
        public void ToYUV_BackToRGB_Match([Range(1, 27)] int index)
        {
            Color c = GetColor(index);

            Vector yuv = c.ToYUV();
            Color back = Color.FromYUV(yuv);

            Vector cv = c.ToRGB();
            Vector bv = back.ToRGB();

            Assert.That(bv, Ist.WithinTolOf(cv, TOL));
        }

        [Test]
        public void ToXYZ_BackToRGB_Match([Range(1, 27)] int index)
        {
            Color c = GetColor(index);

            Vector xyz = c.ToXYZ();
            Color back = Color.FromXYZ(xyz);

            Vector cv = c.ToRGB();
            Vector bv = back.ToRGB();

            Assert.That(bv, Ist.WithinTolOf(cv, TOL));
        }

        [Test]
        public void ToLAB_BackToRGB_Match([Range(1, 27)] int index)
        {
            Color c = GetColor(index);

            Vector lab = c.ToLAB();
            Color back = Color.FromLAB(lab);

            Vector cv = c.ToRGB();
            Vector bv = back.ToRGB();

            Assert.That(bv, Ist.WithinTolOf(cv, TOL));
        }

        [Test]
        public void ToLUV_BackToRGB_Match([Range(1, 27)] int index)
        {
            Color c = GetColor(index);

            Vector luv = c.ToLUV();
            Color back = Color.FromLUV(luv);

            Vector cv = c.ToRGB();
            Vector bv = back.ToRGB();

            Assert.That(bv, Ist.WithinTolOf(cv, TOL));
        }
    }
}
