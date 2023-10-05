using Aiphw.WPF.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace Aiphw.WPF.Views {

    public partial class ConvolutionView : UserControl {

        enum MaskSize {
            m3x3=3, m5x5=5
        }
        enum MaskType {
            Custom, Smooth, SobelX, SobelY
        }
        TextBox[,] _MaskCellTextBox = new TextBox[5, 5];
        MaskSize _MaskSize = MaskSize.m3x3;
        MaskType _MaskType = MaskType.Custom;
        List<float> _MaskKernel = new();

        RawImage inputRaw;
        public ConvolutionView() {
            InitializeComponent();
            BuildMaskControl();
            SaveFileBtn.IsEnabled = false;
            SmoothBtn.IsEnabled = false;
            EdgeDetectBtn.IsEnabled = false;
        }

        private void OpenFileBtn_Click(object sender, RoutedEventArgs e) {

            OpenFileDialog dialog = new() {
                Filter = "Image Files (*.jpg; *.jpeg; *.png; *.bmp; *.ppm)|*.jpg; *.jpeg; *.png; *.bmp; *.ppm",
                Title = "Open Image"
            };
            if (dialog.ShowDialog() == true) {

                RawImage loadRaw = new(dialog.FileName);
                inputRaw = new RawImage(loadRaw);
                Utility.UpdateImageBox(InputImgBox, inputRaw.ToBitmap());
                SaveFileBtn.IsEnabled = true;
                SmoothBtn.IsEnabled = true;
                EdgeDetectBtn.IsEnabled = true;
            }
        }
        private void SaveFileBtn_Click(object sender, RoutedEventArgs e) {
            SaveFileDialog saveFileDialog = new() {
                Filter = "Image Files (*.jpg; *.jpeg; *.png; *.bmp; *.ppm)|*.jpg; *.jpeg; *.png; *.bmp; *.ppm",
                Title = "Save Image"
            };

            if (saveFileDialog.ShowDialog() == true) {
                string filename = saveFileDialog.FileName;
            }
        }
        private void LoadMask(MaskType type) {
            _MaskKernel.Clear();
            switch (type) {
                case MaskType.Custom: 
                    int start = _MaskSize == MaskSize.m5x5 ? 0 : 1;
                    int end = _MaskSize == MaskSize.m3x3 ? 5 : 4;
                    for (int i = start; i < end; i++) {
                        for (int j = start; j < end; j++) {
                            _MaskKernel.Add(float.Parse(_MaskCellTextBox[i, j].Text));
                        }
                    }

                    break;
                

                case MaskType.Smooth: 
                    _MaskSize = MaskSize.m5x5;
                    _MaskKernel.AddRange(new float[] {
                        2,  4,  5,  4,  2,
                        4,  9,  12, 9,  4,
                        5,  12, 15, 12, 5,
                        4,  9,  12, 9,  4,
                        2,  4,  5,  4,  2
                        }
                    );
                    for (int i = 0; i < _MaskKernel.Count; i++) _MaskKernel[i] /= 159;
                    break;

                case MaskType.SobelX:
                    _MaskSize = MaskSize.m3x3;
                    _MaskKernel.AddRange(new float[] {
                        -1, 0,  1,
                        -2, 0,  2,
                        -1, 0,  1,
                        }
                    );
                    break;
                case MaskType.SobelY:
                    _MaskSize = MaskSize.m3x3;
                    _MaskKernel.AddRange(new float[] {
                        -1, -2, -1,
                         0,  0,  0,
                         1,  2,  1,
                        }
                    );
                    break;
            }

        }
        
        private void ProcessBtn_Click(object sender, RoutedEventArgs e) {

        }
        private void BuildMaskControl() {
            // Loop to create and add 25 TextBox controls to the MaskGrid
            for (int i = 0; i < 5; i++) {
                for (int j = 0; j < 5; j++) {
                    TextBox textBox = new TextBox();
                    textBox.Name = $"m{i}{j}";
                    textBox.Text = "0";
                    textBox.Height = 40;
                    textBox.FontSize = 20;
                    textBox.TextAlignment = TextAlignment.Center;
                    textBox.VerticalContentAlignment = VerticalAlignment.Center;
                    textBox.GotFocus += (sender, e) => {
                        TextBox textBox = (TextBox)sender;
                        textBox.SelectAll();
                    };

                    Grid.SetRow(textBox, i);
                    Grid.SetColumn(textBox, j);
                    _MaskCellTextBox[i, j] = textBox;
                    MaskGrid.Children.Add(textBox);
                }
            }
        }
        private void Size3x3Btn_Click(object sender, RoutedEventArgs e) {
            SetMaskSize(MaskSize.m3x3);
        }
        private void Size5x5Btn_Click(object sender, RoutedEventArgs e) {
            SetMaskSize(MaskSize.m5x5);
        }
        private void ResetAllZeroBtn_Click(object sender, RoutedEventArgs e) {
            SetMaskCellTextBoxAll("0");
        }
        private void ResetAllOneBtn_Click(object sender, RoutedEventArgs e) {
            SetMaskCellTextBoxAll("1");
        }
        private void SetMaskCellTextBoxAll(string text) {
            int start = (_MaskSize == MaskSize.m5x5) ? 0 : 1;
            int end = (_MaskSize == MaskSize.m5x5) ? 5 : 4;
            for (int i = start; i < end; i++) {
                for (int j = start; j < end; j++) {
                    _MaskCellTextBox[i, j].Text = text;
                }
            }
        }
        private void SetMaskSize(MaskSize size) {
            _MaskSize = size;
            bool isEnabled = (size == MaskSize.m3x3) ? false : true;
            string text = (size == MaskSize.m3x3) ? string.Empty : "1";
            for (int i = 0; i < 5; i++) {
                _MaskCellTextBox[0, i].IsEnabled = isEnabled;
                _MaskCellTextBox[0, i].Text = text;
                _MaskCellTextBox[4, i].IsEnabled = isEnabled;
                _MaskCellTextBox[4, i].Text = text;
            }
            for (int i = 1; i < 4; i++) {
                _MaskCellTextBox[i, 0].IsEnabled = isEnabled;
                _MaskCellTextBox[i, 0].Text = text;
                _MaskCellTextBox[i, 4].IsEnabled = isEnabled;
                _MaskCellTextBox[i, 4].Text = text;
            }
        }
        [DllImport("Kernel32")]
        public static extern void AllocConsole();

        [DllImport("Kernel32")]
        public static extern void FreeConsole();
        private void DebugBtn_Click(object sender, RoutedEventArgs e) {
            AllocConsole();
            Console.WriteLine("test");
            RawImage image = new RawImage(5,5);
            ImageProcessing.PrintPixelImage(image);
        }
    }
}
