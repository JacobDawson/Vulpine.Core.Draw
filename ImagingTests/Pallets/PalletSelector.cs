using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Vulpine.Core.Draw;
using Vulpine.Core.Draw.Textures;
using Vulpine.Core.Draw.Images;

using VImage = Vulpine.Core.Draw.Image;
using VColor = Vulpine.Core.Draw.Color;

using ImagingTests.Properties;

namespace ImagingTests.Pallets
{
    public partial class PalletSelector : UserControl
    {
        private ImageSystem myimage;
        private ImageSystem source;
        private ImageSystem preview;

        private ImagePallet pimage;

        private Renderor ren;

        ////stores the avilable pallets
        //private List<Pallet> pallets;

        public PalletSelector()
        {
            InitializeComponent();
            myimage = new Bitmap(480, 480);
            source = new Bitmap(480, 480);
            preview = new Bitmap(1080, 64);


            ren = new Renderor();
            ren.Scaling = Scaling.Streach;      

            IncrementBarDelegate = (this.IncrementBar);
            DrawMyImageDelegate = (this.DrawMyImage);
            AppendTextDelegate = (this.AppendText);
            DrawMyPalletDelegate = (this.DrawMyPallet);
        }

        private void LoadFile(string file)
        {
            if (source != null) source.Dispose();
            if (myimage != null) myimage.Dispose();

            source = new ImageSystem(file);
            myimage = new ImageSystem(source.Width, source.Height);

            RenderSource();
        }


        private void RenderSource()
        {
            Interpolent intpol = new Interpolent(source, Intpol.Nearest);
            ren.Render(intpol, myimage);
            DrawMyImage();
        }

        private void RenderPallet()
        {
            Pallet pallet = GetPallet();
            pimage = new ImagePallet(source.Width, source.Height, pallet);
            pimage.FillData(source);

            //Interpolent intpol = new Interpolent(pimage, Intpol.Nearest);
            //ren.Render(intpol, myimage);
            myimage.FillData(pimage);
            DrawMyImage();

            DisplayPallet(pallet);
        }

        private void RenderDither()
        {
            Pallet pallet = GetPallet();
            double amount = GetAmount();

            pimage = new ImagePallet(source.Width, source.Height, pallet);
            pimage.FillDither(source, amount);

            //Interpolent intpol = new Interpolent(pimage, Intpol.Nearest);
            //ren.Render(intpol, myimage);
            myimage.FillData(pimage);
            DrawMyImage();

            DisplayPallet(pallet);
        }

        private void RenderFS()
        {
            Pallet pallet = GetPallet();
            pimage = new ImagePallet(source.Width, source.Height, pallet);
            pimage.FillDitherFS(source);

            //Interpolent intpol = new Interpolent(pimage, Intpol.Nearest);
            //ren.Render(intpol, myimage);
            myimage.FillData(pimage);
            DrawMyImage();

            DisplayPallet(pallet);
        }

        private void DisplayPallet(Texture pallet)
        {
            ren.Render(pallet, preview);
            DrawMyPallet();            
        }

        private double GetAmount()
        {
            switch (cmbAmount.SelectedIndex)
            {
                case 0: return 1.0;
                case 1: return 1.0 / 2.0;
                case 2: return 1.0 / 4.0;
                case 3: return 1.0 / 8.0;
                case 4: return 1.0 / 16.0;
                case 5: return 1.0 / 32.0;
                case 6: return 1.0 / 64.0;
                case 7: return 1.0 / 128.0;
                case 8: return 1.0 / 256.0;

                default: return 1.0 / 4.0;
            }
        }

        #region Pallets...

