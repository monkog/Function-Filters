using System.Windows;
using Filters.Helpers;

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
			ColorDotsCanvas.Background = Properties.Resources.ColorDots.CreateImageBrush();
			ColorMixCanvas.Background = Properties.Resources.ColorMix.CreateImageBrush();
			NegateCanvas.Background = Properties.Resources.Negative.CreateImageBrush();
			PosterCanvas.Background = Properties.Resources.Poster.CreateImageBrush();
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
	}
}