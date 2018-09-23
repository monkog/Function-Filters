namespace Filters.Helpers
{
	public static class ColorExtensions
	{
		/// <summary>
		/// Returns R,G and B values in range [0,255].
		/// </summary>
		/// <param name="n">R,G,B value in range [0,1]</param>
		/// <returns>R,G,B value in range [0,255]</returns>
		public static int ToRgb(this double n)
		{
			if (n >= 0)
				if (n <= 255)
					return (int)n;
				else
					return 255;
			return 0;
		}
	}
}