        private Pallet GetPallet()
        {
            switch (cmbPallet.SelectedIndex)
            {
                case 0: return new Pallet( //Black and White
                    VColor.FromRGB24(0, 0, 0),
                    VColor.FromRGB24(255, 255, 255));
                case 1: return new Pallet( //Normal
                    VColor.FromRGB24(230, 38, 38),
                    VColor.FromRGB24(230, 120, 38),
                    VColor.FromRGB24(230, 230, 38),
                    VColor.FromRGB24(120, 230, 38),
                    VColor.FromRGB24(12, 140, 12),
                    VColor.FromRGB24(52, 184, 215),
                    VColor.FromRGB24(38, 148, 216),
                    VColor.FromRGB24(58, 58, 230),
                    VColor.FromRGB24(138, 74, 230),
                    VColor.FromRGB24(230, 90, 202),
                    VColor.FromRGB24(230, 170, 154),
                    VColor.FromRGB24(140, 78, 60),
                    VColor.FromRGB24(230, 230, 230),
                    VColor.FromRGB24(110, 110, 110),
                    VColor.FromRGB24(0, 0, 0));
                case 2: return new Pallet( //Pastel
                    VColor.FromRGB24(230, 134, 106),
                    VColor.FromRGB24(230, 168, 122),
                    VColor.FromRGB24(230, 194, 170),
                    VColor.FromRGB24(230, 140, 138),
                    VColor.FromRGB24(230, 106, 106),
                    VColor.FromRGB24(174, 216, 190),
                    VColor.FromRGB24(150, 200, 148),
                    VColor.FromRGB24(70, 156, 122),
                    VColor.FromRGB24(158, 200, 90),
                    VColor.FromRGB24(230, 210, 122),
                    VColor.FromRGB24(230, 196, 58),
                    VColor.FromRGB24(200, 154, 78),
                    VColor.FromRGB24(230, 216, 216),
                    VColor.FromRGB24(170, 125, 135),
                    VColor.FromRGB24(112, 64, 82));
                case 3: return new Pallet( //Vibrant
                    VColor.FromRGB24(70, 70, 216),
                    VColor.FromRGB24(106, 132, 230),
                    VColor.FromRGB24(112, 216, 175),
                    VColor.FromRGB24(58, 186, 135),
                    VColor.FromRGB24(32, 126, 108),
                    VColor.FromRGB24(230, 184, 220),
                    VColor.FromRGB24(230, 140, 192),
                    VColor.FromRGB24(170, 52, 150),           
                    VColor.FromRGB24(216, 70, 98),
                    VColor.FromRGB24(230, 162, 58),
                    VColor.FromRGB24(230, 230, 75),
                    VColor.FromRGB24(172, 172, 36),
                    VColor.FromRGB24(216, 224, 228),
                    VColor.FromRGB24(114, 148, 156),
                    VColor.FromRGB24(0, 60, 96));
                case 4: return new Pallet( //Earth Tones
                    VColor.FromRGB24(200, 90, 64),
                    VColor.FromRGB24(216, 148, 38),
                    VColor.FromRGB24(230, 182, 108),
                    VColor.FromRGB24(170, 116, 76),
                    VColor.FromRGB24(110, 60, 50),
                    VColor.FromRGB24(164, 200, 108),
                    VColor.FromRGB24(106, 156, 90),
                    VColor.FromRGB24(65, 110, 55),
                    VColor.FromRGB24(90, 126, 110),
                    VColor.FromRGB24(150, 186, 178),
                    VColor.FromRGB24(136, 136, 186),
                    VColor.FromRGB24(90, 90, 172),
                    VColor.FromRGB24(230, 222, 184),
                    VColor.FromRGB24(140, 110, 102),
                    VColor.FromRGB24(76, 52, 38));
                case 5: return new Pallet( //Orange-Blue-Pink
                    VColor.FromRGB24(230, 230, 58),
                    VColor.FromRGB24(232, 218, 172),
                    VColor.FromRGB24(230, 188, 0),
                    VColor.FromRGB24(230, 170, 74),
                    VColor.FromRGB24(230, 145, 0),
                    VColor.FromRGB24(90, 120, 234),
                    VColor.FromRGB24(58, 58, 230),
                    VColor.FromRGB24(12, 12, 140),
                    VColor.FromRGB24(12, 84, 124),
                    VColor.FromRGB24(230, 154, 216),
                    VColor.FromRGB24(230, 106, 156),
                    VColor.FromRGB24(230, 74, 76),
                    VColor.FromRGB24(230, 230, 230),
                    VColor.FromRGB24(124, 124, 170),
                    VColor.FromRGB24(12, 12, 78));
                case 6: return new Pallet( //Purple-Orange-Green
                    VColor.FromRGB24(140, 0, 90),
                    VColor.FromRGB24(214, 70, 214),
                    VColor.FromRGB24(175, 115, 214),
                    VColor.FromRGB24(130, 70, 216),
                    VColor.FromRGB24(70, 34, 198),
                    VColor.FromRGB24(230, 230, 0),
                    VColor.FromRGB24(230, 188, 0),
                    VColor.FromRGB24(230, 150, 22),
                    VColor.FromRGB24(230, 120, 36),
                    VColor.FromRGB24(196, 230, 36),
                    VColor.FromRGB24(120, 200, 64),
                    VColor.FromRGB24(30, 158, 84),
                    VColor.FromRGB24(230, 230, 216),
                    VColor.FromRGB24(75, 128, 64),
                    VColor.FromRGB24(0, 60, 0));
                case 7: return new Pallet( //Warm Colors
                    VColor.FromRGB24(230, 230, 116),
                    VColor.FromRGB24(230, 230, 0),
                    VColor.FromRGB24(230, 188, 0),
                    VColor.FromRGB24(230, 145, 0),
                    VColor.FromRGB24(230, 100, 0),
                    VColor.FromRGB24(216, 86, 86),
                    VColor.FromRGB24(186, 28, 28),
                    VColor.FromRGB24(126, 0, 0),
                    VColor.FromRGB24(155, 0, 96),
                    VColor.FromRGB24(216, 38, 112),
                    VColor.FromRGB24(214, 124, 146),
                    VColor.FromRGB24(230, 170, 170),
                    VColor.FromRGB24(230, 216, 216),
                    VColor.FromRGB24(156, 90, 90),
                    VColor.FromRGB24(60, 12, 12));
                case 8: return new Pallet( //Cool Colors
                    VColor.FromRGB24(152, 202, 186),
                    VColor.FromRGB24(52, 216, 152),
                    VColor.FromRGB24(26, 172, 144),
                    VColor.FromRGB24(60, 198, 230),
                    VColor.FromRGB24(20, 150, 230),
                    VColor.FromRGB24(20, 102, 216),
                    VColor.FromRGB24(12, 60, 202),
                    VColor.FromRGB24(12, 12, 158),
                    VColor.FromRGB24(72, 40, 186),
                    VColor.FromRGB24(120, 52, 216),
                    VColor.FromRGB24(78, 0, 126),
                    VColor.FromRGB24(46, 0, 78),
                    VColor.FromRGB24(216, 228, 228),
                    VColor.FromRGB24(80, 96, 156),
                    VColor.FromRGB24(14, 14, 78));
                case 9: return new Pallet( //Skintones
                    VColor.FromRGB24(230, 220, 214),
                    VColor.FromRGB24(230, 194, 170),
                    VColor.FromRGB24(230, 174, 138),
                    VColor.FromRGB24(230, 156, 106),
                    VColor.FromRGB24(200, 130, 76),
                    VColor.FromRGB24(230, 140, 138),
                    VColor.FromRGB24(230, 106, 106),
                    VColor.FromRGB24(200, 90, 90),
                    VColor.FromRGB24(186, 94, 70),
                    VColor.FromRGB24(140, 78, 60),
                    VColor.FromRGB24(140, 94, 60),
                    VColor.FromRGB24(110, 74, 46),
                    VColor.FromRGB24(76, 50, 28),
                    VColor.FromRGB24(46, 20, 0),
                    VColor.FromRGB24(0, 0, 0));
                case 10: return new Pallet( //Hair Color
                    VColor.FromRGB24(230, 220, 214),
                    VColor.FromRGB24(230, 206, 108),
                    VColor.FromRGB24(230, 188, 0),
                    VColor.FromRGB24(230, 170, 72),
                    VColor.FromRGB24(230, 146, 0),
                    VColor.FromRGB24(230, 156, 138),
                    VColor.FromRGB24(230, 120, 90),
                    VColor.FromRGB24(214, 86, 52),
                    VColor.FromRGB24(186, 52, 12),
                    VColor.FromRGB24(140, 38, 0),
                    VColor.FromRGB24(170, 95, 38),
                    VColor.FromRGB24(156, 70, 12),
                    VColor.FromRGB24(110, 46, 0),
                    VColor.FromRGB24(78, 34, 0),
                    VColor.FromRGB24(45, 12, 0));
                case 11: return new Pallet( //CGA-1
                    VColor.FromRGB24(0, 0, 0),
                    VColor.FromRGB24(85, 255, 255),
                    VColor.FromRGB24(255, 85, 255),                    
                    VColor.FromRGB24(255, 255, 255));
                case 12: return new Pallet( //CGA-2
                    VColor.FromRGB24(0, 0, 0),
                    VColor.FromRGB24(85, 255, 85),
                    VColor.FromRGB24(255, 85, 85),
                    VColor.FromRGB24(255, 255, 85));
                case 13: return new Pallet( //EGA
                    VColor.FromRGB24(0, 0, 0),
                    VColor.FromRGB24(0, 0, 170),
                    VColor.FromRGB24(0, 170, 0),
                    VColor.FromRGB24(0, 170, 170),
                    VColor.FromRGB24(170, 0, 0),
                    VColor.FromRGB24(170, 0, 170),
                    VColor.FromRGB24(170, 85, 0),
                    VColor.FromRGB24(170, 170, 170),
                    VColor.FromRGB24(85, 85, 85),
                    VColor.FromRGB24(85, 85, 255),
                    VColor.FromRGB24(85, 255, 85),
                    VColor.FromRGB24(85, 255, 255),
                    VColor.FromRGB24(255, 85, 85),
                    VColor.FromRGB24(255, 85, 255),
                    VColor.FromRGB24(255, 255, 85),
                    VColor.FromRGB24(255, 255, 255));
                case 14: return new Pallet( //NTSC Artifact
                    VColor.FromRGB24(0, 0, 0),
                    VColor.FromRGB24(0, 110, 50),
                    VColor.FromRGB24(50, 10, 255),
                    VColor.FromRGB24(0, 138, 255),
                    VColor.FromRGB24(168, 0, 50),
                    VColor.FromRGB24(118, 118, 118),
                    VColor.FromRGB24(236, 18, 255),
                    VColor.FromRGB24(186, 146, 255),
                    VColor.FromRGB24(50, 90, 0),
                    VColor.FromRGB24(0, 220, 0),
                    VColor.FromRGB24(70, 248, 186),
                    VColor.FromRGB24(236, 100, 0),
                    VColor.FromRGB24(186, 228, 0),
                    VColor.FromRGB24(255, 128, 186),
                    VColor.FromRGB24(255, 255, 255));
                case 15: return new Pallet( //NTSC Artifact G-R-Y
                    VColor.FromRGB24(0, 0, 0),
                    VColor.FromRGB24(6, 122, 255),
                    VColor.FromRGB24(0, 54, 184),
                    VColor.FromRGB24(126, 32, 0),
                    VColor.FromRGB24(104, 166, 60),
                    VColor.FromRGB24(210, 94, 0),
                    VColor.FromRGB24(130, 124, 162),
                    VColor.FromRGB24(156, 116, 130),
                    VColor.FromRGB24(238, 100, 0),
                    VColor.FromRGB24(202, 255, 54),
                    VColor.FromRGB24(240, 140, 0),
                    VColor.FromRGB24(196, 250, 80));
                case 16: return new Pallet( //NTSC Artifact C-M-W
                    VColor.FromRGB24(0, 0, 0),
                    VColor.FromRGB24(0, 140, 255),
                    VColor.FromRGB24(6, 122, 255),
                    VColor.FromRGB24(210, 94, 0),
                    VColor.FromRGB24(124, 255, 192),
                    VColor.FromRGB24(130, 124, 162),
                    VColor.FromRGB24(188, 224, 255),
                    VColor.FromRGB24(1920, 134, 255),
                    VColor.FromRGB24(236, 100, 0),
                    VColor.FromRGB24(255, 244, 236),
                    VColor.FromRGB24(255, 148, 160),
                    VColor.FromRGB24(255, 255, 255));
                case 17: return new Pallet( //CGA-3
                    VColor.FromRGB24(0, 0, 0),
                    VColor.FromRGB24(85, 255, 255),
                    VColor.FromRGB24(255, 85, 85),
                    VColor.FromRGB24(255, 255, 255));
                case 18: return new Pallet( //CGA-4
                    VColor.FromRGB24(0, 0, 0),
                    VColor.FromRGB24(0, 170, 0),
                    VColor.FromRGB24(170, 0, 0),
                    VColor.FromRGB24(170, 85, 0));
                case 19: return new Pallet( //Apple II
                    VColor.FromRGB24(0, 0, 0),
                    VColor.FromRGB24(108, 40, 64),
                    VColor.FromRGB24(64, 54, 120),
                    VColor.FromRGB24(218, 60, 240),
                    VColor.FromRGB24(20, 86, 64),
                    VColor.FromRGB24(128, 128, 128),
                    VColor.FromRGB24(38, 150, 240),
                    VColor.FromRGB24(192, 180, 248),
                    VColor.FromRGB24(64, 76, 8),
                    VColor.FromRGB24(218, 104, 16),
                    VColor.FromRGB24(236, 168, 192),
                    VColor.FromRGB24(38, 196, 16),
                    VColor.FromRGB24(192, 202, 136),
                    VColor.FromRGB24(148, 214, 192),
                    VColor.FromRGB24(255, 255, 255));
                case 20: return new Pallet( //Macintosh
                    VColor.FromRGB24(255, 255, 255),
                    VColor.FromRGB24(255, 255, 0),
                    VColor.FromRGB24(255, 102, 0),
                    VColor.FromRGB24(220, 0, 0),
                    VColor.FromRGB24(255, 0, 152),
                    VColor.FromRGB24(50, 0, 152),
                    VColor.FromRGB24(0, 0, 204),
                    VColor.FromRGB24(0, 152, 255),
                    VColor.FromRGB24(0, 170, 0),
                    VColor.FromRGB24(0, 102, 0),
                    VColor.FromRGB24(102, 50, 0),
                    VColor.FromRGB24(152, 102, 50),
                    VColor.FromRGB24(188, 188, 188),
                    VColor.FromRGB24(136, 136, 136),
                    VColor.FromRGB24(68, 68, 68),
                    VColor.FromRGB24(0, 0, 0));
                case 21: return new Pallet( //Gameboy
                    VColor.FromRGB24(15, 56, 15),
                    VColor.FromRGB24(48, 98, 48),
                    VColor.FromRGB24(138, 172, 15),
                    VColor.FromRGB24(156, 188, 15));
                case 22: return new Pallet( //NES
                    VColor.FromRGB24(72, 72, 72),
                    VColor.FromRGB24(0, 8, 88),
                    VColor.FromRGB24(0, 8, 120),
                    VColor.FromRGB24(0, 8, 112),
                    VColor.FromRGB24(56, 0, 80),
                    VColor.FromRGB24(88, 0, 16),
                    VColor.FromRGB24(88, 0, 0),
                    VColor.FromRGB24(64, 0, 0),
                    VColor.FromRGB24(16, 0, 0),
                    VColor.FromRGB24(0, 24, 0),
                    VColor.FromRGB24(0, 30, 0),
                    VColor.FromRGB24(0, 24, 32),
                    VColor.FromRGB24(160, 160, 160),
                    VColor.FromRGB24(0, 72, 184),
                    VColor.FromRGB24(8, 48, 224),
                    VColor.FromRGB24(88, 24, 216),
                    VColor.FromRGB24(160, 8, 168),
                    VColor.FromRGB24(208, 0, 88),
                    VColor.FromRGB24(208, 16, 0),
                    VColor.FromRGB24(160, 32, 0),
                    VColor.FromRGB24(96, 64, 0),
                    VColor.FromRGB24(8, 88, 0),
                    VColor.FromRGB24(0, 104, 0),
                    VColor.FromRGB24(0, 104, 16),
                    VColor.FromRGB24(0, 96, 112),
                    VColor.FromRGB24(0, 0, 0),
                    VColor.FromRGB24(248, 248, 248),
                    VColor.FromRGB24(32, 160, 248),
                    VColor.FromRGB24(80, 120, 248),
                    VColor.FromRGB24(152, 104, 248),
                    VColor.FromRGB24(248, 104, 248),
                    VColor.FromRGB24(248, 112, 176),
                    VColor.FromRGB24(248, 112, 104),
                    VColor.FromRGB24(248, 128, 24),
                    VColor.FromRGB24(192, 152, 0),
                    VColor.FromRGB24(112, 176, 0),
                    VColor.FromRGB24(40, 192, 32),
                    VColor.FromRGB24(0, 200, 112),
                    VColor.FromRGB24(0, 192, 208),
                    VColor.FromRGB24(40, 40, 40),
                    VColor.FromRGB24(160, 216, 248),
                    VColor.FromRGB24(176, 192, 248),
                    VColor.FromRGB24(208, 184, 248),
                    VColor.FromRGB24(248, 192, 248),
                    VColor.FromRGB24(248, 192, 224),
                    VColor.FromRGB24(248, 192, 192),
                    VColor.FromRGB24(248, 200, 160),
                    VColor.FromRGB24(232, 216, 136),
                    VColor.FromRGB24(200, 224, 144),
                    VColor.FromRGB24(168, 232, 160),
                    VColor.FromRGB24(144, 232, 200),
                    VColor.FromRGB24(144, 224, 232),
                    VColor.FromRGB24(168, 168, 168));
                case 23: return new Pallet( //Mario Paint
                    VColor.FromRGB24(248, 0, 0),
                    VColor.FromRGB24(248, 128, 0),
                    VColor.FromRGB24(248, 248, 0),
                    VColor.FromRGB24(0, 248, 0),
                    VColor.FromRGB24(0, 128, 64),
                    VColor.FromRGB24(0, 248, 248),
                    VColor.FromRGB24(0, 0, 248),
                    VColor.FromRGB24(192, 64, 32),
                    VColor.FromRGB24(128, 96, 0),
                    VColor.FromRGB24(248, 192, 128),
                    VColor.FromRGB24(192, 0, 192),
                    VColor.FromRGB24(0, 0, 0),
                    VColor.FromRGB24(128, 128, 128),
                    VColor.FromRGB24(192, 192, 192),
                    VColor.FromRGB24(248, 248, 248));
                case 24: return new Pallet( //MC Wool
                    VColor.FromRGB24(218, 218, 218),
                    VColor.FromRGB24(216, 124, 60),
                    VColor.FromRGB24(178, 78, 186),
                    VColor.FromRGB24(102, 134, 198),
                    VColor.FromRGB24(174, 162, 38),
                    VColor.FromRGB24(66, 172, 56),
                    VColor.FromRGB24(206, 128, 150),
                    VColor.FromRGB24(64, 64, 64),
                    VColor.FromRGB24(152, 158, 158),
                    VColor.FromRGB24(46, 110, 134),
                    VColor.FromRGB24(124, 60, 178),
                    VColor.FromRGB24(46, 56, 138),
                    VColor.FromRGB24(78, 50, 32),
                    VColor.FromRGB24(52, 70, 28),
                    VColor.FromRGB24(148, 52, 48),
                    VColor.FromRGB24(26, 22, 22));
                case 25: return new Pallet( //MC Clay
                    VColor.FromRGB24(208, 176, 160),
                    VColor.FromRGB24(160, 84, 38),
                    VColor.FromRGB24(148, 88, 108),
                    VColor.FromRGB24(114, 108, 136),
                    VColor.FromRGB24(184, 132, 36),
                    VColor.FromRGB24(102, 118, 52),
                    VColor.FromRGB24(162, 78, 78),
                    VColor.FromRGB24(58, 42, 36),
                    VColor.FromRGB24(134, 106, 96),
                    VColor.FromRGB24(86, 90, 90),
                    VColor.FromRGB24(118, 70, 86),
                    VColor.FromRGB24(74, 58, 90),
                    VColor.FromRGB24(76, 50, 36),
                    VColor.FromRGB24(76, 82, 42),
                    VColor.FromRGB24(142, 60, 46),
                    VColor.FromRGB24(36, 24, 16));
                case 26: return new Pallet( //MC Stone
                    VColor.FromRGB24(200, 160, 132),
                    VColor.FromRGB24(54, 54, 46),
                    VColor.FromRGB24(144, 148, 132),
                    VColor.FromRGB24(196, 196, 196),
                    VColor.FromRGB24(124, 116, 116),
                    VColor.FromRGB24(104, 110, 110),
                    VColor.FromRGB24(190, 158, 108),
                    VColor.FromRGB24(142, 142, 142),
                    VColor.FromRGB24(132, 132, 132),
                    VColor.FromRGB24(142, 130, 122),
                    VColor.FromRGB24(242, 242, 242),
                    VColor.FromRGB24(176, 148, 128),
                    VColor.FromRGB24(132, 160, 180),
                    VColor.FromRGB24(132, 142, 124),
                    VColor.FromRGB24(182, 182, 182),
                    VColor.FromRGB24(122, 122, 122));
                case 27: return new Pallet( //MC Wood
                    VColor.FromRGB24(152, 124, 76),
                    VColor.FromRGB24(100, 76, 46),
                    VColor.FromRGB24(192, 174, 120),
                    VColor.FromRGB24(150, 106, 74),

                    VColor.FromRGB24(192, 136, 120),
                    VColor.FromRGB24(108, 104, 90),
                    VColor.FromRGB24(132, 156, 118),
                    VColor.FromRGB24(178, 142, 98),
                    VColor.FromRGB24(152, 140, 74),
                    VColor.FromRGB24(74, 68, 60),
                    VColor.FromRGB24(114, 122, 80),
                    VColor.FromRGB24(120, 74, 72),

                    VColor.FromRGB24(100, 94, 44),
                    VColor.FromRGB24(48, 44, 40),
                    VColor.FromRGB24(92, 52, 46),
                    VColor.FromRGB24(158, 150, 142),
                    VColor.FromRGB24(142, 142, 74),
                    VColor.FromRGB24(82, 64, 52),
                    VColor.FromRGB24(62, 100, 74),
                    VColor.FromRGB24(156, 112, 46),

                    VColor.FromRGB24(108, 134, 152),
                    VColor.FromRGB24(190, 190, 100),
                    VColor.FromRGB24(182, 110, 58),
                    VColor.FromRGB24(206, 186, 84),
                    VColor.FromRGB24(176, 138, 64),
                    VColor.FromRGB24(152, 86, 108),
                    VColor.FromRGB24(150, 94, 38),
                    VColor.FromRGB24(138, 148, 26));

                case 28: return new Pallet( //MC Wood Plus
                    VColor.FromRGB24(152, 124, 76),
                    VColor.FromRGB24(100, 76, 46),
                    VColor.FromRGB24(192, 174, 120),
                    VColor.FromRGB24(150, 106, 74),

                    VColor.FromRGB24(192, 136, 120),
                    VColor.FromRGB24(108, 104, 90),
                    VColor.FromRGB24(132, 156, 118),
                    VColor.FromRGB24(178, 142, 98),
                    VColor.FromRGB24(152, 140, 74),
                    VColor.FromRGB24(74, 68, 60),
                    VColor.FromRGB24(114, 122, 80),
                    VColor.FromRGB24(120, 74, 72),

                    VColor.FromRGB24(100, 94, 44),
                    VColor.FromRGB24(48, 44, 40),
                    VColor.FromRGB24(92, 52, 46),
                    VColor.FromRGB24(158, 150, 142),
                    VColor.FromRGB24(142, 142, 74),
                    VColor.FromRGB24(82, 64, 52),
                    VColor.FromRGB24(62, 100, 74),
                    VColor.FromRGB24(156, 112, 46),

                    VColor.FromRGB24(108, 134, 152),
                    VColor.FromRGB24(190, 190, 100),
                    VColor.FromRGB24(182, 110, 58),
                    VColor.FromRGB24(206, 186, 84),
                    VColor.FromRGB24(176, 138, 64),
                    VColor.FromRGB24(152, 86, 108),
                    VColor.FromRGB24(150, 94, 38),
                    VColor.FromRGB24(138, 148, 26),


                    VColor.FromRGB24(168, 114, 62),
                    VColor.FromRGB24(190, 78, 32),
                    VColor.FromRGB24(174, 156, 88),
                    VColor.FromRGB24(224, 164, 78),
                    VColor.FromRGB24(176, 112, 24),
                    VColor.FromRGB24(200, 132, 64),
                    VColor.FromRGB24(164, 116, 68),
                    VColor.FromRGB24(188, 160, 134),

                    VColor.FromRGB24(176, 136, 82),
                    VColor.FromRGB24(234, 222, 180),
                    VColor.FromRGB24(214, 152, 106),
                    VColor.FromRGB24(202, 164, 128),
                    VColor.FromRGB24(170, 164, 156),
                    VColor.FromRGB24(220, 152, 86),
                    VColor.FromRGB24(88, 46, 36),
                    VColor.FromRGB24(206, 150, 108),

                    VColor.FromRGB24(160, 114, 88),
                    VColor.FromRGB24(180, 108, 76),
                    VColor.FromRGB24(170, 136, 118),
                    VColor.FromRGB24(174, 132, 108),
                    VColor.FromRGB24(212, 150, 26),
                    VColor.FromRGB24(198, 166, 166),
                    VColor.FromRGB24(100, 48, 74),
                    VColor.FromRGB24(146, 38, 30),

                    VColor.FromRGB24(110, 44, 0),
                    VColor.FromRGB24(176, 130, 80),
                    VColor.FromRGB24(220, 152, 120),
                    VColor.FromRGB24(80, 20, 38),
                    VColor.FromRGB24(218, 172, 84),
                    VColor.FromRGB24(232, 226, 212),
                    VColor.FromRGB24(156, 152, 116),
                    VColor.FromRGB24(196, 134, 76),

                    VColor.FromRGB24(104, 8, 0),
                    VColor.FromRGB24(220, 204, 154),
                    VColor.FromRGB24(224, 118, 142));

                case 29: return new Pallet( //Pico-8
                    VColor.FromRGB24(2, 4, 8),
                    VColor.FromRGB24(29, 43, 83),
                    VColor.FromRGB24(126, 37, 83),
                    VColor.FromRGB24(0, 135, 81),
                    VColor.FromRGB24(171, 82, 54),
                    VColor.FromRGB24(95, 87, 79),
                    VColor.FromRGB24(194, 195, 199),
                    VColor.FromRGB24(255, 241, 232),
                    VColor.FromRGB24(255, 0, 77),
                    VColor.FromRGB24(255, 163, 0),
                    VColor.FromRGB24(255, 263, 39),
                    VColor.FromRGB24(0, 228, 54),
                    VColor.FromRGB24(41, 173, 255),
                    VColor.FromRGB24(131, 118, 156),
                    VColor.FromRGB24(255, 119, 168),
                    VColor.FromRGB24(255, 204, 170));

                case 30: return new Pallet( //DawnBringer16
                    VColor.FromRGB24(20, 12, 28),
                    VColor.FromRGB24(68, 36, 52),
                    VColor.FromRGB24(48, 52, 109),
                    VColor.FromRGB24(78, 74, 78),
                    VColor.FromRGB24(133, 76, 48),
                    VColor.FromRGB24(52, 101, 36),
                    VColor.FromRGB24(208, 70, 72),
                    VColor.FromRGB24(117, 113, 97),
                    VColor.FromRGB24(89, 125, 206),
                    VColor.FromRGB24(210, 125, 44),
                    VColor.FromRGB24(133, 149, 161),
                    VColor.FromRGB24(109, 170, 44),
                    VColor.FromRGB24(210, 170, 153),
                    VColor.FromRGB24(109, 194, 202),
                    VColor.FromRGB24(218, 212, 94),
                    VColor.FromRGB24(222, 238, 214));

                case 31: return new Pallet( //Kensler16
                    VColor.FromRGB(0.25, 0.195, 0.681),
                    VColor.FromRGB(0.889, 0.058, 0.759),
                    VColor.FromRGB(0.728, 0.664, 0.998),
                    VColor.FromRGB(1.0, 1.0, 1.0),
                    VColor.FromRGB(0.999, 0.582, 0.617),
                    VColor.FromRGB(0.907, 0.008, 0.0),
                    VColor.FromRGB(0.478, 0.142, 0.241),
                    VColor.FromRGB(0.0, 0.0, 0.0),
                    VColor.FromRGB(0.1, 0.336, 0.283),
                    VColor.FromRGB(0.416, 0.539, 0.154),
                    VColor.FromRGB(0.089, 0.929, 0.459),
                    VColor.FromRGB(0.197, 0.756, 0.763),
                    VColor.FromRGB(0.022, 0.499, 0.758),
                    VColor.FromRGB(0.433, 0.307, 0.140),
                    VColor.FromRGB(0.787, 0.562, 0.3),
                    VColor.FromRGB(0.935, 0.89, 0.023));

                case 32: return new Pallet( //DawnBringer32
                    VColor.FromRGB24(0, 0, 0),
                    VColor.FromRGB24(34, 32, 52),
                    VColor.FromRGB24(69, 40, 60),
                    VColor.FromRGB24(102, 57, 49),
                    VColor.FromRGB24(143, 86, 59),
                    VColor.FromRGB24(223, 113, 38),
                    VColor.FromRGB24(217, 160, 102),
                    VColor.FromRGB24(238, 195, 154),

                    VColor.FromRGB24(251, 242, 54),
                    VColor.FromRGB24(153, 229, 80),
                    VColor.FromRGB24(106, 190, 48),
                    VColor.FromRGB24(55, 148, 110),
                    VColor.FromRGB24(75, 105, 47),
                    VColor.FromRGB24(82, 75, 36),
                    VColor.FromRGB24(50, 60, 57),
                    VColor.FromRGB24(63, 63, 116),

                    VColor.FromRGB24(48, 96, 130),
                    VColor.FromRGB24(91, 110, 225),
                    VColor.FromRGB24(99, 155, 255),
                    VColor.FromRGB24(95, 205, 228),
                    VColor.FromRGB24(203, 219, 252),
                    VColor.FromRGB24(255, 255, 255),
                    VColor.FromRGB24(155, 173, 183),
                    VColor.FromRGB24(132, 126, 135),

                    VColor.FromRGB24(105, 106, 106),
                    VColor.FromRGB24(89, 86, 82),
                    VColor.FromRGB24(118, 66, 138),
                    VColor.FromRGB24(172, 50, 50),
                    VColor.FromRGB24(217, 87, 99),
                    VColor.FromRGB24(215, 123, 186),
                    VColor.FromRGB24(143, 151, 74),
                    VColor.FromRGB24(138, 111, 48));



                    
            }

            throw new InvalidOperationException();
        }

