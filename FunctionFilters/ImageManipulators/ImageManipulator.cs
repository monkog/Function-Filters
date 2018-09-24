using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Media;
using FunctionFilters.Helpers;
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
		/// <returns>Image brush.</returns>
		public static ImageBrush ApplyManipulation(this Bitmap source, int[] colorMap)
		{
			var outputBitmap = new Bitmap(source.Width, source.Height);

			for (int i = 0; i < outputBitmap.Width; i++)
				for (int j = 0; j < outputBitmap.Height; j++)
				{
					var color = source.GetPixel(i, j);
					var resultColor = Color.FromArgb(color.A, colorMap[color.R], colorMap[color.G], colorMap[color.B]);
					outputBitmap.SetPixel(i, j, resultColor);
				}

			return outputBitmap.CreateImageBrush();
		}
	}
}