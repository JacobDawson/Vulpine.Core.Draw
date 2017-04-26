﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Draw
{
    /// <summary>
    /// Represents the various methods of image interpolation suported
    /// by the engine. Most methods offer a tradeoff between speed and
    /// image quality.
    /// </summary>
    public enum Intpol
    {
        /// <summary>
        /// Nearest Neigbor interpolaiton always takes the closest matching
        /// point. It's the same as no interpolaiton at all, which is why
        /// it produces uniquly pixilated results.
        /// </summary>
        Nearest = 1,

        /// <summary>
        /// Bilinar is the fastest interpolation method. Unfortunatly it only
        /// considers a small area, which can lead to sharp boundrais where
        /// smooth gradients would be prefered.
        /// </summary>
        BiLiniar = 2,

        /// <summary>
        /// Bicubic interpolation considers a larger area than Biliniar 
        /// interpolation, leading to mutch smoother results. Unfortunatly,
        /// it is considerably slower.
        /// </summary>
        BiCubic = 3,

        /// <summary>
        /// Interpolates color using the Sinc3 (Lanczos) resampling function.
        /// It works by treating the image as a 2D signal, and passing a windoing
        /// funciton over that signal. Usualy this produces very good results,
        /// but the method has been known to produce ringing artifacts.
        /// </summary>
        Sinc3 = 4,

        /// <summary>
        /// The default method to be used when the user dosen't care how an
        /// image is interpolated. The actual method used is not garenteed to
        /// be consistent between builds.
        /// </summary>
        Default = BiLiniar,
    }

    /// <summary>
    /// Represnets the various methods of anti-ailising that can be utilised
    /// in pixel-based rendering.
    /// </summary>
    public enum AAMethod
    {
        /// <summary>
        /// No Anti-Aliasing is used. Instead a single sample is calculated per pixel
        /// and used to color the entire pixel. This is the fastest rendering method
        /// but alising artifacts are clearly visible.
        /// </summary>
        None = 0,

        /// <summary>
        /// Takes a random set of samples and computes there average to color each
        /// pixel. While better than using no Anti-Alising at all, the non-uniformity
        /// of the samples can lead to undesirabale results.
        /// </summary>
        Random = 1,

        /// <summary>
        /// Divides each pixel into sub-pixels and selects a single random sample
        /// per sub-pixel. The samples are then averaged to get the color of the
        /// whole pixel. This provides a somewhat uniform distribution of samples
        /// while still runing fairly quickly.
        /// </summary>
        Jittred = 2,

        /// <summary>
        /// Generates a uniform distribution of random samples to calculate the
        /// color of each pixel. This method produces the most acurate results,
        /// but at the cost of running slower than the other methods.
        /// </summary>
        Poisson = 3,
    }

    /// <summary>
    /// There are multiple ways to convert a color image to a grayscale image,
    /// all depending on what effect the end user wishes to produce. This enum
    /// lists all sutch methods suported by the engine.
    /// </summary>
    public enum GrayMehtod
    {
        /// <summary>
        /// Takes the average of the red, green, and blue channels. This 
        /// is the most basic and intuitive method, but dose not always 
        /// produce pleasing results.
        /// </summary>
        Average = 1,

        /// <summary>
        /// Takes the maximum of the red, green, and bule channels. This
        /// is equevlent to setting the saturation to zero, while leaving
        /// value allown in the HSV color space.
        /// </summary>
        Maximum = 2,

        /// <summary>
        /// Takes the minimum of the red, green and blue channels. This dose
        /// not corispond to any photometric property, but is included for
        /// the sake of completness.
        /// </summary>
        Minimum = 3,

        /// <summary>
        /// Takes the Luminance value of the HSL color space to be the grey
        /// value. Usefull for image processing that relies on HSL space.
        /// </summary>
        Lumanince = 4,

        /// <summary>
        /// Takes a weighted average of the red, green, and blue channels in
        /// an atempt to produce the most 'visualy similar' grey value possable.
        /// </summary>
        Natural = 5,

        /// <summary>
        /// The default method to be used when the user dosen't care what
        /// grey value is used. The actual method used is not garenteed to
        /// be consistent between builds.
        /// </summary>
        Default = Natural,
    }


}


