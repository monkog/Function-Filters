using System.Linq;
using FunctionFilters.Controls;
using OxyPlot;
using Xunit;

namespace FunctionFiltersTests.Controls
{
	public class PlotViewModelTests
	{
		private readonly PlotViewModel _unitUnderTest;

		public PlotViewModelTests()
		{
			_unitUnderTest = new PlotViewModel();
		}

		[Fact]
		public void Constructor_Always_PlotInitialized()
		{
			Assert.NotNull(_unitUnderTest.PlotModel);
		}

		[Fact]
		public void Constructor_Always_MinAndMaxPointsExist()
		{
			Assert.Equal(2, _unitUnderTest.Points.Count);

			Assert.Equal(0, _unitUnderTest.Points.First().X);
			Assert.Equal(0, _unitUnderTest.Points.First().Y);

			Assert.Equal(255, _unitUnderTest.Points.Last().X);
			Assert.Equal(255, _unitUnderTest.Points.Last().Y);
		}

		[Theory]
		[InlineData(125, -3)]
		[InlineData(125, 125)]
		[InlineData(125, 366)]
		public void Sort_XBetweenExistingValues_LineSeriesPointsSortedByX(int x, int y)
		{
			var point = new DataPoint(x, y);
			_unitUnderTest.Points.Add(point);
			_unitUnderTest.SortPointsCollection();

			Assert.Equal(3, _unitUnderTest.Points.Count);
			Assert.Equal(point, _unitUnderTest.Points[1]);
		}

		[Fact]
		public void MouseDown_RightButton_PointsCollectionUnchanged()
		{
			var args = new OxyMouseEventArgs { ChangedButton = OxyMouseButton.Right, Position = new ScreenPoint(125, 125) };
			var points = _unitUnderTest.Points;

			_unitUnderTest.PlotModel.HandleMouseDown(null, args);

			Assert.StrictEqual(points, _unitUnderTest.Points);
		}
	}
}