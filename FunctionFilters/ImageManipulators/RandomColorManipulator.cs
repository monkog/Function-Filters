using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Media;
using FunctionFilters.Helpers;
using Color = System.Drawing.Color;

namespace FunctionFilters.ImageManipulators
{
	[ExcludeFromCodeCoverage]
	public static class RandomColorManipulator
	{
		private static readonly int[] ColorMap;

		static RandomColorManipulator()
		{
			ColorMap = new int[256];

			var random = new Random();

			for (int i = 0; i < 256; i++)
				ColorMap[i] = i;

			for (int i = 0; i < 30; i++)
				ColorMap[random.Next() % 256] = random.Next() % 256;

		}

		/// <summary>
		/// Places randomly colorful dots.
		/// </summary>
		public static ImageBrush ColorDots(this Bitmap source)
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