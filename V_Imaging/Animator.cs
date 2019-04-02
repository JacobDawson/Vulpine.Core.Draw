/**
 *  This file is an integral part of the Vulpine Core Library
 *  Copyright (c) 2016-2019 Benjamin Jacob Dawson
 *
 *      http://www.jakesden.com/corelibrary.html
 *
 *  The Vulpine Core Library is free software; you can redistribute it 
 *  and/or modify it under the terms of the GNU Lesser General Public
 *  License as published by the Free Software Foundation; either
 *  version 2.1 of the License, or (at your option) any later version.
 *
 *  The Vulpine Core Library is distributed in the hope that it will 
 *  be useful, but WITHOUT ANY WARRANTY; without even the implied 
 *  warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
 *  See the GNU Lesser General Public License for more details.
 *
 *      https://www.gnu.org/licenses/lgpl-2.1.html
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with this library; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Geometry;
using Vulpine.Core.Calc.Matrices;
using Vulpine.Core.Calc.RandGen;

namespace Vulpine.Core.Draw
{
    public class Animator
    {
        //determins how the texture should be scaled to fit
        private Scaling scale = Scaling.Vertical;






        private Image front_buffer;
        private Image back_buffer;

        private bool two_pass;

        //stores the delegate to handel render events
        private EventHandler<RenderEventArgs> e_render;
        private EventHandler<RenderEventArgs> e_frame;

        //stores the start and finish events
        private EventHandler e_start;
        private EventHandler e_finish;

        #region Object Events...

        /// <summary>
        /// The render event is envoked after each pixel is rendered. This
        /// can be used to show a progress bar, or display the results of
        /// the render thus far.
        /// </summary>
        public event EventHandler<RenderEventArgs> RenderEvent
        {
            add { e_render += value; }
            remove { e_render -= value; }
        }

        /// <summary>
        /// The frame update event is envoked upon completion of each frame, so
        /// that the frame date can be drawn to the screen or saved to disk, before
        /// the frame buffer is overwritten with the next frame.
        /// </summary>
        public event EventHandler<RenderEventArgs> FrameUpdate
        {
            add { e_frame += value; }
            remove { e_frame -= value; }
        }

        /// <summary>
        /// The start event is envoked as soon as the rendering starts,
        /// before anything else happens. It's mostly included for sake of
        /// completness, but may be useful in a multi-threaded enviroment.
        /// </summary>
        public event EventHandler StartEvent
        {
            add { e_start += value; }
            remove { e_start -= value; }
        }

        /// <summary>
        /// The finish event is envoked once the render is complete. This
        /// allows the end user to prefrom any post processing, like 
        /// displaying the rendered image, or saving it to disk.
        /// </summary>
        public event EventHandler FinishEvent
        {
            add { e_finish += value; }
            remove { e_finish -= value; }
        }

        #endregion ////////////////////////////////////////////////////////////////

        #region Animation...

        //private void RenderStart(Animation ani, int start, int stop)
        //{
        //    //Animation ani = GetAnimation();
        //    //SetRenderor();
        //    //SetBuffer();

        //    start = Math.Min(0, start);
        //    stop = Math.Min(stop, ani.FrameCount - 1);

        //    DrawBuffer();
        //    frame_no = start;

        //    twice = chkSuper.Checked;

        //    if (twice)
        //    {
        //        barFrame.Maximum = back_buffer.Size / STEP;
        //        barFrame.Value = 0;
        //        barFrame.Refresh();
        //    }
        //    else
        //    {
        //        barFrame.Maximum = myimage.Size / STEP;
        //        barFrame.Value = 0;
        //        barFrame.Refresh();
        //    }

        //    barTotal.Maximum = stop;
        //    barTotal.Value = start;
        //    barTotal.Refresh();

        //    halt = false;
        //    btnStartStop.Text = "HALT !";
        //    btnStartStop.Refresh();

        //    ThreadStart s = () => RenderMainLoop(ani, start, stop);
        //    Thread thread = new Thread(s);
        //    thread.Start();
        //}

        //private void RenderMainLoop(Animation ani, int start, int stop)
        //{
        //    DateTime epoc = DateTime.Now;

        //    for (int i = start; i <= stop; i++)
        //    {
        //        string file = String.Format(@"S:\Animation\frame_{0:0000}.png", i);
        //        Texture frame = ani.GetFrame(i);

        //        if (twice)
        //        {
        //            ren.Render(frame, back_buffer);
        //            Texture temp = new Interpolent(back_buffer, Intpol.BiCubic);

        //            //barFrame.Value = 0;
        //            quick.Render(temp, myimage);
        //        }
        //        else
        //        {
        //            //we render the image straight
        //            ren.Render(frame, myimage);
        //        }

        //        lock (myimage.Key)
        //        {
        //            Bitmap bmp = (Bitmap)myimage;
        //            bmp.Save(file);
        //        }

        //        TimeSpan span = DateTime.Now - epoc;
        //        double per = span.TotalSeconds / (i - start + 1);
        //        double rem = per * (stop - i);

        //        eta = TimeSpan.FromSeconds(rem);
        //        elap = span;
        //        UpdateLable();

        //        IncrementTotal();
        //        DrawBuffer();

        //        if (halt)
        //        {
        //            ButtonReset();
        //            break;
        //        }
        //    }
        //}


        public void StartAnimation(Animation ani, int start, int stop)
        {         
            Texture frame = null;
            bool halt = false;

            start = Math.Min(0, start);
            stop = Math.Min(stop, ani.FrameCount - 1);

            //invokes any starting events that are regesterd
            if (e_start != null) e_start(this, EventArgs.Empty);

            for (int i = start; i <= stop; i++)
            {
                if (two_pass)
                {
                    //first renders the frame to the back buffer
                    frame = ani.GetFrame(i);
                    RenderFrame(frame, back_buffer, i, true);

                    //downsamples the back buffer
                    Downsample(back_buffer, front_buffer);
                }
                else
                {
                    //simply renders the frame to the front buffer
                    frame = ani.GetFrame(i);
                    RenderFrame(frame, front_buffer, i, false);                  
                }

                //informes the listners of the frame update
                halt = OnFrame(i, front_buffer);
                if (halt) break;
            }

            //invokes any finishing events that are regesterd
            if (e_finish != null) e_finish(this, EventArgs.Empty);
        }


        public void ThreadLoop(Animation ani, int start, int stop, int step)
        {
            Image thread_buffer0 = null;
            Image thread_buffer1 = null;

            Texture frame = null;
            bool halt = false;

            start = Math.Min(0, start);
            stop = Math.Min(stop, ani.FrameCount - 1);

            for (int i = start; i <= stop; i += step)
            {
                //obtains the frame (need to lock this!!!)
                frame = ani.GetFrame(i);

                if (two_pass)
                {
                    RenderFrame(frame, thread_buffer1, i, true);
                    Downsample(thread_buffer1, thread_buffer0);
                }
                else
                {
                    //simply renders the frame to the front buffer
                    RenderFrame(frame, thread_buffer0, i, false);
                }

                //informes the listners of the frame update
                halt = OnFrame(i, thread_buffer0);
                if (halt) break;
            }
        }

        /// <summary>
        /// Raises the frame update event and informs all listeners. It returns true
        /// if any of the listners indicate that the animaiton should stop.
        /// </summary>
        /// <param name="frame">The frame that was last rendered</param>
        /// <param name="buffer">Link to the frame buffer</param>
        /// <returns>True if the rendering should stop</returns>
        private bool OnFrame(int frame, Image buffer)
        {
            //checks that we actualy have someone listening
            if (e_frame == null) return false;

            //creates new event args and invokes the event
            var args = new RenderEventArgs(frame, buffer.Size, buffer);
            e_frame(this, args); return args.Halt;
        }


        #endregion ////////////////////////////////////////////////////////////////

        #region Rendering...


        /// <summary>
        /// Renders the given frame to a buffer image, overwriting any data 
        /// the buffer may contain in the process.
        /// </summary>
        /// <param name="t">Texture to be rendered</param>
        /// <param name="buffer">Buffer image to hold the rendering</param>
        /// <param name="frame">Index of the frame being rendered</param>
        private void RenderFrame(Texture t, Image buffer, int frame, bool shift)
        {      
            double w = buffer.Width;
            double h = buffer.Height;
            double xp = 0.0;
            double yp = 0.0;
            bool halt = false;
            int count = 0;

            //subtracts one from the dimentions if shifting
            w = shift ? (w - 1.0) : w;
            h = shift ? (h - 1.0) : h;

            for (int y = 0; y < buffer.Height; y++)
            {
                for (int x = 0; x < buffer.Width; x++)
                {
                    //centers the point if we're not shifting
                    xp = shift ? x : (x + 0.5);
                    yp = shift ? y : (y + 0.5);

                    //computes the sample point
                    count = y * buffer.Width + x;
                    buffer[x, y] = RenderPixel(t, xp, yp, w, h);
                    halt = OnRender(frame, count, buffer);

                    if (halt) return;
                }
            }
        }

        /// <summary>
        /// Generates the color of a single pixel in the target image, given the 
        /// texture to render, the locaiton of the pixel, and the dimentions of 
        /// the target image.
        /// </summary>
        /// <param name="t">Texture to be rendered</param>
        /// <param name="x">X cordinate of the pixel to render</param>
        /// <param name="y">Y cordinate of the pixel to render</param>
        /// <param name="w">Width of the target image</param>
        /// <param name="h">Height of the target image</param>
        /// <returns>The final color of the given pixel in the rendered image</returns>
        private Color RenderPixel(Texture t, double x, double y, double w, double h)
        {
            //determins the scaling factor in the X and Y direction
            double sx = (scale == Scaling.Vertical) ? h : w;
            double sy = (scale == Scaling.Horizontal) ? w : h;

            //scales the texture to fit the target image
            double u = ((2.0 * x) - w) / sx;
            double v = ((2.0 * y) - h) / sy;

            //samples the center of the pixel
            return t.Sample(u, -v);

        }

        /// <summary>
        /// Downsamples a larger image into a smaller one, preforming a sort of
        /// poor-man's anti-ailising. It uses a square latius filter, where every
        /// sample of the source image contributes equaly to the target, aproximating
        /// a gausian. Here the dimentions of the source image are asumed to be 
        /// twice the target image plus two. 
        /// </summary>
        /// <param name="source">The larger source image</param>
        /// <param name="target">The smaler target image</param>
        private void Downsample(Image source, Image target)
        {
            int width = target.Width;
            int height = target.Height;

            Vector avg;
            int x2, y2;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    x2 = x * 2;
                    y2 = y * 2;
                    avg = new Vector(4);

                    avg += 0.0625 * (Vector)source[x2, y2];
                    avg += 0.1250 * (Vector)source[x2 + 1, y2];
                    avg += 0.0625 * (Vector)source[x2 + 2, y2];

                    avg += 0.1250 * (Vector)source[x2, y2 + 1];
                    avg += 0.2500 * (Vector)source[x2 + 1, y2 + 1];
                    avg += 0.1250 * (Vector)source[x2 + 2, y2 + 1];

                    avg += 0.0625 * (Vector)source[x2, y2 + 2];
                    avg += 0.1250 * (Vector)source[x2 + 1, y2 + 2];
                    avg += 0.0625 * (Vector)source[x2 + 2, y2 + 2];

                    target[x, y] = Color.FromRGB(avg);
                }
            }
        }

        /// <summary>
        /// Raises the render event and informs all listners. It returns true if
        /// any of the listners indicate that the rendering should stop.
        /// </summary>
        /// <param name="frame">Curent frame being rendered</param>
        /// <param name="count">Pixels rendered so far</param>
        /// <param name="buffer">Link to the frame buffer</param>
        /// <returns>True if the rendering should stop</returns>
        private bool OnRender(int frame, int count, Image buffer)
        {
            //checks that we actualy have someone listening
            if (e_render == null) return false;

            //creates new event args and invokes the event
            var args = new RenderEventArgs(frame, count, buffer);
            e_render(this, args); return args.Halt;
        }

        #endregion ////////////////////////////////////////////////////////////////
    }
}
