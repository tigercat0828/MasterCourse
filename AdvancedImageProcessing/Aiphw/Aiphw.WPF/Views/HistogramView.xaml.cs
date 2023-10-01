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
        public int SliderValue = 50;
        public HistogramView() {
            InitializeComponent();
            SaveFileBtn.IsEnabled = false;
        }
        private void OpenFileBtn_Click(object sender, RoutedEventArgs e) {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.bmp; *.ppm)|*.jpg; *.jpeg; *.png; *.bmp; *.ppm";
            dialog.Title = "Open Image";
            if (dialog.ShowDialog() == true) {

                RawImage loadRaw = new RawImage(dialog.FileName);

                _outputRaw = ImageProcessing.GrayScale(loadRaw);
                Utility.UpdateImageBox(OutputImgBox, _outputRaw.ToBitmap());

                SetImageInfoTextBlock();
                DrawHistogram();

                SaveFileBtn.IsEnabled = true;
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
        public void SetImageInfoTextBlock() {
            ImageInfoText.Text = $"{_outputRaw.Width} x {_outputRaw.Height} = {_outputRaw.Width * _outputRaw.Height}";
        }
        private void DrawHistogram() {

            Utility.SetSingleChannelHistogram(HistoGraph.Plot, _outputRaw, channel: 0, Color.FromArgb(128, 128, 128), "gray");

            HistoGraph.Render();
        }


    }
}
