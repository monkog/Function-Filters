using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Media;
using FunctionFilters.Helpers;
using Color = System.Drawing.Color;

namespace FunctionFilters.ImageManipulators
{
	public static class NegationManipulator
	{
		/// <summary>
		/// Converts image to its negative.
		/// </summary>
		/// <param name="source">Bitmap to manipulate.</param>
		/// <returns>Image brush with negated bitmap.</returns>
		[ExcludeFromCodeCoverage]
		public static ImageBrush Negate(this Bitmap source)
		{
			var outputBitmap = new Bitmap(source.Width, source.Height);

			for (int i = 0; i < outputBitmap.Width; i++)
				for (int j = 0; j < outputBitmap.Height; j++)
				{
					var color = source.GetPixel(i, j);
					outputBitmap.SetPixel(i, j, color.Negate());
				}

			return outputBitmap.CreateImageBrush();
		}

		/// <summary>
		/// Calculates the negative color.
		/// </summary>
		/// <param name="color">Color to negate.</param>
		/// <returns>Negated color.</returns>
		public static Color Negate(this Color color)
		{
			return Color.FromArgb(color.A, 255 - color.R, 255 - color.G, 255 - color.B);
		}
	}
}