        #endregion

        #region Thread Safe Methods

        private delegate void DelegateDrawMyImage();
        private DelegateDrawMyImage DrawMyImageDelegate;

        /**
         *  Draws the contents of the buffer image to the screen.
         *  This allows us to show the image as it's being rendered.
         */
        private void DrawMyImage()
        {
            if (this.InvokeRequired)
            {
                //must invoke the delegate to be thread safe
                this.Invoke(DrawMyImageDelegate);
            }
            else
            {
                //creates the graphics object for the canvis
                Graphics gfx = pnlCanvas.CreateGraphics();
                gfx.InterpolationMode = InterpolationMode.NearestNeighbor;

                //clears the background
                gfx.Clear(System.Drawing.Color.White);

                //computes the ideal width and height for the image
                int w0 = myimage.Width;
                int h0 = myimage.Height;
                int w1 = pnlCanvas.Width;
                int h1 = pnlCanvas.Height;

                int wp = w1;
                int hp = wp * h0 / w0;

                if (hp > h1)
                {
                    hp = h1;
                    wp = hp * w0 / h0;
                }

                //locks the image while we draw it
                lock (myimage.Key)
                {
                    gfx.DrawImage((Bitmap)myimage, 0, 0, wp, hp);
                }

                //releases the graphics object
                gfx.Dispose();
            }
        }

