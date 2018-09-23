using System;
using System.Drawing;
using FunctionFilters.Helpers;
using Color = System.Drawing.Color;

namespace FunctionFilters
{
    public partial class MainWindow
    {
        /// <summary>
        /// Places randomly colorful dots.
        /// </summary>
        public void ColorDots()
        {
            int[] rgbColors = new int[256];
            Random random = new Random();

            for (int i = 0; i < 256; i++)
                rgbColors[i] = i;

            for (int i = 0; i < 30; i++)
                rgbColors[random.Next()%256] = random.Next()%256;

            Bitmap outputBitmap = new Bitmap(SourceBitmap.Width, SourceBitmap.Height);

            for (int i = 0; i < outputBitmap.Width; i++)
                for (int j = 0; j < outputBitmap.Height; j++)
                {
                    Color color = SourceBitmap.GetPixel(i, j);
                    color = Color.FromArgb(color.A, rgbColors[color.R], rgbColors[color.G], rgbColors[color.B]);
                    outputBitmap.SetPixel(i, j, color);
                }

            OutputPhoto.Background = outputBitmap.CreateImageBrush();
        }
    }
}