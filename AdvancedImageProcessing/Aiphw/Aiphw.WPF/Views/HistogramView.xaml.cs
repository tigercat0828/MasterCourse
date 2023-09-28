using Aiphw.WPF.Models;
using Microsoft.Win32;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using ScottPlot;
using System;

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

                DrawHistogram();
            }
        }
        private void DrawHistogram() {

            // create a histogram with a fixed number of bins
            ScottPlot.Statistics.Histogram hist = new(min: 0, max: 255, binCount: 128);

            // add random data to the histogram
            byte[] byteArray = _outputRaw.Pixels;


            double[] grayArray = new double[byteArray.Length / 4]; // Create a new array to store every fourth element
            Console.WriteLine(_outputRaw.Pixels.Length);
            Console.WriteLine(grayArray.Length);

            for (int i = 0; i < grayArray.Length; i++) {
                grayArray[i] = byteArray[i * 4];
            }

            Plot plot = Histogram.Plot;
            plot.Clear();
            hist.AddRange(grayArray);
            plot.AddBar(values: hist.Counts, positions: hist.Bins);
            plot.XAxis.Label("gray value");
            plot.SetAxisLimits(yMin: 0);

           Histogram.Render();
        }
    }
}
