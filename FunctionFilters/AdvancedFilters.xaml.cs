using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FunctionFilters.Controls;
using FunctionFilters.Helpers;
using OxyPlot;
using Point = System.Windows.Point;

namespace FunctionFilters
{
    /// <summary>
    /// Interaction logic for AdvancedFilters.xaml
    /// </summary>
    public partial class AdvancedFilters
    {
	    private readonly MainWindow _owner;
	    private PlotViewModel _plotViewModel;
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
            _plotViewModel = new PlotViewModel();
            DataContext = _plotViewModel;
			
            _outputBitmap = new Bitmap(_owner.SourceBitmap);

            AdvancedFunction.DefaultTrackerTemplate = null;
            _rgbColorsTable = new int[256];
            _selectedIndex = 0;
            ChannelComboBox.SelectionChanged += ChannelComboBox_SelectionChanged;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
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

            _selectedIndex = ChannelComboBox.SelectedIndex;
            _outputBitmap = outputBitmap;
	        _owner.OutputPhoto.Background = outputBitmap.CreateImageBrush();
        }

        /// <summary>
        /// Updates the corresponding R, G, B values from the plot.
        /// </summary>
        private void UpdateColorValuesTables()
        {
            IList<IDataPoint> pointsList = _plotViewModel.Points;
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
                    _rgbColorsTable[j] = (a * j + b).ToRgb();
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ChannelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_plotViewModel.Points.Count == 2)
                return;

            ApplyFunction();
            _plotViewModel = new PlotViewModel();
            DataContext = _plotViewModel;
        }
    }
}
