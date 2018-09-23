using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using FunctionFilters.Helpers;

namespace FunctionFilters
{
	/// <summary>
	/// Interaction logic for PredefinedFilters.xaml
	/// </summary>
	[ExcludeFromCodeCoverage]
	public partial class PredefinedFilters
	{
		private readonly FunctionFilters.MainWindow _owner;

		public ICommand FilterCommand => new RelayCommand<ImageFilter>(ApplyFilter);

		public PredefinedFilters(FunctionFilters.MainWindow owner)
		{
			InitializeComponent();
			Owner = owner;
			_owner = owner;
			DataContext = this;

			ColorDotsCanvas.Background = Properties.Resources.ColorDots.CreateImageBrush();
			ColorMixCanvas.Background = Properties.Resources.ColorMix.CreateImageBrush();
			NegateCanvas.Background = Properties.Resources.Negative.CreateImageBrush();
			PosterCanvas.Background = Properties.Resources.Poster.CreateImageBrush();
		}

		private void ApplyFilter(ImageFilter filter)
		{
			_owner.Filter = filter;
			DialogResult = true;
			Close();
		}
	}
}