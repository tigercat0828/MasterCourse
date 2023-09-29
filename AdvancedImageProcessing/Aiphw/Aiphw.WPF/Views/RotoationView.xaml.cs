using Aiphw.WPF.Models;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace Aiphw.WPF.Views {
    /// <summary>
    /// RotoationView.xaml 的互動邏輯
    /// </summary>
    public partial class RotationView : UserControl {
        public RotationView() {

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

                Utility.UpdateImageBox(OutputImgBox, _outputRaw.ToBitmap());
                Utility.UpdateImageBox(InputImgBox, _inputRaw.ToBitmap());

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
            Utility.UpdateImageBox(OutputImgBox, _outputRaw.ToBitmap());
        }

        private void LeftRotateBtn_Click(object sender, RoutedEventArgs e) {
            _outputRaw = ImageProcessing.LeftRotate(_outputRaw);
            Utility.UpdateImageBox(OutputImgBox, _outputRaw.ToBitmap());
        }


    }
}