        private delegate void DelegateDrawMyPallet();
        private DelegateDrawMyPallet DrawMyPalletDelegate;

        /**
         *  Draws the contents of the buffer image to the screen.
         *  This allows us to show the image as it's being rendered.
         */
        private void DrawMyPallet()
        {
            if (this.InvokeRequired)
            {
                //must invoke the delegate to be thread safe
                this.Invoke(DrawMyPalletDelegate);
            }
            else
            {
                //creates the graphics object for the canvis
                Graphics gfx = pnlPallet.CreateGraphics();
                gfx.InterpolationMode = InterpolationMode.NearestNeighbor;

                //clears the background
                gfx.Clear(System.Drawing.Color.White);

                //obtains the widht and height of the preview pain
                int w = pnlPallet.Width;
                int h = pnlPallet.Height;

                //locks the image while we draw it
                lock (myimage.Key)
                {
                    gfx.DrawImage((Bitmap)preview, 0, 0, w, h);
                }

                //releases the graphics object
                gfx.Dispose();
            }
        }

        private delegate void DelegateIncrementBar();
        private DelegateIncrementBar IncrementBarDelegate;

        /**
         *  This allows the process to report on its progress
         *  by updateing the progress bar each time it completes
         *  a row of the rendered image.
         */
        private void IncrementBar()
        {
            if (this.InvokeRequired)
            {
                //must invoke the delegate to be thread safe
                this.Invoke(IncrementBarDelegate);
            }
            else
            {
                //we are safe, increment the bar
                barProgress.Increment(1);
                barProgress.Refresh();
            }
        }

