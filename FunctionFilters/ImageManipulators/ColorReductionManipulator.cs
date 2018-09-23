using System.Drawing;
using System.Windows.Media;
using FunctionFilters.Helpers;
using Color = System.Drawing.Color;

namespace FunctionFilters.ImageManipulators
{
	public static class ColorReductionManipulator
	{
		/// <summary>
		/// Reduces the number of colors in the provided bitmap, making it look like a poster.
		/// </summary>
		/// <param name="source">Source bitmap.</param>
		/// <returns>Poster-like image brush.</returns>
		public static ImageBrush ConvertToPoster(this Bitmap source)
		{
			int position = 0;
			int[] rgbColors = new int[256];
			for (int i = 0; i < 5; i++)
				for (int j = 0; j < 51; j++)
					rgbColors[position++] = i * 51;
			rgbColors[position] = 255;

			Bitmap outputBitmap = new Bitmap(source.Width, source.Height);

			for (int i = 0; i < outputBitmap.Width; i++)
				for (int j = 0; j < outputBitmap.Height; j++)
				{
					Color color = source.GetPixel(i, j);
					color = Color.FromArgb(color.A, rgbColors[color.R], rgbColors[color.G], rgbColors[color.B]);
					outputBitmap.SetPixel(i, j, color);
				}

			return outputBitmap.CreateImageBrush();
		}
	}
}