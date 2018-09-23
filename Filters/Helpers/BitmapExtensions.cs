using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Filters.Helpers
{
	[ExcludeFromCodeCoverage]
	public static class BitmapExtensions
	{
		/// <summary>
		/// Creates BitmapImage from provided Bitmap.
		/// </summary>
		/// <param name="bitmap">Provided Bitmap</param>
		/// <returns>Converted BitmapImage</returns>
		public static ImageBrush CreateImageBrush(this Bitmap bitmap)
		{
			var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
				bitmap.GetHbitmap()
				, IntPtr.Zero
				, Int32Rect.Empty
				, BitmapSizeOptions.FromEmptyOptions());

			return new ImageBrush(bitmapSource);
		}
	}
}