        private delegate void DelegateAppendText(TimeSpan time);
        private DelegateAppendText AppendTextDelegate;

        /**
         *  Allosw the loading process to apend text to the info window,
         *  this is primarly to desplay error messages that occor while
         *  loading the data.
         */
        private void AppendText(TimeSpan time)
        {
            if (this.InvokeRequired)
            {
                //must invoke the delegate to be thread safe
                this.Invoke(AppendTextDelegate, time);
            }
            else
            {
                lblTime.Text = String.Format("Time:  {0}", time);
                lblTime.Refresh();
            }
        }

        #endregion


        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"H:\Programing\TestImage";
            ofd.ShowDialog(this);
            LoadFile(ofd.FileName);
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = @"H:\Programing\TestImage\Output";
            sfd.ShowDialog(this);

            lock (myimage.Key)
            {
                Bitmap bmp = (Bitmap)myimage;
                bmp.Save(sfd.FileName);
            }
        }

        private void btnOriginal_Click(object sender, EventArgs e)
        {
            RenderSource();
        }

        private void btnPallet_Click(object sender, EventArgs e)
        {
            RenderPallet();
        }

        private void btnDither_Click(object sender, EventArgs e)
        {
            RenderDither();
        }

        private void btnFloyd_Click(object sender, EventArgs e)
        {
            RenderFS();
        }
    }
}
