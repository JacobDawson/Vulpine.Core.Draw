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
        /// This is the most basic form of cubic interpolation. It utilises
        /// B-Splines in order to interpolate the data. This leads to very
        /// smooth images which are also very blury. 
        /// </summary>
        BiCubic = 3,

        /// <summary>
        /// This form of cubic interpolation uses Catmull–Rom Splines to
        /// interpolate the data. This leads to very sharp images, but can
        /// cause unwanted ringing or halo artifacts.
        /// </summary>
        Catrom = 4,

        /// <summary>
        /// Mitchell-Netravali interpolation atempts to combine the weights
        /// of the generic and Catmull-Rom interpolation, inorder to reduce
        /// the artifacts of both, for an ideal result.
        /// </summary>
        Mitchel = 5,

        /// <summary>
        /// The Sinc3 or Lancoze resampling funciton is based on ideas from signal
        /// analisis, allowing it to reconstruct the original image given a high
        /// enough sample rate. This can lead to very sharp images, but unforntnatly
        /// it suffers from the same problem as Catrom Interpolation. It samples
        /// from a wider area than the other funcitons, making it the most precice
        /// resampling funciton avaliable.
        /// </summary>
        Sinc3 = 6,

        /// <summary>
        /// The default method to be used when the user dosen't care how an
        /// image is interpolated. The actual method used is not garenteed to
        /// be consistent between builds.
        /// </summary>
        Default = BiLiniar,
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
        /// Takes the Luminance value of the HSL color space to be the grey
        /// value. Usefull for image processing that relies on HSL space.
        /// </summary>
        Lumanince = 3,

        /// <summary>
        /// Takes a weighted average of the red, green, and blue channels in
        /// an atempt to produce the most 'visualy similar' grey value possable.
        /// </summary>
        Natural = 4,

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

    /// <summary>
    /// Enumerates the vairieus diffrent color spaces that colors can be organised into.
    /// This can be thought of as a sort of color format, though it dose not include things
    /// like how many bits per channel should be stored. Diffrent collor spaces are useful
    /// in diffrent applications, as they allow color to be interpreted in diffrent ways.
    /// In particular, distances between colors are not the same in all spaces.
    /// </summary>
    public enum ColorSpace
    {
        /// <summary>
        /// This is the standard, default color space used by most digital systems. It
        /// divides colors into red, green, and blue components. This is based on the fact 
        /// that human eyes have three cones, sensitive to red, green, and blue light. 
        /// All other color spaces are defined in terms of this one.
        /// </summary>
        RGB = 0,

        /// <summary>
        /// This color space tries to match how humans perceve color, phychologicaly. It
        /// measures color based on saturation (how washed out or vibrant it is) and 
        /// value (how bright or dark it is) It also measures a cylical hue component
        /// which stores the actual cormatic data.
        /// </summary>
        HSV = 1,

        /// <summary>
        /// This is a similar color space to HSV, though it differs in how the saturation
        /// is computed. Luminance, like value, ranges from dark to light, but a fully 
        /// luminant color is completly white. This works out better for some computer 
        /// imaging applications, otherwise it is a personal preference.
        /// </summary>
        HSL = 2,

        /// <summary>
        /// This color space seperates the overall luminance (or brightness) of the color
        /// from the cromatic informaiton. This is mostly useful for image compression,
        /// as the human eye is less sensitive to changes in color than it is to changes
        /// in brightness. 
        /// </summary>
        YUV = 3,

        /// <summary>
        /// A scientificly determined color space, based on how humans pereceve color. 
        /// Real XYZ space takes into account things like illumination and viewing
        /// conditions, which are only aproximated here. Tipicaly, this color space is
        /// only used as an intermediat step in converstion to other color spaces.
        /// </summary>
        XYZ = 4,

        /// <summary>
        /// A scientificly determined color space, ment to be perceptualy uniform. That is,
        /// pairs of colors which apear the same, should be more or less the same distance
        /// apart. For that reason, this space gives the best results when interpolating
        /// or mixing colors. Unfortunatly it is also the most dificult space to compute.
        /// </summary>
        LAB = 5,       
    }

    /// <summary>
    /// Many opperations, like convolution or interpolation require accessing data that
    /// lies outside the bounds of an image. The best way to handel these litteral edge
    /// cases, is to artificialy extend the image beyond it's bounds, either by tiling
    /// or reflecting the image in both directions.
    /// </summary>
    public enum ImageExt
    {
        /// <summary>
        /// This setting tiles the image indefinatly along both direcitons. It should
        /// only be used if the image can be tiled witout causing unwanted artifacts.
        /// </summary>
        TileXY = 0,

        /// <summary>
        /// This setting tiles the image indefinatly in the horizontal direction, and
        /// mirrors the image in the virtical direction. This is usefull for images
        /// that are tilable, but only in the horizontal direction.
        /// </summary>
        TileX_MirrorY = 1,

        /// <summary>
        /// This setting tiles the image indefinatly in the vertical direction, and
        /// mirrors the image in the horizontal direction. This is usefull for images
        /// that are tilable, but only in the vertical direction.
        /// </summary>
        MirrorX_TileY = 2,

        /// <summary>
        /// This setting makes mirroed copies of the image in both horizontal and
        /// vertical directions. This is the prefered setting for all images which
        /// are not tileable.
        /// </summary>
        MirrorXY = 3,

        /// <summary>
        /// This is the default setting which should work for all images, regardless
        /// of tileability. Of course tileable images will preform better if they
        /// are explicity makred as tileable.
        /// </summary>
        Default = MirrorXY,
    }

    /// <summary>
    /// Textures are infinate in scope, but raster images are not. Therefore when converting
    /// images to textures, it's nessary to define a texture border to determin what happens
    /// when we try to sample points that lie outside the source image. 
    /// </summary>
    public enum TexBorder
    {
        /// <summary>
        /// There is no border, and we allow sampeling outside the bounds of the image.
        /// This geneeraly results in the image being repeated indefinitly across the entire
        /// texture. Though the exact result depends on the source image.
        /// </summary>
        None = 0,

        /// <summary>
        /// The borders of the texture are treated as transparent, allowing whatever is
        /// underneath the texture to show through. This is the ideal setting to use if
        /// you intend to stack multiple textures on top of each other.
        /// </summary>
        Transparent = 1,

        /// <summary>
        /// The borders of the texture are treated as a single matte color. This is useful
        /// if you want to display the texture on screen, or use it in some envoroment that
        /// only accepts RGB colors without transparancy.
        /// </summary>
        Matte = 2,
    }


}


