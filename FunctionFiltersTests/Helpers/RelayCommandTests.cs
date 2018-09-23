using FunctionFilters.Helpers;
using Xunit;

namespace FunctionFiltersTests.Helpers
{
	public class RelayCommandTests
	{
		[Fact]
		public void CanExecute_Always_True()
		{
			var unitUnderTest = new RelayCommand(() => { });
			var result = unitUnderTest.CanExecute(null);
			Assert.True(result);
		}

		[Fact]
		public void Execute_Parameter_Executed()
		{
			var value = string.Empty;
			var expectedValue = "Hello";
			var unitUnderTest = new RelayCommand(() => { value = expectedValue; });

			unitUnderTest.Execute(null);

			Assert.Equal(expectedValue, value);
		}
	}

}