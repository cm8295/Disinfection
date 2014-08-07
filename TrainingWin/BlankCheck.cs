using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing;
using System.IO;


namespace TrainingWin
{
    class BlankCheck
    {
        public double bc_Center(string path, Canvas canvas)     //c1
        {
            exporttojpg(path, canvas);        //c1
            return picprocess(path);
        }
        public double bc_Right(string path, Canvas canvas)   //c2
        {
            exporttojpg2(path, canvas);        //c2
            return picprocess2(path);
        }
        public double bc_Left(string path, Canvas canvas)   //c3
        {
            exporttojpg3(path, canvas);        //c3
            return picprocess3(path);
        }
        void exporttojpg(string path, Canvas c)
        {
            if (path == null) return;
            Size size = new Size((int)c.Width, (int)c.Height);
            RenderTargetBitmap renderbitmap = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96d, 96d, PixelFormats.Pbgra32);
            renderbitmap.Render(c);
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            using (FileStream outsream = new FileStream(path+@"\CenterImage.png", FileMode.Create))
            {
                //JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderbitmap));
                encoder.Save(outsream);
            }
        }
        private double picprocess(string path)
        {
            Int32 whites = 0;
            Bitmap bm = new Bitmap(path + @"\CenterImage.png");
            System.Drawing.Color color; int wide = (int)bm.Width; int height = (int)bm.Height;
            for (int i = 0; i < wide; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    color = bm.GetPixel(i, j);
                    if (color.R == 255 & color.B == 255 & color.G == 255)
                    {
                        whites++;
                    }
                }
            }
            double a = (double)whites / ((bm.Height - 200)* bm.Width);
            a = Math.Round(a, 3)*100;         //保留3位小数
            bm.Dispose();            
            return (a);
        }

        void exporttojpg2(string path, Canvas c)
        {
            if (path == null) return;
            Size size = new Size((int)c.Width, (int)c.Height);
            RenderTargetBitmap renderbitmap = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96d, 96d, PixelFormats.Pbgra32);
            renderbitmap.Render(c);
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            using (FileStream outsream = new FileStream(path + @"\RightImage.png", FileMode.Create))
            {
                //JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderbitmap));
                encoder.Save(outsream);
            }
        }
        private double picprocess2(string path)
        {
            Int32 whites = 0;
            Bitmap bm = new Bitmap(path + @"\RightImage.png");
            System.Drawing.Color color; int wide = (int)bm.Width; int height = (int)bm.Height;
            for (int i = 0; i < wide; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    color = bm.GetPixel(i, j);
                    if (color.R == 255 & color.B == 255 & color.G == 255)
                    {
                        whites++;
                    }
                }
            }
            double a = (double)whites / ((bm.Height - 200) * bm.Width);
            a = Math.Round(a, 3) * 100;         //保留3位小数
            bm.Dispose();
            return (a);
        }

        void exporttojpg3(string path, Canvas c)
        {
            if (path == null) return;
            Size size = new Size((int)c.Width, (int)c.Height);
            RenderTargetBitmap renderbitmap = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96d, 96d, PixelFormats.Pbgra32);
            renderbitmap.Render(c);
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            using (FileStream outsream = new FileStream(path + @"\Left_Image.png", FileMode.Create))
            {
                //JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderbitmap));
                encoder.Save(outsream);
            }
        }
        private double picprocess3(string path)
        {
            Int32 whites = 0;
            Bitmap bm = new Bitmap(path + @"\Left_Image.png");
            System.Drawing.Color color; int wide = (int)bm.Width; int height = (int)bm.Height;
            for (int i = 0; i < wide; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    color = bm.GetPixel(i, j);
                    if (color.R == 255 & color.B == 255 & color.G == 255)
                    {
                        whites++;
                    }
                }
            }
            double a = (double)whites / ((bm.Height - 200) * bm.Width);
            a = Math.Round(a, 3) * 100;         //保留3位小数
            bm.Dispose();
            return (a);
        }
    }
}
