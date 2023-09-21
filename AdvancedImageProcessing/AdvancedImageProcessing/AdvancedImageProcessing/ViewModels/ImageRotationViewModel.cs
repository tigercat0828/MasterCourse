using Avalonia.Media.Imaging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedImageProcessing.ViewModels {
    public class ImageRotationViewModel : ViewModelBase {
        private string _description = "Test";
        public string Description { 
            get=> _description; 
            set=> this.RaiseAndSetIfChanged(ref _description, value);
        }
        private Bitmap _source = null;
        public Bitmap Source => _source;
        private Bitmap _output = null;
        public Bitmap Output {
            get => _output; 
            set => this.RaiseAndSetIfChanged(ref _output, value);
        }
        public void OpenFile() {
            
        }
        public void SaveFile() {

        }
        public void RightRotate() {

        }
        public void LeftRotate() {

        }
    }
}
