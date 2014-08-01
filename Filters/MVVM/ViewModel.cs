using System.Collections.Generic;
using System.ComponentModel;
using OxyPlot;
using OxyPlot.Wpf;
using LineSeries = OxyPlot.Series.LineSeries;

namespace Filters.MVVM
{
    /// <summary>
    /// MVVM model for ploting functions.
    /// </summary>
    class ViewModel : INotifyPropertyChanged
    {
        private IDataPoint m_lastScreenPoint;
        private IDataPoint m_selectedScreenPoint;
        private bool m_isMouseMoving;
        public LineSeries m_currentLineSeries;
        private PlotModel m_plotModel;
        public PlotModel m_publicPlotModel
        {
            get { return m_plotModel; }
            set { m_plotModel = value; OnPropertyChanged("m_publicPlotModel"); }
        }

        public ViewModel()
        {
            m_isMouseMoving = false;
            m_publicPlotModel = new PlotModel();
            initializeLineSeries(out m_currentLineSeries);

            m_publicPlotModel.Series.Add(m_currentLineSeries);
            m_publicPlotModel.MouseDown += m_publicPlotModel_MouseDown;
            m_publicPlotModel.MouseMove += m_publicPlotModel_MouseMove;
            m_publicPlotModel.MouseUp += m_publicPlotModel_MouseUp;
        }

        public void initializeLineSeries(out LineSeries lineSeries)
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
        private void m_publicPlotModel_MouseDown(object sender, OxyMouseEventArgs e)
        {
            if (e.ChangedButton != OxyMouseButton.Left)
                return;

            IDataPoint iDataPoint = m_currentLineSeries.InverseTransform(new ScreenPoint(e.Position.X, e.Position.Y));

            if (iDataPoint.X < 0 || iDataPoint.X > 255 || iDataPoint.Y < 0 || iDataPoint.Y > 255)
                return;

            foreach (var dataPoint in m_currentLineSeries.Points)
                if (iDataPoint.X <= dataPoint.X + 10 && iDataPoint.X >= dataPoint.X - 10
                    && iDataPoint.Y <= dataPoint.Y + 10 && iDataPoint.Y >= dataPoint.Y - 10)
                {
                    m_isMouseMoving = true;
                    m_selectedScreenPoint = dataPoint;
                    m_lastScreenPoint = dataPoint;
                    return;
                }

            m_currentLineSeries.Points.Add(m_currentLineSeries.InverseTransform(new ScreenPoint(e.Position.X, e.Position.Y)));
            updateModel();
            m_plotModel.RefreshPlot(true);

            m_isMouseMoving = false;
        }

        /// <summary>
        /// Moves the selected point around the ploting area.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_publicPlotModel_MouseMove(object sender, OxyMouseEventArgs e)
        {
            if (!m_isMouseMoving)
                return;

            m_lastScreenPoint = m_currentLineSeries.InverseTransform(new ScreenPoint(e.Position.X, e.Position.Y));

            if (m_lastScreenPoint.X < 0 || m_lastScreenPoint.X > 255
                || m_lastScreenPoint.Y < 0 || m_lastScreenPoint.Y > 255)
                return;

            m_currentLineSeries.Points.Remove(m_selectedScreenPoint);
            m_currentLineSeries.Points.Add(m_lastScreenPoint);
            m_selectedScreenPoint = m_lastScreenPoint;
            updateModel();
            m_plotModel.RefreshPlot(true);
        }

        void m_publicPlotModel_MouseUp(object sender, OxyMouseEventArgs e)
        {
            m_isMouseMoving = false;
        }

        /// <summary>
        /// Updates values on the plot.
        /// </summary>
        public void updateModel()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            foreach (DataPoint dataPoint in m_currentLineSeries.Points)
                dataPoints.Add(dataPoint);

            dataPoints.Sort((p, q) => p.X < q.X ? -1 : 1);

            m_currentLineSeries.Points.Clear();

            foreach (var dataPoint in dataPoints)
                m_currentLineSeries.Points.Add(dataPoint);
        }

        public IList<IDataPoint> getPoints()
        {
            return m_currentLineSeries.Points;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
