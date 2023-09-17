using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;
using Image = System.Windows.Controls.Image;
// rotate image 
namespace Homework1 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            SaveFileBtn.IsEnabled = false;
            LeftRotateBtn.IsEnabled = false;
            RightRotateBtn.IsEnabled = false;
        }

        Bitmap _processBmp;
        Bitmap _sourceBmp;

        private void OpenFileBtn_Click(object sender, RoutedEventArgs e) {
     
            var dialog = new OpenFileDialog();
            dialog.Filter = "Supported Image File|*.jpg; *.png; *.ppm; *.bmp";
            if(dialog.ShowDialog() == true) {
                
                Bitmap srcImg = new Bitmap(dialog.FileName);
                _sourceBmp = new Bitmap(srcImg);
                _processBmp = new Bitmap(srcImg);
                
                SourceImgBox.Source = BitmapToImageSource(_sourceBmp);
                ProcessedImgBox.Source = BitmapToImageSource(_processBmp);

                SaveFileBtn.IsEnabled = true;
                LeftRotateBtn.IsEnabled = true;
                RightRotateBtn.IsEnabled = true;

            }
        }
        
        private void SaveFileBtn_Click(object sender, RoutedEventArgs e) {
        }

        private void RightRotateBtn_Click(object sender, RoutedEventArgs e) {
            RightRotate();
            UpdateImageBox(ProcessedImgBox, _processBmp);
        }

        private void LeftRotateBtn_Click(object sender, RoutedEventArgs e) {
            LeftRotate();
            UpdateImageBox(ProcessedImgBox, _processBmp);
        }
        private void RightRotate() {
            int width = _processBmp.Width;
            int height = _processBmp.Height;

            
            int newWidth = width;
            int newHeight = height; 

            Bitmap processing = new(newHeight, newWidth); 

            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    Color pixel = _processBmp.GetPixel(i, j);
                    
                    int x = newHeight - 1 - j; 
                    int y = i;

                    processing.SetPixel(x, y, pixel);
                }
            }

            _processBmp = processing;
        }


        private void LeftRotate() {
            int width = _processBmp.Width;
            int height = _processBmp.Height;


            int newWidth = width;
            int newHeight = height;

            Bitmap processing = new(newHeight, newWidth);

            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    Color pixel = _processBmp.GetPixel(i, j);

                    int x = j;
                    int y = newWidth - 1 - i;

                    processing.SetPixel(x, y, pixel);
                }
            }

            _processBmp = processing;
        }

        private BitmapImage BitmapToImageSource(Bitmap bitmap) {
            using (MemoryStream memory = new MemoryStream()) {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }
        private void UpdateImageBox(Image imgBox, Bitmap bitmap) {

            imgBox.Source = BitmapToImageSource(bitmap);
        }
    }
}
