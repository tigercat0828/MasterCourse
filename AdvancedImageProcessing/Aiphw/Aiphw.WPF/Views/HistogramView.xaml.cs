using Aiphw.WPF.Models;
using Microsoft.Win32;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace Aiphw.WPF.Views {
    /// <summary>
    /// HistogramView.xaml 的互動邏輯
    /// </summary>
    public partial class HistogramView : UserControl {
        RawImage _outputRaw;
        RawImage _inputRaw;
        public HistogramView() {
            InitializeComponent();
        }
        private void OpenFileBtn_Click(object sender, RoutedEventArgs e) {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.bmp; *.ppm)|*.jpg; *.jpeg; *.png; *.bmp; *.ppm";
            dialog.Title = "Open Image";
            if (dialog.ShowDialog() == true) {

                RawImage loadRaw = new RawImage(dialog.FileName);

                _inputRaw = new RawImage(loadRaw);
                _outputRaw = ImageProcessing.GrayScale(loadRaw);
                Utility.UpdateImageBox(OutputImgBox, _outputRaw.ToBitmap());
                Utility.UpdateImageBox(InputImgBox, _inputRaw.ToBitmap());
            }
        }
    }
}
