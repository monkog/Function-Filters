using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Filters
{
    /// <summary>
    /// Interaction logic for PredefinedFilters.xaml
    /// </summary>
    public partial class PredefinedFilters : Window
    {
        public PredefinedFilters()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Changes the loaded photo to its negative.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_negateButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow owner = Owner as MainWindow;
            owner.negate();
            Close();
        }

        private void m_posterButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow owner = Owner as MainWindow;
            owner.makePoster();
            Close();
        }

        private void m_colorMixButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow owner = Owner as MainWindow;
            owner.mixColors();
            Close();
        }

        private void m_colorDotsButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow owner = Owner as MainWindow;
            owner.colorDots();
            Close();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            m_colorDotsCanvas.Background = MainWindow.createImageBrushFromBitmap(Filters.Properties.Resources.chips);
            m_colorMixCanvas.Background = MainWindow.createImageBrushFromBitmap(Filters.Properties.Resources.saturation);
            m_negateCanvas.Background = MainWindow.createImageBrushFromBitmap(Filters.Properties.Resources.negative);
            m_posterCanvas.Background = MainWindow.createImageBrushFromBitmap(Filters.Properties.Resources.poster);
        }
    }
}
