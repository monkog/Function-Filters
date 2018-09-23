using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Filters.Helpers;
using Color = System.Drawing.Color;

namespace Filters
{
    public partial class MainWindow
    {
        /// <summary>
        /// Creates Bitmap from the provided BitmapImage.
        /// </summary>
        /// <param name="bitmapImage">Provided BitmapImage</param>
        /// <returns>Bitmap from BitmapImage</returns>
        public Bitmap createBitmapFromBitmapImage(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder bitmapEncoder = new BmpBitmapEncoder();
                bitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                bitmapEncoder.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        /// <summary>
        /// Converts image to its negative.
        /// </summary>
        public void negate()
        {
            Bitmap outputBitmap = new Bitmap((int)SourceBitmap.Width, (int)SourceBitmap.Height);

            for (int i = 0; i < outputBitmap.Width; i++)
                for (int j = 0; j < outputBitmap.Height; j++)
                {
                    Color color = SourceBitmap.GetPixel(i, j);
                    color = Color.FromArgb(color.A, 255 - color.R, 255 - color.G, 255 - color.B);
                    outputBitmap.SetPixel(i, j, color);
                }

            OutputPhoto.Background = BitmapExtensions.CreateImageBrush(outputBitmap);
        }

        /// <summary>
        /// Makes the loaded photo look like a poster.
        /// </summary>
        public void makePoster()
        {
            int position = 0;
            int[] rgbColors = new int[256];
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 51; j++)
                    rgbColors[position++] = i * 51;
            rgbColors[position] = 255;

            Bitmap outputBitmap = new Bitmap((int)SourceBitmap.Width, (int)SourceBitmap.Height);

            for (int i = 0; i < outputBitmap.Width; i++)
                for (int j = 0; j < outputBitmap.Height; j++)
                {
                    Color color = SourceBitmap.GetPixel(i, j);
                    color = Color.FromArgb(color.A, rgbColors[color.R], rgbColors[color.G], rgbColors[color.B]);
                    outputBitmap.SetPixel(i, j, color);
                }

            OutputPhoto.Background = BitmapExtensions.CreateImageBrush(outputBitmap);
        }

        /// <summary>
        /// For all colors sets maximum saturation or minimum saturation.
        /// </summary>
        public void mixColors()
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

            Bitmap outputBitmap = new Bitmap((int)SourceBitmap.Width, (int)SourceBitmap.Height);

            for (int i = 0; i < outputBitmap.Width; i++)
                for (int j = 0; j < outputBitmap.Height; j++)
                {
                    Color color = SourceBitmap.GetPixel(i, j);
                    color = Color.FromArgb(color.A, rgbColors[color.R], rgbColors[color.G], rgbColors[color.B]);
                    outputBitmap.SetPixel(i, j, color);
                }

            OutputPhoto.Background = BitmapExtensions.CreateImageBrush(outputBitmap);
        }

        /// <summary>
        /// Places randomly colorful dots.
        /// </summary>
        public void colorDots()
        {
            int[] rgbColors = new int[256];
            Random random = new Random();

            for (int i = 0; i < 256; i++)
                rgbColors[i] = i;

            for (int i = 0; i < 30; i++)
                rgbColors[random.Next()%256] = random.Next()%256;

            Bitmap outputBitmap = new Bitmap((int)SourceBitmap.Width, (int)SourceBitmap.Height);

            for (int i = 0; i < outputBitmap.Width; i++)
                for (int j = 0; j < outputBitmap.Height; j++)
                {
                    Color color = SourceBitmap.GetPixel(i, j);
                    color = Color.FromArgb(color.A, rgbColors[color.R], rgbColors[color.G], rgbColors[color.B]);
                    outputBitmap.SetPixel(i, j, color);
                }

            OutputPhoto.Background = BitmapExtensions.CreateImageBrush(outputBitmap);
        }

        #region Saving files.
        /// <summary>
        /// Saves the converted image with .bmp extension.
        /// </summary>
        /// <param name="visual">Image to save</param>
        /// <param name="fileName">Output file name</param>
        void saveToBmp(FrameworkElement visual, string fileName)
        {
            var encoder = new BmpBitmapEncoder();
            saveUsingEncoder(visual, fileName, encoder);
        }

        /// <summary>
        /// Saves the converted image with .png extension.
        /// </summary>
        /// <param name="visual">Image to save</param>
        /// <param name="fileName">Output file name</param>
        void saveToPng(FrameworkElement visual, string fileName)
        {
            var encoder = new PngBitmapEncoder();
            saveUsingEncoder(visual, fileName, encoder);
        }

        /// <summary>
        /// Saves the converted image with .jpeg extension.
        /// </summary>
        /// <param name="visual">Image to save</param>
        /// <param name="fileName">Output file name</param>
        void saveToJpeg(FrameworkElement visual, string fileName)
        {
            var encoder = new JpegBitmapEncoder();
            saveUsingEncoder(visual, fileName, encoder);
        }

        /// <summary>
        /// Saves the converted image with .gif extension.
        /// </summary>
        /// <param name="visual">Image to save</param>
        /// <param name="fileName">Output file name</param>
        void saveToGif(FrameworkElement visual, string fileName)
        {
            var encoder = new GifBitmapEncoder();
            saveUsingEncoder(visual, fileName, encoder);
        }

        /// <summary>
        /// Universal function to save image with a chosen extention.
        /// </summary>
        /// <param name="visual">Image to save</param>
        /// <param name="fileName">Output file name</param>
        /// <param name="encoder">Matching encoder</param>
        void saveUsingEncoder(FrameworkElement visual, string fileName, BitmapEncoder encoder)
        {
            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(visual);
            BitmapFrame frame = BitmapFrame.Create(bitmap);
            encoder.Frames.Add(frame);

            using (var stream = File.Create(fileName))
            {
                encoder.Save(stream);
            }
        }
        #endregion
    }
}