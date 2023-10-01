using Aiphw.WPF.Models;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Aiphw.WPF.Views {
    /// <summary>
    /// ConvoluitionView.xaml 的互動邏輯
    /// </summary>
    /// 
  
    public partial class ConvolutionView : UserControl {

        TextBox[,] MaskCell = new TextBox[5,5];
        public ConvolutionView() {
            InitializeComponent();
            BuildMaskControl();
            SaveFileBtn.IsEnabled = false;
            SmoothBtn.IsEnabled = false;
            EdgeDetectBtn.IsEnabled = false;
        }
        private void OpenFileBtn_Click(object sender, RoutedEventArgs e) {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.bmp; *.ppm)|*.jpg; *.jpeg; *.png; *.bmp; *.ppm";
            dialog.Title = "Open Image";
            if (dialog.ShowDialog() == true) {

                RawImage loadRaw = new RawImage(dialog.FileName);



                SaveFileBtn.IsEnabled = true;
                SmoothBtn.IsEnabled = true;
                EdgeDetectBtn.IsEnabled = true;
            }
        }
        private void SaveFileBtn_Click(object sender, RoutedEventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JPEG Image (*.jpg)|*.jpg|PNG Image (*.png)|*.png|Bitmap Image (*.bmp)|*.bmp|PPM Image (*.ppm)|*.ppm";
            saveFileDialog.Title = "Save Image";

            if (saveFileDialog.ShowDialog() == true) {
                string filename = saveFileDialog.FileName;
            }
        }
        private void BuildMaskControl() {
            // Loop to create and add 25 TextBox controls to the MaskGrid
            
            for (int i = 0; i < 5; i++) {
                for (int j = 0; j < 5; j++) {
                    // Create a new TextBox
                    TextBox textBox = new TextBox();
                    textBox.Name = $"m{i}{j}";
                    textBox.Text = "0";
                    textBox.Height = 40;
                    textBox.FontSize = 20;
                    textBox.TextAlignment = TextAlignment.Center;
                    textBox.VerticalContentAlignment = VerticalAlignment.Center;
                    Grid.SetRow(textBox, i); 
                    Grid.SetColumn(textBox, j); 
                    MaskCell[i, j] = textBox;
                    MaskGrid.Children.Add(textBox);
                }
            }
        }
        private void ResetAll0_Click(object sender, RoutedEventArgs e) {
            for (int i = 0; i < 5; i++) {
                for (int j = 0; j < 5; j++) {
                    MaskCell[i,j].Text = "0";
                }
            }
        }
        private void ResetAll1_Click(object sender, RoutedEventArgs e) {
            for (int i = 0; i < 5; i++) {
                for (int j = 0; j < 5; j++) {
                    MaskCell[i, j].Text = "1";
                }
            }
        }

    }
}
