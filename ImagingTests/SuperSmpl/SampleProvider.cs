using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Data.Queues;
using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Geometry;
using Vulpine.Core.Calc.RandGen;

namespace ImagingTests.SuperSmpl
{
    public static class SampleProvider
    {
        private static VRandom rng;
        private const int MaxTry = 75;

        static SampleProvider()
        {
            rng = new RandXOR();
        }


        public static IEnumerable<Point2D> GetRandom(int c)
        {
            for (int i = 0; i < c; i++)
            {
                double u = rng.NextDouble();
                double v = rng.NextDouble();
                yield return new Point2D(u, v);
            }
        }

        public static IEnumerable<Point2D> GetJittred(int cr)
        {
            double u, v;

            //selects a random point inside each cell
            for (int i = 0; i < cr; i++)
            {
                for (int j = 0; j < cr; j++)
                {
                    double u0 = (double)i / cr;
                    double u1 = (double)(i + 1) / cr;

                    double v0 = (double)j / cr;
                    double v1 = (double)(j + 1) / cr;

                    u = rng.RandDouble(u0, u1);
                    v = rng.RandDouble(v0, v1);
                    yield return new Point2D(u, v);
                }
            }
        }

        public static IEnumerable<Point2D> GetPoissonA(int c)
        {
            Point2D[] points = new Point2D[c];
            //Deque<Point2D> queue = new DequeArray<Point2D>(c);

            double mindist = 0.75 / Math.Sqrt(c);

            //generates a random point to start with
            double u = rng.NextDouble();
            double v = rng.NextDouble();

            points[0] = new Point2D(u, v);
            //queue.Push(points[0]);
            yield return points[0];

            int i = 1;
            int t = 0;

            while ((i < c) && (t < MaxTry))
            {
                //generates a random canidate point
                u = rng.NextDouble();
                v = rng.NextDouble();
                Point2D can = new Point2D(u, v);
                bool pass = true;

                t++;

                //checks that the canidate is not too close
                for (int k = 0; k < i; k++)
                {
                    pass = (can.Dist(points[k]) > mindist);
                    if (!pass) break;
                }

                //includes the point only if it passes
                if (pass)
                {
                    points[i] = can;
                    i++; t = 0;
                    yield return can;
                }
            }
        }


        public static IEnumerable<Point2D> GetPoissonB(int c, int k)
        {
            Deque<Point2D> processes = new DequeArray<Point2D>(c);
            Deque<Point2D> samples = new DequeArray<Point2D>(c);

            double mindist = 0.75 / Math.Sqrt(c);

            double u = rng.RandDouble(0.25, 0.75);
            double v = rng.RandDouble(0.25, 0.75);

            //starts the process in the dead center
            Point2D p0 = new Point2D(0.5, 0.5);
            processes.PushBack(p0);
            samples.PushBack(p0);
            yield return p0;

            //int count = 0;

            while (!processes.Empty && (samples.Count < c))
            {
                Point2D samp = processes.PopFront();

                for (int i = 0; i < k; i++)
                {
                    //selects a point in the neighborhood of our current sample
                    //Point2D can = rng.PointCircle(samp, mindist, mindist * 2.0);
                    double dist = rng.RandDouble(mindist, mindist * 2.0);
                    Point2D can = samp + (Point2D)(rng.RandUnit(2) * dist);
                    bool fail = false;

                    //tests for inclusion in the unit square
                    fail |= (can.X < 0.0) | (can.X > 1.0);
                    fail |= (can.Y < 0.0) | (can.Y > 1.0);

                    //tests agenst EACH POINT for min distance
                    foreach (Point2D p in samples)
                    {
                        fail |= (p.Dist(can) < mindist);
                        if (fail) break;
                    }

                    //adds the canidate if it passes
                    if (!fail)
                    {
                        processes.PushFront(can);
                        samples.PushFront(can);
                        yield return can;
                    }
                }
            }

        }


        public static IEnumerable<Point2D> GetPoissonDisk(double delta)
        {
            Deque<Point2D> queue = new DequeArray<Point2D>(1024);

            //starts at the origin
            double u = 0.0;
            double v = 0.0;

            //starts at the origin
            Point2D can = new Point2D(u, v);
            queue.PushBack(can);
            yield return can;

            int i = 0;
            int t = 0;

            double max_r = 1.0 - delta;

            while ((t < 12) && (i < 10000000))
            {
                //generates a random canidate point
                u = rng.RandDouble(-1.0, 1.0);
                v = rng.RandDouble(-1.0, 1.0);
                can = new Point2D(u, v);
                bool pass = true;

                //ignores points outside the unit disk
                if (can.Radius > max_r) continue;

                t++; i++;

                //checks that the canidate is not too close
                foreach (Point2D p in queue)
                {
                    pass = (can.Dist(p) > delta);
                    if (!pass) break;
                }

                //includes the point only if it passes
                if (pass)
                {
                    queue.PushBack(can);
                    t = 0;
                    yield return can;
                }
            }
        }

        


        public static IEnumerable<Point2D> GetHex(int n)
        {
            //n = n / 2;

            //double a = 2.0 / ((2.0 * n) + 1.0);
            double a = 1.0 / n;
            double b = a * 0.86602540378443864676;

            int m = (int)Math.Floor(1.0 / b);
            double x, y;

            for (int j = -m; j <= m; j++)
            {
                for (int i = -n; i <= n; i++)
                {
                    x = a * i;
                    y = b * j;

                    //shifts by half for odd rows
                    if (j % 2 != 0) x += a / 2.0;

                    ////jitters each point
                    //x += rng.RandGauss(0.0, a / 6.0);
                    //y += rng.RandGauss(0.0, a / 6.0);

                    double x0 = (x + 1.0) * 0.5;
                    double y0 = (y + 1.0) * 0.5;

                    //yield return new Point2D(x, y);

                    Point2D p = new Point2D(x, y);
                    if (p.Radius < 1.0) yield return new Point2D(x0, y0);
                }
            }
        }

    }
}
