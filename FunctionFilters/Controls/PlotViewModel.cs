using System.Collections.Generic;
using System.ComponentModel;
using OxyPlot;
using LineSeries = OxyPlot.Series.LineSeries;

namespace FunctionFilters.Controls
{
	/// <summary>
	/// View model for plot control.
	/// </summary>
	public class PlotViewModel : INotifyPropertyChanged
	{
		private IDataPoint _lastScreenPoint;
		private IDataPoint _selectedScreenPoint;
		private bool _isMouseMoving;

		private readonly LineSeries _currentLineSeries;
		private PlotModel _plotModel;

		/// <summary>
		/// Gets the collection of points in the plot.
		/// </summary>
		public IList<IDataPoint> Points => _currentLineSeries.Points;

		/// <summary>
		/// Gets or sets the plot model.
		/// </summary>
		public PlotModel PlotModel
		{
			get { return _plotModel; }
			set
			{
				_plotModel = value;
				OnPropertyChanged(nameof(PlotModel));
			}
		}

		public PlotViewModel()
		{
			_isMouseMoving = false;
			_plotModel = new PlotModel();
			_currentLineSeries = InitializeLineSeries();

			_plotModel.Series.Add(_currentLineSeries);
			_plotModel.MouseDown += Plot_MouseDown;
			_plotModel.MouseMove += Plot_MouseMove;
			_plotModel.MouseUp += Plot_MouseUp;
		}

		/// <summary>
		/// Sorts values on the plot.
		/// </summary>
		public void SortPointsCollection()
		{
			var points = new List<IDataPoint>(_currentLineSeries.Points);
			points.Sort((p, q) => p.X < q.X ? -1 : 1);
			_currentLineSeries.Points = points;
		}

		private LineSeries InitializeLineSeries()
		{
			var lineSeries = new LineSeries
			{
				StrokeThickness = 2,
				MarkerSize = 3,
				MarkerStroke = OxyColors.Black,
				MarkerType = MarkerType.Circle,
				Color = OxyColors.Black,
				CanTrackerInterpolatePoints = true
			};

			lineSeries.Points.Add(new DataPoint(0, 0));
			lineSeries.Points.Add(new DataPoint(255, 255));
			return lineSeries;
		}

		private void Plot_MouseDown(object sender, OxyMouseEventArgs e)
		{
			if (e.ChangedButton != OxyMouseButton.Left)
				return;

			var point = _currentLineSeries.InverseTransform(new ScreenPoint(e.Position.X, e.Position.Y));

			if (point.X < 0 || point.X > 255 || point.Y < 0 || point.Y > 255)
				return;

			foreach (var seriesPoint in _currentLineSeries.Points)
				if (point.X <= seriesPoint.X + 10 && point.X >= seriesPoint.X - 10
					&& point.Y <= seriesPoint.Y + 10 && point.Y >= seriesPoint.Y - 10)
				{
					_isMouseMoving = true;
					_selectedScreenPoint = seriesPoint;
					_lastScreenPoint = seriesPoint;
					return;
				}

			_currentLineSeries.Points.Add(_currentLineSeries.InverseTransform(new ScreenPoint(e.Position.X, e.Position.Y)));
			SortPointsCollection();
			_plotModel.RefreshPlot(true);

			_isMouseMoving = false;
		}

		private void Plot_MouseMove(object sender, OxyMouseEventArgs e)
		{
			if (!_isMouseMoving)
				return;

			_lastScreenPoint = _currentLineSeries.InverseTransform(new ScreenPoint(e.Position.X, e.Position.Y));

			if (_lastScreenPoint.X < 0 || _lastScreenPoint.X > 255
				|| _lastScreenPoint.Y < 0 || _lastScreenPoint.Y > 255)
				return;

			_currentLineSeries.Points.Remove(_selectedScreenPoint);
			_currentLineSeries.Points.Add(_lastScreenPoint);
			_selectedScreenPoint = _lastScreenPoint;
			SortPointsCollection();
			_plotModel.RefreshPlot(true);
		}

		private void Plot_MouseUp(object sender, OxyMouseEventArgs e)
		{
			_isMouseMoving = false;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
