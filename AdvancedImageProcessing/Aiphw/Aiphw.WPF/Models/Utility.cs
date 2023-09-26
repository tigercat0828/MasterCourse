using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace Aiphw.WPF.Models
{
    public static class Utility
    {
        private static BitmapImage BitmapToImageSource(Bitmap bitmap) {
            using (MemoryStream memory = new MemoryStream()) {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }
        public static void UpdateImageBox(Image imgBox, Bitmap bitmap) {
            imgBox.Source = BitmapToImageSource(bitmap);
        }
        public static void DrawHistogram(string XasixName, string YaxisName, ) {

        }
    }
}
