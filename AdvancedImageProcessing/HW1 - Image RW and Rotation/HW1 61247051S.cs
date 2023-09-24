
public static class ImageProcessing {

    public static RawImage RightRotate(RawImage input) {
        int width = input.Width;
        int height = input.Height;

        int newWidth = width;
        int newHeight = height;

        RawImage processing = new(newHeight, newWidth);

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                byte[] pixel = input.GetPixel(i, j);

                int x = newHeight - 1 - j;
                int y = i;

                processing.SetPixel(x, y, pixel);
            }
        }
        processing.FinishEdit();
        return processing;
    }
    public static RawImage LeftRotate(RawImage input) {
        int width = input.Width;
        int height = input.Height;

        int newWidth = width;
        int newHeight = height;

        RawImage processing = new(newHeight, newWidth);

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                byte[] pixel = input.GetPixel(i, j);

                int x = j;
                int y = newWidth - 1 - i;

                processing.SetPixel(x, y, pixel);
            }
        }
        processing.FinishEdit();
        return processing;
    }
}
public class RawImage : ICloneable, IDisposable {

    public const int BYTE4 = 4;
    private Bitmap _bitmap;
    public int Width => _bitmap.Width;
    public int Height => _bitmap.Height;

    public byte[] Pixels;
    public RawImage() { }
    public RawImage(string filename) {
        string extension = Path.GetExtension(filename);
        if (extension == ".ppm") {
            _bitmap = ReadPPM(filename);
        }
        else {
            _bitmap = new Bitmap(filename);
        }
        BitmapData bitmapData = _bitmap.LockBits(
            new Rectangle(0, 0, _bitmap.Width, _bitmap.Height),
            ImageLockMode.ReadOnly,
            PixelFormat.Format32bppArgb
        );
        Pixels = new byte[bitmapData.Stride * _bitmap.Height];
        Marshal.Copy(bitmapData.Scan0, Pixels, 0, Pixels.Length);
        _bitmap.UnlockBits(bitmapData);
    }
    public RawImage(RawImage other) {
        _bitmap = other._bitmap.Clone() as Bitmap;
        Pixels = other.Pixels.ToArray();
    }
    public RawImage(int width, int height) {
        _bitmap = new Bitmap(width, height);
        BitmapData bitmapData = _bitmap.LockBits(
            new Rectangle(0, 0, _bitmap.Width, _bitmap.Height),
            ImageLockMode.ReadOnly,
            PixelFormat.Format32bppArgb
        );
        Pixels = new byte[bitmapData.Stride * _bitmap.Height];
        Marshal.Copy(bitmapData.Scan0, Pixels, 0, Pixels.Length);
        _bitmap.UnlockBits(bitmapData);
    }
    public Bitmap ToBitmap() {
        return new Bitmap(_bitmap);
    }
    public void FinishEdit() {
        BitmapData bitmapData = _bitmap.LockBits(
            new Rectangle(0, 0, _bitmap.Width, _bitmap.Height),
            ImageLockMode.WriteOnly,
            PixelFormat.Format32bppArgb
        );
        Marshal.Copy(Pixels, 0, bitmapData.Scan0, Pixels.Length);
        _bitmap.UnlockBits(bitmapData);
    }
    public void SetPixel(int x, int y, byte[] color) {
        int index = (y * Width + x) * BYTE4;
        Pixels[index] = color[0];
        Pixels[index + 1] = color[1];
        Pixels[index + 2] = color[2];
        Pixels[index + 3] = color[3];
    }
    public byte[] GetPixel(int x, int y) {
        /// todo : optimize here
        int index = (y * Width + x) * BYTE4;
        return new byte[] {
            Pixels[index],      // B
            Pixels[index+1],    // G
            Pixels[index+2],    // R
            Pixels[index+3],    // A
        };
    }

    public void SaveFile(string filename) {
        string extension = Path.GetExtension(filename);
        switch (extension.ToLower()) {
            case ".jpg":
            case ".jpeg":
                _bitmap.Save(filename, ImageFormat.Jpeg);
                break;
            case ".png":
                _bitmap.Save(filename, ImageFormat.Png);
                break;
            case ".bmp":
                _bitmap.Save(filename, ImageFormat.Bmp);
                break;
            case ".ppm":
                WritePPM(filename);
                break;
            default:
                Console.WriteLine("Unsupported file format.");
                break;
        }
    }
    public object Clone() {
        return new RawImage() {
            _bitmap = _bitmap.Clone() as Bitmap,
            Pixels = Pixels.ToArray()
        };
    }
    private void WritePPM(string filename) {
        using (StreamWriter writer = new StreamWriter(filename)) {
            // Write the PPM header
            writer.WriteLine("P3");                 // P6 format for binary PPM
            writer.WriteLine($"{Width} {Height}");  // Width, height
            writer.WriteLine("255");                // Maximum color value

            for (int i = 0; i < Pixels.Length; i += RawImage.BYTE4) {
                byte B = Pixels[i];
                byte G = Pixels[i + 1];
                byte R = Pixels[i + 2];
                writer.WriteLine($"{R,3} {G,3} {B,3}");
            }
        }
	}
}
public partial class MainWindow : Window {
	public MainWindow() {
		InitializeComponent();
		SaveFileBtn.IsEnabled = false;
		LeftRotateBtn.IsEnabled = false;
		RightRotateBtn.IsEnabled = false;
	}

	RawImage _outputRaw;
	RawImage _inputRaw;

	private void OpenFileBtn_Click(object sender, RoutedEventArgs e) {

		OpenFileDialog dialog = new OpenFileDialog();
		dialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.bmp; *.ppm)|*.jpg; *.jpeg; *.png; *.bmp; *.ppm";
		dialog.Title = "Open Image";
		if (dialog.ShowDialog() == true) {

			RawImage loadRaw = new RawImage(dialog.FileName);
			
			_inputRaw = new RawImage(loadRaw);
			_outputRaw = new RawImage(loadRaw);

			UpdateImageBox(OutputImgBox, _outputRaw.ToBitmap());
			UpdateImageBox(InputImgBox, _inputRaw.ToBitmap());

			SaveFileBtn.IsEnabled = true;
			LeftRotateBtn.IsEnabled = true;
			RightRotateBtn.IsEnabled = true;
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

	private void RightRotateBtn_Click(object sender, RoutedEventArgs e) {
		_outputRaw = ImageProcessing.RightRotate(_outputRaw);
		UpdateImageBox(OutputImgBox, _outputRaw.ToBitmap());
	}

	private void LeftRotateBtn_Click(object sender, RoutedEventArgs e) {
		_outputRaw = ImageProcessing.LeftRotate(_outputRaw);
		UpdateImageBox(OutputImgBox, _outputRaw.ToBitmap());
	}

	private BitmapImage BitmapToImageSource(Bitmap bitmap) {
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
	private void UpdateImageBox(Image imgBox, Bitmap bitmap) {
		
		imgBox.Source = BitmapToImageSource(bitmap);
	}
}

