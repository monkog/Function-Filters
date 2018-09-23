using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Filters.Helpers;
using Microsoft.Win32;
using Color = System.Drawing.Color;

namespace Filters
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public Bitmap SourceBitmap;
	    readonly ImageBrush _whiteSmokeBitmap;

        public MainWindow()
        {
            InitializeComponent();
            SourceBitmap = null;
            Bitmap grayBitmap = new Bitmap(1, 1);
            grayBitmap.SetPixel(0, 0, Color.WhiteSmoke);
            _whiteSmokeBitmap = BitmapExtensions.CreateImageBrush(grayBitmap);
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
	        var openFileDialog = new OpenFileDialog
	        {
		        Filter = "all image files(*.bmp; *.gif; *.jpeg; *.jpg; *.png)|*.bmp;*.gif; *.jpeg; *.jpg; *.png"
		                 + "|BMP Files (*.bmp)|*.bmp|GIF Files (*.gif)|*.gif|JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png"
	        };

	        // Set filter for file extension and default file extension 

	        // Display OpenFileDialog by calling ShowDialog method 
            bool? result = openFileDialog.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                string fileName = openFileDialog.FileName;
                ImageBrush imageBrush = new ImageBrush();
                BitmapImage bitmapImage = new BitmapImage(new Uri(fileName));
                imageBrush.ImageSource = bitmapImage;
                SourcePhoto.Background = imageBrush;
                SourceBitmap = createBitmapFromBitmapImage(bitmapImage);
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

                switch (extension)
                {
                    case ".bmp":
                        saveToBmp(OutputPhoto, fileName);
                        break;
                    case ".gif":
                        saveToGif(OutputPhoto, fileName);
                        break;
                    case ".jpeg":
                        saveToJpeg(OutputPhoto, fileName);
                        break;
                    case ".png":
                        saveToPng(OutputPhoto, fileName);
                        break;
                }
            }
        }

        private void AdvancedButton_Click(object sender, RoutedEventArgs e)
        {
            if (SourceBitmap != null)
            {
                var advancedFiltersWindow = new AdvancedFilters(this);
                advancedFiltersWindow.Show();
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (SourceBitmap != null)
            {
                PredefinedFilters predefinedFiltersWindow = new PredefinedFilters(this);
                predefinedFiltersWindow.Show();
            }
        }
    }
}
