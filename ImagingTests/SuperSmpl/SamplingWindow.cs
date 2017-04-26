using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Vulpine.Core.Data.Queues;
using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Geometry;
using Vulpine.Core.Calc.RandGen;

namespace ImagingTests.SuperSmpl
{
    public partial class SamplingWindow : UserControl
    {
        private IEnumerator<Point2D> ittr;
        private bool start;

        private const float DOT = 5.0f;

        public SamplingWindow()
        {
            InitializeComponent();

            ittr = null;
            start = true;
        }

        private IEnumerable<Point2D> GetSampelProvider()
        {
            int index = cboMethod.SelectedIndex;
            int n = 10;
            int k = 6;

            bool test = Int32.TryParse(txtN.Text, out n);
            if (!test) n = 10;

            test = Int32.TryParse(txtK.Text, out k);
            if (!test) k = 6;

            switch (index)
            {
                case 0:
                    return SampleProvider.GetRandom(n * n);
                case 1:
                    return SampleProvider.GetJittred(n);
                case 2:
                    return SampleProvider.GetPoissonA(n * n);
                case 3:
                    return SampleProvider.GetPoissonB(n * n, k);
                default:
                    return SampleProvider.GetRandom(n * n);
            }
        }

        private void SetCurrent()
        {
            ittr = GetSampelProvider().GetEnumerator();
            start = true;

            ittr.MoveNext();
        }

        private void FillAll()
        {
            Graphics gfx = pnlCanvas.CreateGraphics();
            var ittr = GetSampelProvider();
            gfx.Clear(Color.White);

            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            DateTime start = DateTime.Now;
            int count = 0;

            foreach (Point2D p in ittr)
            {
                DrawPoint(gfx, p);
                count++;
            }

            DateTime finish = DateTime.Now;
            lblTime.Text = String.Format("Time: {0}", finish - start);
            lblSample.Text = String.Format("Samples Generated: {0}", count);

            lblTime.Refresh();
            lblSample.Refresh();

            gfx.Dispose();
        }

        private void FillOne()
        {
            Graphics gfx = pnlCanvas.CreateGraphics();
            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            //makes shure we have a providor
            if (ittr == null) SetCurrent();
            if (!ittr.MoveNext()) SetCurrent();

            //cleares the graphics at the start of a run
            if (start)
            {
                gfx.Clear(Color.White);
                start = false;
            }   

            DrawPoint(gfx, ittr.Current);
            gfx.Dispose();
        }

        private void DrawPoint(Graphics gfx, Point2D p)
        {
            gfx.FillEllipse(
                brush: Brushes.Red,
                x: ((float)p.X * 500.0f) - DOT,
                y: ((float)p.Y * 500.0f) - DOT,
                width: DOT * 2.0f,
                height: DOT * 2.0f
            );         
        }

        private void RunTestA(int n)
        {
            Graphics gfx = pnlCanvas.CreateGraphics();
            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            DateTime start = DateTime.Now;

            for (int i = 0; i < 16384; i++)
            {
                gfx.Clear(Color.White);
                var points = SampleProvider.GetPoissonA(n);

                foreach (Point2D p in points)
                    DrawPoint(gfx, p);
            }

            DateTime finish = DateTime.Now;
            lblTime.Text = String.Format("Time: {0}", finish - start);

            gfx.Dispose();
        }

        private void RunTestB(int n)
        {
            Graphics gfx = pnlCanvas.CreateGraphics();
            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            DateTime start = DateTime.Now;

            for (int i = 0; i < 16384; i++)
            {
                gfx.Clear(Color.White);
                var points = SampleProvider.GetPoissonB(n, 6);

                foreach (Point2D p in points)
                {
                    DrawPoint(gfx, p);
                }
            }

            DateTime finish = DateTime.Now;
            lblTime.Text = String.Format("Time: {0}", finish - start);

            gfx.Dispose();
        }

        private void btnFill_Click(object sender, EventArgs e)
        {
            FillAll();
            SetCurrent();
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            FillOne();
        }

        private void btnStep10_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++) FillOne();
        }

        private void cboMethod_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SetCurrent();
        }

        private void btnA_Click(object sender, EventArgs e)
        {
            RunTestA(25);
        }

        private void btnB_Click(object sender, EventArgs e)
        {
            RunTestB(25);
        }
    }
}
