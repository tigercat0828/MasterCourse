using Aiphw.WPF.Models;
using Microsoft.Win32;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace Aiphw.WPF.Views {
    /// <summary>
    /// NoiseView.xaml 的互動邏輯
    /// </summary>
    public partial class NoiseView : UserControl {
        public NoiseView() {
            InitializeComponent();
            GaussianBtn.IsEnabled = false;
            PepperBtn.IsEnabled = false;
            SaveFileBtn.IsEnabled = false;
        }
        RawImage loadRaw;

        RawImage noiseRaw;

        RawImage outputRaw;
        private void OpenFileBtn_Click(object sender, RoutedEventArgs e) {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.bmp; *.ppm)|*.jpg; *.jpeg; *.png; *.bmp; *.ppm";
            dialog.Title = "Open Image";
            if (dialog.ShowDialog() == true) {
                loadRaw = new RawImage(dialog.FileName);

                GaussianBtn.IsEnabled = true;
                PepperBtn.IsEnabled = true;
                SaveFileBtn.IsEnabled = true;
                ResetControls();
                UpdateInputControl();

            }
        }
        private void ResetControls() {

            OutputImgBox.Source = null;
            NoiseImgBox.Source = null;
            OutputHistogram.Plot.Clear();
            NoiseHistogram.Plot.Clear();
            OutputHistogram.Render();
            NoiseHistogram.Render();
        }


        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {

        }

        private void GaussianBtn_Click(object sender, RoutedEventArgs e) {

            outputRaw = ImageProcessing.GaussianNoise(loadRaw, out noiseRaw, (int)NoiseSlider.Value);
            UpdateAllControls();
        }

        private void PepperBtn_Click(object sender, RoutedEventArgs e) {
            outputRaw = ImageProcessing.SaltPepperNoise(loadRaw, out noiseRaw, (int)NoiseSlider.Value);
            UpdateAllControls();
        }
        private void UpdateInputControl() {
            Utility.UpdateImageBox(InputImgBox, loadRaw.ToBitmap());
            Utility.SetSingleChannelHistogram(InputHistogram.Plot, ImageProcessing.GrayScale(loadRaw), 0, Color.FromArgb(128, 128, 128), "Gray");
            InputHistogram.Render();
        }
        private void UpdateAllControls() {
            Utility.UpdateImageBox(InputImgBox, loadRaw.ToBitmap());
            Utility.SetSingleChannelHistogram(InputHistogram.Plot, ImageProcessing.GrayScale(loadRaw), 0, Color.FromArgb(128, 128, 128), "Gray");
            InputHistogram.Render();
            Utility.UpdateImageBox(NoiseImgBox, noiseRaw.ToBitmap());
            Utility.SetSingleChannelHistogram(NoiseHistogram.Plot, ImageProcessing.GrayScale(noiseRaw), 0, Color.FromArgb(128, 128, 128), "Gray");
            NoiseHistogram.Render();
            Utility.UpdateImageBox(OutputImgBox, outputRaw.ToBitmap());
            Utility.SetSingleChannelHistogram(OutputHistogram.Plot, ImageProcessing.GrayScale(outputRaw), 0, Color.FromArgb(128, 128, 128), "Gray");
            OutputHistogram.Render();
        }

        private void SaveFileBtn_Click(object sender, RoutedEventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JPEG Image (*.jpg)|*.jpg|PNG Image (*.png)|*.png|Bitmap Image (*.bmp)|*.bmp|PPM Image (*.ppm)|*.ppm";
            saveFileDialog.Title = "Save Image";

            if (saveFileDialog.ShowDialog() == true) {
                string filename = saveFileDialog.FileName;
                outputRaw.SaveFile(filename);
            }
        }
    }
}
