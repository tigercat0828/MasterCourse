using Aiphw.WPF.Models;
using Microsoft.Win32;
using ScottPlot;
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
            }
        }
        public void SetImageInfoTextBlock() {
            ImageInfoText.Text = $"{_outputRaw.Width} x {_outputRaw.Height} = {_outputRaw.Width * _outputRaw.Height}";
        }
        private void DrawHistogram() {



            // add random data to the histogram
            byte[] byteArray = _outputRaw.Pixels;

            double[] grayArray = new double[byteArray.Length / 4]; // Create a new array to store every fourth element

            for (int i = 0; i < grayArray.Length; i++) {
                grayArray[i] = byteArray[i * 4];
            }

            // create a histogram with a fixed number of bins
            ScottPlot.Statistics.Histogram hist = new(min: 0, max: 255, binCount: 128);
            Plot plot = Histogram.Plot;

            plot.Clear();
            hist.AddRange(grayArray);
            plot.AddBar(values: hist.Counts, positions: hist.Bins);
            plot.XAxis.Label("intensity");
            plot.YAxis.Label("frequency");

            plot.SetAxisLimits(yMin: 0);

            Histogram.Render();
        }
    }
}
