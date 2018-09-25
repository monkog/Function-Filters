using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Color = System.Drawing.Color;

namespace FunctionFilters.ImageManipulators
{
	[ExcludeFromCodeCoverage]
	public static class ImageManipulator
	{
		/// <summary>
		/// Manipulates the bitmap using the provided color map.
		/// </summary>
		/// <param name="source">Source bitmap.</param>
		/// <param name="colorMap">Color map.</param>
		/// <param name="colors">Description which color channels should be manipulated.</param>
		/// <returns>Resulting bitmap.</returns>
		public static Bitmap ApplyManipulation(this Bitmap source, int[] colorMap, ColorChannel colors)
		{
			var outputBitmap = new Bitmap(source.Width, source.Height);
			var manipulateRed = (colors & ColorChannel.Red) == ColorChannel.Red;
			var manipulateGreen = (colors & ColorChannel.Green) == ColorChannel.Green;
			var manipulateBlue = (colors & ColorChannel.Blue) == ColorChannel.Blue;

			for (int i = 0; i < outputBitmap.Width; i++)
				for (int j = 0; j < outputBitmap.Height; j++)
				{
					var color = source.GetPixel(i, j);

					var red = manipulateRed ? colorMap[color.R] : color.R;
					var green = manipulateGreen ? colorMap[color.G] : color.G;
					var blue = manipulateBlue ? colorMap[color.B] : color.B;

					var resultColor = Color.FromArgb(color.A, red, green, blue);
					outputBitmap.SetPixel(i, j, resultColor);
				}

			return outputBitmap;
		}
	}
}