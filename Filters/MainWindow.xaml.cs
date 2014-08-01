using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;

namespace Filters
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Bitmap m_sourceBitmap;
        ImageBrush m_whiteSmokeBitmap;

        public MainWindow()
        {
            InitializeComponent();
            m_sourceBitmap = null;
            Bitmap grayBitmap = new Bitmap(1, 1);
            grayBitmap.SetPixel(0, 0, Color.WhiteSmoke);
            m_whiteSmokeBitmap = createImageBrushFromBitmap(grayBitmap);
        }

        private void m_openButton_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            openFileDialog.Filter = "all image files(*.bmp; *.gif; *.jpeg; *.jpg; *.png)|*.bmp;*.gif; *.jpeg; *.jpg; *.png"
                + "|BMP Files (*.bmp)|*.bmp|GIF Files (*.gif)|*.gif|JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = openFileDialog.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                string fileName = openFileDialog.FileName;
                ImageBrush imageBrush = new ImageBrush();
                BitmapImage bitmapImage = new BitmapImage(new Uri(fileName));
                imageBrush.ImageSource = bitmapImage;
                m_sourcePhoto.Background = imageBrush;
                m_sourceBitmap = createBitmapFromBitmapImage(bitmapImage);
                m_outputPhoto.Background = m_whiteSmokeBitmap;
            }
        }

        private void m_saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_sourceBitmap == null || m_outputPhoto.Background == m_whiteSmokeBitmap)
                return;

            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "BMP Files (*.bmp)|*.bmp|GIF Files (*.gif)|*.gif|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png";
            Nullable<bool> result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string fileName = saveFileDialog.FileName;
                string extension = Path.GetExtension(fileName);

                switch (extension)
                {
                    case ".bmp":
                        saveToBmp(m_outputPhoto, fileName);
                        break;
                    case ".gif":
                        saveToGif(m_outputPhoto, fileName);
                        break;
                    case ".jpeg":
                        saveToJpeg(m_outputPhoto, fileName);
                        break;
                    case ".png":
                        saveToPng(m_outputPhoto, fileName);
                        break;
                }
            }
        }

        private void m_advancedButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_sourceBitmap != null)
            {
                AdvancedFilters advancedFiltersWindow = new AdvancedFilters();
                advancedFiltersWindow.Owner = this;
                advancedFiltersWindow.Show();
            }
        }

        private void m_filterButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_sourceBitmap != null)
            {
                PredefinedFilters predefinedFiltersWindow = new PredefinedFilters();
                predefinedFiltersWindow.Owner = this;
                predefinedFiltersWindow.Show();
            }
        }
    }
}
