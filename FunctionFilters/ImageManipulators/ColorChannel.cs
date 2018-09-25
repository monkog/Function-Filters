using System;

namespace FunctionFilters.ImageManipulators
{
	/// <summary>
	/// Defines which color channel should be manipulated.
	/// </summary>
	[Flags]
	public enum ColorChannel
	{
		Red = 0,
		Green = 1,
		Blue = 2,
		All = Red | Green | Blue
	}
}