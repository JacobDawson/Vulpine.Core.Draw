/**
 *  This file is an integral part of the Vulpine Core Library: 
 *  Copyright (c) 2016-2017 Benjamin Jacob Dawson. 
 *
 *      https://www.jacobs-den.org/projects/core-library/
 *
 *  This file is licensed under the Apache License, Version 2.0 (the "License"); 
 *  you may not use this file except in compliance with the License. You may 
 *  obtain a copy of the License at:
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.    
 */

using System;
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
        /// but the method has been known to cause ringing artifacts.
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
    public enum AntiAilis
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
    public enum Desaturate
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

    /// <summary>
    /// Textures lack a notion of absolute scale. Thus, when textures are rendered out to
    /// an image, they must be scaled to fit the target image. How this scaling (or streaching)
    /// is acomplished can be spesified by this enumeration.
    /// </summary>
    public enum Scaling
    {
        /// <summary>
        /// The texture is scaled verticaly to match the height of the image. In
        /// other words, the virtical FOV is preserved when resizing the image.
        /// </summary>
        Vertical = 1,

        /// <summary>
        /// The texture is scaled horizontaly to match the width of the image. In
        /// other words, the horizontal FOV is preserved when resizing the image.
        /// </summary>
        Horizontal = 2,

        /// <summary>
        /// The texture is steched horizontaly and verticaly inorder to fit the
        /// dimentions of the image. This option may lead to visual distortions.
        /// </summary>
        Streach = 3,
    }

    /// <summary>
    /// When rendering an image, several sample points are generated per pixel. A weighted
    /// average is then computed to get the final result. The weight of each sample, or
    /// how much that sample contributs to the final result, is determined by a weighting
    /// funciton, also known as a window function. These functions are called windows because
    /// they only consider samples up to a set distance, and zero out all others.
    /// </summary>
    public enum Window
    {
        /// <summary>
        /// Scales the sample points uniformly. That is, every sample point contributs
        /// equaly to the final pixel, iregardless of distance. It is called a box
        /// window due to the shape of its graph.
        /// </summary>
        Box = 0,

        /// <summary>
        /// Scales the sample points liniarly. It uses the function one minius the
        /// distance to determin the weight of each sample. It is called a tent
        /// window due to the shape of its graph.
        /// </summary>
        Tent = 1,

        /// <summary>
        /// Uses a normalised cosine function to determin the weight of each sample.
        /// It is scaled sutch that the cosine is zero when the distance is one.
        /// The result is simlar to the sinc window, but is faster to compute.
        /// </summary>
        Cosine = 2,

        /// <summary>
        /// Uses a gausian function, also called a bell-curve, to determin the weight
        /// of each sample. It is unique in that it dose not go to zero at a fixed
        /// point, but rather continues to get smaller with greater distance. Ultmatly,
        /// this leads to more bluring and a smoother image.
        /// </summary>
        Gausian = 3,

        /// <summary>
        /// Use a normalised sinc function to determin the weight of each sample.
        /// It is scaled sutch that the function is zero when the distance is one.
        /// It is based on the Nyquist sampling therum, and offers the most 
        /// realistic results.
        /// </summary>
        Sinc = 4,

        /// <summary>
        /// The Lanczos filter is equivlent to the sinc filter squared. It is similar
        /// to the Sinc3 interpolation funciton, which can be thought of as the reverse
        /// of image rendering. It allows for a much sharper image while still removing
        /// ailising artifacts.
        /// </summary>
        Lanczos = 5,
    }


}


