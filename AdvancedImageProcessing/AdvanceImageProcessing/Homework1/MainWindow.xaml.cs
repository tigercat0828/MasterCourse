using Microsoft.Win32;
using System.Drawing;

using System.IO;
using System.Windows;
using System.Windows.Controls;
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
        }

        string _fileName;
        Bitmap _processBmp;
        Bitmap _sourceBmp;

        private void OpenFileBtn_Click(object sender, RoutedEventArgs e) {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Supported Image File|*.jpg; *.png; *.ppm; *.bmp";
            if(dialog.ShowDialog() == true) {
                
                Bitmap srcImg = new Bitmap(dialog.FileName);

                _processBmp = new Bitmap(srcImg);
                _sourceBmp = new Bitmap(srcImg);

                SourceImgBox.Source = BitmapToImageSource(srcImg);
                ProcessedImgBox.Source = BitmapToImageSource(srcImg);
            }
        }
        
        private void SaveFileBtn_Click(object sender, RoutedEventArgs e) {
        }

        private void RightRotateBtn_Click(object sender, RoutedEventArgs e) {
            RightRoate();
        }

        private void LeftRotateBtn_Click(object sender, RoutedEventArgs e) {
            LeftRotate();
            UpdateImageBox(ProcessedImgBox, _processBmp);
        }
        private void RightRoate() {

        }
        private void LeftRotate() {
            for (int i = 0; i < _processBmp.Width / 2; i++) {
                for (int j = 0; j < _processBmp.Height / 2; j++) {
                    _processBmp.SetPixel(i, j, Color.AliceBlue);
                }
            }
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
