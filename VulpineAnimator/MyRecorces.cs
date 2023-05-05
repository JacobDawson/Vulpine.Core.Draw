using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace VulpineAnimator
{
    public static class MyRecorces
    {
        private static string Path = @"H:\Programing\MainProdjects\VPCL\SourceData\ForAnimator\";

        public static ImageSys Braunschweig
        {
            get
            {
                Image bmp = Bitmap.FromFile(Path + "Braunschweig_Church_Hall.jpg");
                return new ImageSys((Bitmap)bmp);
            }
        }

        public static ImageSys Rocket_Alley
        {
            get
            {
                Image bmp = Bitmap.FromFile(Path + "Rocket_Alley.jpg");
                return new ImageSys((Bitmap)bmp);
            }
        }

        public static ImageSys Wall_Clock
        {
            get
            {
                Image bmp = Bitmap.FromFile(Path + "WallClock2.png");
                return new ImageSys((Bitmap)bmp);
            }
        }
    }
}
