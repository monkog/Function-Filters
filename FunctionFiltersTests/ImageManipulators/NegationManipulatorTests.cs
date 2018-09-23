using System.Drawing;
using FunctionFilters.ImageManipulators;
using Xunit;

namespace FunctionFiltersTests.ImageManipulators
{
	public class NegationManipulatorTests
	{
		[Fact]
		public void Negate_White_Black()
		{
			var black = Color.Black;
			var result = Color.White.Negate();

			Assert.Equal(black.A, result.A);
			Assert.Equal(black.R, result.R);
			Assert.Equal(black.G, result.G);
			Assert.Equal(black.B, result.B);
		}

		[Fact]
		public void Negate_Black_White()
		{
			var white = Color.White;
			var result = Color.Black.Negate();

			Assert.Equal(white.A, result.A);
			Assert.Equal(white.R, result.R);
			Assert.Equal(white.G, result.G);
			Assert.Equal(white.B, result.B);
		}

		[Fact]
		public void Negate_Color_Negated()
		{
			var color = Color.FromArgb(15, 30, 55);
			var result = color.Negate();

			Assert.Equal(color.A, result.A);
			Assert.Equal(240, result.R);
			Assert.Equal(225, result.G);
			Assert.Equal(200, result.B);
		}
	}
}