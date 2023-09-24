using Microsoft.Win32;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace Aiphw.WPF {
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

        RawImage _outputRaw;
        RawImage _inputRaw;

        private void OpenFileBtn_Click(object sender, RoutedEventArgs e) {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.bmp; *.ppm)|*.jpg; *.jpeg; *.png; *.bmp; *.ppm";
            dialog.Title = "Open Image";
            if (dialog.ShowDialog() == true) {

                RawImage loadRaw = new RawImage(dialog.FileName);

                _inputRaw = new RawImage(loadRaw);
                _outputRaw = new RawImage(loadRaw);

                UpdateImageBox(OutputImgBox, _outputRaw.ToBitmap());
                UpdateImageBox(InputImgBox, _inputRaw.ToBitmap());

                SaveFileBtn.IsEnabled = true;
                LeftRotateBtn.IsEnabled = true;
                RightRotateBtn.IsEnabled = true;
            }
        }

        private void SaveFileBtn_Click(object sender, RoutedEventArgs e) {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JPEG Image (*.jpg)|*.jpg|PNG Image (*.png)|*.png|Bitmap Image (*.bmp)|*.bmp|PPM Image (*.ppm)|*.ppm";
            saveFileDialog.Title = "Save Image";

            if (saveFileDialog.ShowDialog() == true) {
                string filename = saveFileDialog.FileName;
                _outputRaw.SaveFile(filename);
            }
        }

        private void RightRotateBtn_Click(object sender, RoutedEventArgs e) {
            _outputRaw = ImageProcessing.RightRotate(_outputRaw);
            UpdateImageBox(OutputImgBox, _outputRaw.ToBitmap());
        }

        private void LeftRotateBtn_Click(object sender, RoutedEventArgs e) {
            _outputRaw = ImageProcessing.LeftRotate(_outputRaw);
            UpdateImageBox(OutputImgBox, _outputRaw.ToBitmap());
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
