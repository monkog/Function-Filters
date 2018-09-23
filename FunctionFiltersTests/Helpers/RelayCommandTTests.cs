using FunctionFilters.Helpers;
using Xunit;

namespace FunctionFiltersTests.Helpers
{
	public class RelayCommandTTests
	{
		[Fact]
		public void CanExecute_Always_True()
		{
			var unitUnderTest = new RelayCommand<object>(param => { });
			var result = unitUnderTest.CanExecute(null);
			Assert.True(result);
		}

		[Fact]
		public void Execute_Parameter_Executed()
		{
			var value = string.Empty;
			var passedParam = "Hello";
			var unitUnderTest = new RelayCommand<string>(param => { value = param; });

			unitUnderTest.Execute(passedParam);

			Assert.Equal(passedParam, value);
		}

	}
}