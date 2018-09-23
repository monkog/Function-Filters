using System;
using System.Drawing;
using Filters.Helpers;
using Color = System.Drawing.Color;

namespace Filters
{
    public partial class MainWindow
    {
        /// <summary>
        /// Converts image to its negative.
        /// </summary>
        public void Negate()
        {
            Bitmap outputBitmap = new Bitmap(SourceBitmap.Width, SourceBitmap.Height);

            for (int i = 0; i < outputBitmap.Width; i++)
                for (int j = 0; j < outputBitmap.Height; j++)
                {
                    Color color = SourceBitmap.GetPixel(i, j);
                    color = Color.FromArgb(color.A, 255 - color.R, 255 - color.G, 255 - color.B);
                    outputBitmap.SetPixel(i, j, color);
                }

            OutputPhoto.Background = outputBitmap.CreateImageBrush();
        }

        /// <summary>
        /// Makes the loaded photo look like a poster.
        /// </summary>
        public void MakePoster()
        {
            int position = 0;
            int[] rgbColors = new int[256];
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 51; j++)
                    rgbColors[position++] = i * 51;
            rgbColors[position] = 255;

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

        /// <summary>
        /// For all colors sets maximum saturation or minimum saturation.
        /// </summary>
        public void MixColors()
        {
            int position = 0;
            int value = 2;
            int[] rgbColors = new int[256];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 30; j++)
                    rgbColors[position++] = value;
                value = Math.Abs(value - 256);
            }

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