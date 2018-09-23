using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Filters.MVVM;
using OxyPlot;
using Point = System.Windows.Point;

namespace Filters
{
    /// <summary>
    /// Interaction logic for AdvancedFilters.xaml
    /// </summary>
    public partial class AdvancedFilters
    {
	    private readonly MainWindow _owner;
	    private ViewModel _viewModel;
        private int[] _rgbColorsTable;
        private int _selectedIndex;
        private Bitmap _outputBitmap;

        public AdvancedFilters(MainWindow owner)
        {
	        _owner = owner;
	        Owner = owner;
	        InitializeComponent();
        }

        private void AdvancedFilters_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel = new ViewModel();
            DataContext = _viewModel;
			
            _outputBitmap = new Bitmap(_owner.SourceBitmap);

            m_advancedFunction.DefaultTrackerTemplate = null;
            _rgbColorsTable = new int[256];
            _selectedIndex = 0;
            m_channelComboBox.SelectionChanged += ChannelComboBox_SelectionChanged;
            m_advancedFunction.MouseDown += m_advancedFunction_MouseDown;
        }

        void m_advancedFunction_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point point = Mouse.GetPosition(m_advancedFunction);
            _viewModel.CurrentLineSeries.Points.Add(new DataPoint(point.X, point.Y));
            _viewModel.UpdateModel();
        }

        private void m_okButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyFunction();
            Close();
        }

        private void ApplyFunction()
        {
            UpdateColorValuesTables();

            Bitmap outputBitmap = new Bitmap(_outputBitmap.Width, _outputBitmap.Height);

            switch (_selectedIndex)
            {
                case 0:
                    for (int i = 0; i < outputBitmap.Width; i++)
                        for (int j = 0; j < outputBitmap.Height; j++)
                        {
                            Color color = _outputBitmap.GetPixel(i, j);
                            Color newColor = Color.FromArgb(color.A, _rgbColorsTable[color.R]
                                , _rgbColorsTable[color.G]
                                , _rgbColorsTable[color.B]);
                            outputBitmap.SetPixel(i, j, newColor);
                        }
                    break;
                case 1:
                    for (int i = 0; i < outputBitmap.Width; i++)
                        for (int j = 0; j < outputBitmap.Height; j++)
                        {
                            Color color = _outputBitmap.GetPixel(i, j);
                            Color newColor = Color.FromArgb(color.A, _rgbColorsTable[color.R], color.G, color.B);
                            outputBitmap.SetPixel(i, j, newColor);
                        }
                    break;
                case 2:
                    for (int i = 0; i < outputBitmap.Width; i++)
                        for (int j = 0; j < outputBitmap.Height; j++)
                        {
                            Color color = _outputBitmap.GetPixel(i, j);
                            Color newColor = Color.FromArgb(color.A, color.R, _rgbColorsTable[color.G], color.B);
                            outputBitmap.SetPixel(i, j, newColor);
                        }
                    break;
                case 3:
                    for (int i = 0; i < outputBitmap.Width; i++)
                        for (int j = 0; j < outputBitmap.Height; j++)
                        {
                            Color color = _outputBitmap.GetPixel(i, j);
                            Color newColor = Color.FromArgb(color.A, color.R, color.G, _rgbColorsTable[color.B]);
                            outputBitmap.SetPixel(i, j, newColor);
                        }
                    break;
            }

            _selectedIndex = m_channelComboBox.SelectedIndex;
            _outputBitmap = outputBitmap;
	        _owner.OutputPhoto.Background = MainWindow.createImageBrushFromBitmap(outputBitmap);
        }

        /// <summary>
        /// Updates the corresponding R, G, B values from the plot.
        /// </summary>
        private void UpdateColorValuesTables()
        {
            IList<IDataPoint> pointsList = _viewModel.GetPoints();
            for (int i = 0; i < pointsList.Count - 1; i++)
            {
                // y = ax + b
                // y1 = ax1 + b

                // y - ax = y1 - ax1
                // a = (y1 - y)/(x1 - x)
                // b = y - a * x

                double a = (pointsList[i + 1].Y - pointsList[i].Y) / (pointsList[i + 1].X - pointsList[i].X);
                double b = pointsList[i].Y - a * pointsList[i].X;

                for (int j = (int)pointsList[i].X; j <= (int)pointsList[i + 1].X; j++)
                {
                    _rgbColorsTable[j] = (int)ToRgb(a * j + b);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Returns R,G and B values in range [0,255].
        /// </summary>
        /// <param name="n">R,G,B value in range [0,1]</param>
        /// <returns>R,G,B value in range [0,255]</returns>
        private static double ToRgb(double n)
        {
            var result = 1.0 * n;

            if (result >= 0)
                if (result <= 255)
                    return result;
                else
                    return 255;
            return 0;
        }

        private void ChannelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.CurrentLineSeries.Points.Count == 2)
                return;

            ApplyFunction();
            _viewModel = new ViewModel();
            DataContext = _viewModel;
        }
    }
}
