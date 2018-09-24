using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Media;

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
			return source.ApplyManipulation(ColorMap);
		}
	}
}