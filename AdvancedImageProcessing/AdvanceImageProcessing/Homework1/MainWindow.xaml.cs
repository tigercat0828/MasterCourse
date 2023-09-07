using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Homework1 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        string _fileName;
        BitmapImage _processedImage;
        BitmapImage _sourceIamge;

        private void OpenFileBtn_Click(object sender, RoutedEventArgs e) {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Supported Image File|*.jpg; *.png; *.ppm; *.bmp";
            if(dialog.ShowDialog() == true) {

                _fileName = dialog.FileName;
                
                _processedImage = new BitmapImage(new Uri(dialog.FileName));
                _sourceIamge = new BitmapImage(new Uri(dialog.FileName));

                SourceImg.Source = _sourceIamge;
                ProcessedImg.Source = _processedImage;
            }
        }

        private void SaveFileBtn_Click(object sender, RoutedEventArgs e) {

        }

        private void RightRotateBtn_Click(object sender, RoutedEventArgs e) {

        }

        private void LeftRotateBtn_Click(object sender, RoutedEventArgs e) {

        }
        private void RightRoate() {

        }
        private void LeftRotate() {

        }
    }
}
