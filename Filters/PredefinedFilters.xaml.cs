using System.Windows;

namespace Filters
{
	/// <summary>
	/// Interaction logic for PredefinedFilters.xaml
	/// </summary>
	public partial class PredefinedFilters
	{
		private readonly MainWindow _owner;

		public PredefinedFilters(MainWindow owner)
		{
			InitializeComponent();
			Owner = owner;
			_owner = owner;
		}

		/// <summary>
		/// Changes the loaded photo to its negative.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NegateButton_Click(object sender, RoutedEventArgs e)
		{
			_owner.negate();
			Close();
		}

		private void PosterButton_Click(object sender, RoutedEventArgs e)
		{
			_owner.makePoster();
			Close();
		}

		private void ColorMixButton_Click(object sender, RoutedEventArgs e)
		{
			_owner.mixColors();
			Close();
		}

		private void ColorDotsButton_Click(object sender, RoutedEventArgs e)
		{
			_owner.colorDots();
			Close();
		}

		private void Window_Loaded_1(object sender, RoutedEventArgs e)
		{
			ColorDotsCanvas.Background = MainWindow.createImageBrushFromBitmap(Properties.Resources.ColorDots);
			ColorMixCanvas.Background = MainWindow.createImageBrushFromBitmap(Properties.Resources.ColorMix);
			NegateCanvas.Background = MainWindow.createImageBrushFromBitmap(Properties.Resources.Negative);
			PosterCanvas.Background = MainWindow.createImageBrushFromBitmap(Properties.Resources.Poster);
		}
	}
}