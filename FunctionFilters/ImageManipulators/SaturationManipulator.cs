using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Media;
using FunctionFilters.Helpers;
using Color = System.Drawing.Color;

namespace FunctionFilters.ImageManipulators
{
	[ExcludeFromCodeCoverage]
	public static class SaturationManipulator
	{
		private static readonly int[] ColorMap;

		static SaturationManipulator()
		{
			ColorMap = new int[256];

			int position = 0;
			int value = 0;

			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 32; j++)
					ColorMap[position++] = value;
				value = Math.Abs(value - 255);
			}
		}

		/// <summary>
		/// For all colors sets maximum saturation or minimum saturation.
		/// </summary>
		public static ImageBrush MixColors(this Bitmap source)
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