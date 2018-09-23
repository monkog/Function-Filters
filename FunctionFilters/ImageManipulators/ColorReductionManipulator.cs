using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Media;
using FunctionFilters.Helpers;
using Color = System.Drawing.Color;

namespace FunctionFilters.ImageManipulators
{
	/// <summary>
	/// Reduces the number of colors to 64.
	/// </summary>
	[ExcludeFromCodeCoverage]
	public static class ColorReductionManipulator
	{
		private static readonly int[] ColorMap;

		static ColorReductionManipulator()
		{
			ColorMap = new int[256];

			// Map all possible color values to four values.
			int position = 0;
			for (int i = 0; i < 4; i++)
				for (int j = 0; j < 64; j++)
					ColorMap[position++] = i * 64;
		}

		/// <summary>
		/// Reduces the number of colors in the provided bitmap, making it look like a poster.
		/// </summary>
		/// <param name="source">Source bitmap.</param>
		/// <returns>Poster-like image brush.</returns>
		public static ImageBrush ConvertToPoster(this Bitmap source)
		{
			var outputBitmap = new Bitmap(source.Width, source.Height);

			for (int i = 0; i < outputBitmap.Width; i++)
				for (int j = 0; j < outputBitmap.Height; j++)
				{
					var color = source.GetPixel(i, j);
					var resultColor = Color.FromArgb(color.A, ColorMap[color.R], ColorMap[color.G], ColorMap[color.B]);
					outputBitmap.SetPixel(i, j, resultColor);
				}

			return outputBitmap.CreateImageBrush();
		}
	}
}