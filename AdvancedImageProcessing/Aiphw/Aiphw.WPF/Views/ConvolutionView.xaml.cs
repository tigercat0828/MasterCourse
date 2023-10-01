using Aiphw.WPF.Models;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Aiphw.WPF.Views {

    public partial class ConvolutionView : UserControl {

        enum Size {
            m3x3, m5x5
        }

        TextBox[,] _MaskCellTextBox = new TextBox[5, 5];
        Size _MaskSize = Size.m3x3;
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
        private void LoadMask() {
            _MaskKernel.Clear();
            int start = (_MaskSize == Size.m5x5) ? 0 : 1;
            int end = (_MaskSize == Size.m3x3) ? 5 : 4;
            for (int i = start; i < end; i++) {
                for (int j = start; j < end; j++) {
                    _MaskKernel.Add(float.Parse(_MaskCellTextBox[i, j].Text));
                }
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
            SetMaskSize(Size.m3x3);
        }
        private void Size5x5Btn_Click(object sender, RoutedEventArgs e) {
            SetMaskSize(Size.m5x5);
        }
        private void ResetAllZeroBtn_Click(object sender, RoutedEventArgs e) {
            SetMaskCellTextBoxAll("0");
        }
        private void ResetAllOneBtn_Click(object sender, RoutedEventArgs e) {
            SetMaskCellTextBoxAll("1");
        }
        private void SetMaskCellTextBoxAll(string text) {
            int start = (_MaskSize == Size.m5x5) ? 0 : 1;
            int end = (_MaskSize == Size.m5x5) ? 5 : 4;
            for (int i = start; i < end; i++) {
                for (int j = start; j < end; j++) {
                    _MaskCellTextBox[i, j].Text = text;
                }
            }
        }
        private void SetMaskSize(Size size) {
            _MaskSize = size;
            bool isEnabled = (size == Size.m3x3) ? false : true;
            string text = (size == Size.m3x3) ? string.Empty : "1";
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

    }
}
