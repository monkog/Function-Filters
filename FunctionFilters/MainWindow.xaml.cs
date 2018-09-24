using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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

		public ICommand OpenCommand => new RelayCommand(OpenFile);

		public ICommand SaveCommand => new RelayCommand(SaveFile);

		public ICommand AdvancedFiltersCommand => new RelayCommand(ChooseAdvancedFilter);

		public ICommand PredefinedFiltersCommand => new RelayCommand(ChoosePredefinedFilter);

		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
			SourceBitmap = null;
			var bitmap = new Bitmap(1, 1);
			bitmap.SetPixel(0, 0, Color.WhiteSmoke);
			_whiteSmokeBitmap = bitmap.CreateImageBrush();
		}

		private void OpenFile()
		{
			var openFileDialog = new OpenFileDialog
			{
				Filter = "all image files(*.bmp; *.gif; *.jpeg; *.jpg; *.png)|*.bmp;*.gif; *.jpeg; *.jpg; *.png"
						 + "|BMP Files (*.bmp)|*.bmp|GIF Files (*.gif)|*.gif|JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png"
			};

			bool? result = openFileDialog.ShowDialog();

			if (result != true) return;

			string fileName = openFileDialog.FileName;
			var imageBrush = new ImageBrush();
			var bitmapImage = new BitmapImage(new Uri(fileName));
			imageBrush.ImageSource = bitmapImage;
			SourcePhoto.Background = imageBrush;
			SourceBitmap = bitmapImage.CreateBitmap();
			OutputPhoto.Background = _whiteSmokeBitmap;
		}

		private void SaveFile()
		{
			if (SourceBitmap == null || OutputPhoto.Background == _whiteSmokeBitmap)
				return;

			var saveFileDialog = new SaveFileDialog
			{
				Filter =
					"BMP Files (*.bmp)|*.bmp|GIF Files (*.gif)|*.gif|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png"
			};
			bool? result = saveFileDialog.ShowDialog();

			if (result != true) return;

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

		private void SaveFile(FrameworkElement visual, string fileName, BitmapEncoder encoder)
		{
			var bitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 96, 96, PixelFormats.Pbgra32);
			bitmap.Render(visual);
			var frame = BitmapFrame.Create(bitmap);
			encoder.Frames.Add(frame);

			try
			{
				using (var stream = File.Create(fileName))
				{
					encoder.Save(stream);
				}

			}
			catch (Exception)
			{
				MessageBox.Show("Cannot access the file. Try again later.");
			}
		}

		private void ChooseAdvancedFilter()
		{
			if (SourceBitmap == null) return;
			var advancedFiltersWindow = new AdvancedFilters(this);
			advancedFiltersWindow.ShowDialog();
		}

		private void ChoosePredefinedFilter()
		{
			if (SourceBitmap == null) return;

			var predefinedFiltersWindow = new PredefinedFilters(this);
			var result = predefinedFiltersWindow.ShowDialog();
			if (result != true) return;

			switch (Filter)
			{
				case ImageFilter.ColorDots:
					OutputPhoto.Background = SourceBitmap.ColorDots();
					break;
				case ImageFilter.ColorMix:
					OutputPhoto.Background = SourceBitmap.MixColors();
					break;
				case ImageFilter.Negate:
					OutputPhoto.Background = SourceBitmap.Negate();
					break;
				case ImageFilter.Poster:
					OutputPhoto.Background = SourceBitmap.ConvertToPoster();
					break;
			}
		}
	}
}