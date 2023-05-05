using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Matrices;
using Vulpine.Core.Calc.Geometry.Planer;

namespace Vulpine.Core.Draw
{
    public abstract class Canvas
    {

        public abstract Color Forground { get; set; }

        public abstract Color Backgorund { get; set; }

        public abstract double LineThickness { get; set; }

        public abstract Intpol Interpolation { get; set; }

        public abstract bool DoubleBuffer { get; }


        public abstract void DrawLine(double x0, double y0, double x1, double y1);

        public abstract void DrawLine(double x0, double y0, double x1, double y1, 
            Color c0, Color c1);

        public abstract void FillTriangle(double x0, double y0, double x1, double y1, 
            double x2, double y2);

        public abstract void FillTriangle(double x0, double y0, double x1, double y1, 
            double x2, double y2, Color c0, Color c1, Color c2);

        public abstract void DrawImage(String lable, double xs, double ys, double ws, double hs, 
            double xt, double yt, double wt, double ht);

        public abstract void DrawImage(String lable, double xs, double ys, double ws, double hs,
            Trans2D transform);

        public abstract void DrawImage(String lable, Rectangle source, Trans2D transform, Color tint);

        public abstract void DrawImage(String lable, Rectangle source, Rectangle target, Color tint);

        public abstract void DrawText(String text, double x, double y, double w, double h);


        //public abstract void SetBrush(Color color, double thickness);

        public abstract bool AddResource(Image img, String lable);

        public abstract bool AddResource(Object obj, String lable);


        public abstract void Clear();

        public abstract void Flush();

        public abstract void FlushAndClear();

        public abstract void FlushToImage(Image target);


        //////////////////////////////////////////////////////////////////////////////////////

        //public abstract void DrawLine(double x0, double y0, double x1, double y1);

        //public abstract void DrawLine(double x0, double y0, double x1, double y1, Color c, double t);

        //public abstract void DrawLine(double x0, double y0, double x1, double y1, Color c0, Color c1, double t);

        //public abstract void FillTriangle(double x0, double y0, double x1, double y1, double x2, double y2);

        //public abstract void FillTriangle(double x0, double y0, double x1, double y1, double x2, double y2, Color c);

        //public abstract void FillTriangle(double x0, double y0, double x1, double y1, double x2, double y2, Color c0, Color c1, Color c2);

        ////public abstract void FillTriangle(double x0, double y0, double x1, double y1, double x2, double y2, String texture, double u0, double v0, double u1, double v1, double u2, double v2);

        //public abstract void DrawImage(String lable, double xs, double ys, double xt, double yt, double width, double height);

        //public abstract void DrawImage(String lable, double xs, double ys, double ws, double hs, double xt, double yt, double wt, double ht);
    }
}
