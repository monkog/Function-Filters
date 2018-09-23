using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Filters;
using FunctionFilters.Helpers;
using FunctionFilters.ImageManipulators;
using Microsoft.Win32;
using Color = System.Drawing.Color;

namespace FunctionFilters
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public Bitmap SourceBitmap;
		readonly ImageBrush _whiteSmokeBitmap;

		public ImageFilter Filter { get; set; }

		public MainWindow()
		{
			InitializeComponent();
			SourceBitmap = null;
			var bitmap = new Bitmap(1, 1);
			bitmap.SetPixel(0, 0, Color.WhiteSmoke);
			_whiteSmokeBitmap = bitmap.CreateImageBrush();
		}

		private void OpenButton_Click(object sender, RoutedEventArgs e)
		{
			var openFileDialog = new OpenFileDialog
			{
				Filter = "all image files(*.bmp; *.gif; *.jpeg; *.jpg; *.png)|*.bmp;*.gif; *.jpeg; *.jpg; *.png"
						 + "|BMP Files (*.bmp)|*.bmp|GIF Files (*.gif)|*.gif|JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png"
			};

			bool? result = openFileDialog.ShowDialog();

			if (result == true)
			{
				string fileName = openFileDialog.FileName;
				ImageBrush imageBrush = new ImageBrush();
				BitmapImage bitmapImage = new BitmapImage(new Uri(fileName));
				imageBrush.ImageSource = bitmapImage;
				SourcePhoto.Background = imageBrush;
				SourceBitmap = bitmapImage.CreateBitmap();
				OutputPhoto.Background = _whiteSmokeBitmap;
			}
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			if (SourceBitmap == null || OutputPhoto.Background == _whiteSmokeBitmap)
				return;

			var saveFileDialog = new SaveFileDialog
			{
				Filter =
					"BMP Files (*.bmp)|*.bmp|GIF Files (*.gif)|*.gif|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png"
			};
			bool? result = saveFileDialog.ShowDialog();

			if (result == true)
			{
				string fileName = saveFileDialog.FileName;
				string extension = Path.GetExtension(fileName);
				BitmapEncoder encoder;

				switch (extension)
				{
					case ".bmp":
						encoder = new BmpBitmapEncoder();
						break;
					case ".gif":
						encoder = new GifBitmapEncoder();
						break;
					case ".jpeg":
						encoder = new JpegBitmapEncoder();
						break;
					case ".png":
						encoder = new PngBitmapEncoder();
						break;
					default:
						throw new NotSupportedException($"The {extension} is not supported.");
				}

				SaveFile(OutputPhoto, fileName, encoder);
			}
		}

		/// <summary>
		/// Saves image with a chosen extension.
		/// </summary>
		/// <param name="visual">Image to save</param>
		/// <param name="fileName">Output file name</param>
		/// <param name="encoder">Matching encoder</param>
		void SaveFile(FrameworkElement visual, string fileName, BitmapEncoder encoder)
		{
			RenderTargetBitmap bitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 96, 96, PixelFormats.Pbgra32);
			bitmap.Render(visual);
			BitmapFrame frame = BitmapFrame.Create(bitmap);
			encoder.Frames.Add(frame);

			using (var stream = File.Create(fileName))
			{
				encoder.Save(stream);
			}
		}

		private void AdvancedButton_Click(object sender, RoutedEventArgs e)
		{
			if (SourceBitmap != null)
			{
				var advancedFiltersWindow = new AdvancedFilters(this);
				advancedFiltersWindow.ShowDialog();
			}
		}

		private void FilterButton_Click(object sender, RoutedEventArgs e)
		{
			if (SourceBitmap == null) return;

			PredefinedFilters predefinedFiltersWindow = new PredefinedFilters(this);
			var result = predefinedFiltersWindow.ShowDialog();
			if (result == true)
			{
				switch (Filter)
				{
					case ImageFilter.ColorDots:
						ColorDots();
						break;
					case ImageFilter.ColorMix:
						MixColors();
						break;
					case ImageFilter.Negate:
						OutputPhoto.Background = SourceBitmap.Negate();
						break;
					case ImageFilter.Poster:
						MakePoster();
						break;
				}
			}
		}
	}
}
