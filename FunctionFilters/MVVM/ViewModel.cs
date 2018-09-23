using System.Collections.Generic;
using System.ComponentModel;
using OxyPlot;
using LineSeries = OxyPlot.Series.LineSeries;

namespace Filters.MVVM
{
    /// <summary>
    /// MVVM model for ploting functions.
    /// </summary>
    class ViewModel : INotifyPropertyChanged
    {
        private IDataPoint _lastScreenPoint;
        private IDataPoint _selectedScreenPoint;
        private bool _isMouseMoving;
        public LineSeries CurrentLineSeries;
        private PlotModel _plotModel;
        public PlotModel PlotModel
		{
			get
			{
				return _plotModel;
			}
			set { _plotModel = value; OnPropertyChanged("PlotModel"); }
		}

		public ViewModel()
        {
            _isMouseMoving = false;
            PlotModel = new PlotModel();
            InitializeLineSeries(out CurrentLineSeries);

            PlotModel.Series.Add(CurrentLineSeries);
            PlotModel.MouseDown += PublicPlotModel_MouseDown;
            PlotModel.MouseMove += PublicPlotModel_MouseMove;
            PlotModel.MouseUp += PublicPlotModel_MouseUp;
        }

        public void InitializeLineSeries(out LineSeries lineSeries)
        {
            lineSeries = new LineSeries
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
        }

        /// <summary>
        /// Translates mouse position to the point on the plot.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PublicPlotModel_MouseDown(object sender, OxyMouseEventArgs e)
        {
            if (e.ChangedButton != OxyMouseButton.Left)
                return;

            IDataPoint iDataPoint = CurrentLineSeries.InverseTransform(new ScreenPoint(e.Position.X, e.Position.Y));

            if (iDataPoint.X < 0 || iDataPoint.X > 255 || iDataPoint.Y < 0 || iDataPoint.Y > 255)
                return;

            foreach (var dataPoint in CurrentLineSeries.Points)
                if (iDataPoint.X <= dataPoint.X + 10 && iDataPoint.X >= dataPoint.X - 10
                    && iDataPoint.Y <= dataPoint.Y + 10 && iDataPoint.Y >= dataPoint.Y - 10)
                {
                    _isMouseMoving = true;
                    _selectedScreenPoint = dataPoint;
                    _lastScreenPoint = dataPoint;
                    return;
                }

            CurrentLineSeries.Points.Add(CurrentLineSeries.InverseTransform(new ScreenPoint(e.Position.X, e.Position.Y)));
            UpdateModel();
            _plotModel.RefreshPlot(true);

            _isMouseMoving = false;
        }

        /// <summary>
        /// Moves the selected point around the ploting area.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PublicPlotModel_MouseMove(object sender, OxyMouseEventArgs e)
        {
            if (!_isMouseMoving)
                return;

            _lastScreenPoint = CurrentLineSeries.InverseTransform(new ScreenPoint(e.Position.X, e.Position.Y));

            if (_lastScreenPoint.X < 0 || _lastScreenPoint.X > 255
                || _lastScreenPoint.Y < 0 || _lastScreenPoint.Y > 255)
                return;

            CurrentLineSeries.Points.Remove(_selectedScreenPoint);
            CurrentLineSeries.Points.Add(_lastScreenPoint);
            _selectedScreenPoint = _lastScreenPoint;
            UpdateModel();
            _plotModel.RefreshPlot(true);
        }

        void PublicPlotModel_MouseUp(object sender, OxyMouseEventArgs e)
        {
            _isMouseMoving = false;
        }

        /// <summary>
        /// Updates values on the plot.
        /// </summary>
        public void UpdateModel()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            foreach (DataPoint dataPoint in CurrentLineSeries.Points)
                dataPoints.Add(dataPoint);

            dataPoints.Sort((p, q) => p.X < q.X ? -1 : 1);

            CurrentLineSeries.Points.Clear();

            foreach (var dataPoint in dataPoints)
                CurrentLineSeries.Points.Add(dataPoint);
        }

        public IList<IDataPoint> GetPoints()
        {
            return CurrentLineSeries.Points;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
	        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
