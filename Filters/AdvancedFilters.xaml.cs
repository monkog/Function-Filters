using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Filters.MVVM;
using OxyPlot;
using OxyPlot.Wpf;
using Point = System.Windows.Point;
using Series = OxyPlot.Series.Series;

namespace Filters
{
    /// <summary>
    /// Interaction logic for AdvancedFilters.xaml
    /// </summary>
    public partial class AdvancedFilters : Window
    {
        private ViewModel viewModel;
        private int[] rgbColorsTable;
        private int m_selectedIndex;
        private Bitmap m_outputBitmap;

        public AdvancedFilters()
        {
            InitializeComponent();
        }

        private void AdvancedFilters_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel = new ViewModel();
            DataContext = viewModel;

            var mainWindow = Owner as MainWindow;
            m_outputBitmap = new Bitmap(mainWindow.m_sourceBitmap);

            m_advancedFunction.DefaultTrackerTemplate = null;
            rgbColorsTable = new int[256];
            m_selectedIndex = 0;
            m_channelComboBox.SelectionChanged += m_channelComboBox_SelectionChanged;
            m_advancedFunction.MouseDown += m_advancedFunction_MouseDown;
        }

        void m_advancedFunction_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point point = Mouse.GetPosition(m_advancedFunction);
            viewModel.m_currentLineSeries.Points.Add(new DataPoint(point.X, point.Y));
            viewModel.updateModel();
        }

        private void m_okButton_Click(object sender, RoutedEventArgs e)
        {
            applyFunction();
            Close();
        }

        private void applyFunction()
        {
            var mainWindow = Owner as MainWindow;
            updateColorValuesTables();

            Bitmap outputBitmap = new Bitmap(m_outputBitmap.Width, m_outputBitmap.Height);

            switch (m_selectedIndex)
            {
                case 0:
                    for (int i = 0; i < outputBitmap.Width; i++)
                        for (int j = 0; j < outputBitmap.Height; j++)
                        {
                            Color color = m_outputBitmap.GetPixel(i, j);
                            Color newColor = Color.FromArgb(color.A, rgbColorsTable[color.R]
                                , rgbColorsTable[color.G]
                                , rgbColorsTable[color.B]);
                            outputBitmap.SetPixel(i, j, newColor);
                        }
                    break;
                case 1:
                    for (int i = 0; i < outputBitmap.Width; i++)
                        for (int j = 0; j < outputBitmap.Height; j++)
                        {
                            Color color = m_outputBitmap.GetPixel(i, j);
                            Color newColor = Color.FromArgb(color.A, rgbColorsTable[color.R], color.G, color.B);
                            outputBitmap.SetPixel(i, j, newColor);
                        }
                    break;
                case 2:
                    for (int i = 0; i < outputBitmap.Width; i++)
                        for (int j = 0; j < outputBitmap.Height; j++)
                        {
                            Color color = m_outputBitmap.GetPixel(i, j);
                            Color newColor = Color.FromArgb(color.A, color.R, rgbColorsTable[color.G], color.B);
                            outputBitmap.SetPixel(i, j, newColor);
                        }
                    break;
                case 3:
                    for (int i = 0; i < outputBitmap.Width; i++)
                        for (int j = 0; j < outputBitmap.Height; j++)
                        {
                            Color color = m_outputBitmap.GetPixel(i, j);
                            Color newColor = Color.FromArgb(color.A, color.R, color.G, rgbColorsTable[color.B]);
                            outputBitmap.SetPixel(i, j, newColor);
                        }
                    break;
            }

            m_selectedIndex = m_channelComboBox.SelectedIndex;
            m_outputBitmap = outputBitmap;
            mainWindow.m_outputPhoto.Background = MainWindow.createImageBrushFromBitmap(outputBitmap);
        }

        /// <summary>
        /// Updates the corresponding R, G, B values from the plot.
        /// </summary>
        private void updateColorValuesTables()
        {
            IList<IDataPoint> pointsList = viewModel.getPoints();
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
                    rgbColorsTable[j] = (int)toRGB(a * j + b);
                }
            }
        }

        private void m_cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Returns R,G and B values in range [0,255].
        /// </summary>
        /// <param name="n">R,G,B value in range [0,1]</param>
        /// <returns>R,G,B value in range [0,255]</returns>
        private static double toRGB(double n)
        {
            var result = 1.0 * n;

            if (result >= 0)
                if (result <= 255)
                    return result;
                else
                    return 255;
            return 0;
        }

        private void m_channelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (viewModel.m_currentLineSeries.Points.Count == 2)
                return;

            applyFunction();
            viewModel = new ViewModel();
            DataContext = viewModel;
        }
    }
}
