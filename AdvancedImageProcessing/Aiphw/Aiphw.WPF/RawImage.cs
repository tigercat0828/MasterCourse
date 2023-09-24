using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
namespace Aiphw.WPF;

[SuppressMessage("Microsoft.Design", "CA1416:ValidatePlatformCompatibility")]
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
    #region PPM format

    private Bitmap ReadPPM(string filename) {
        try {
            using (StreamReader reader = new StreamReader(filename)) {
                // Read and validate the PPM format (magic number)
                string format = reader.ReadLine()?.Trim();
                if (format != "P3") {
                    Console.WriteLine("Invalid PPM format. Only P3 (plain text) is supported.");
                    return null;
                }

                // Read and parse the width and height
                string[] dimensions = reader.ReadLine()?.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (dimensions == null || dimensions.Length != 2 || !int.TryParse(dimensions[0], out int width) || !int.TryParse(dimensions[1], out int height) || width <= 0 || height <= 0) {
                    Console.WriteLine("Invalid image dimensions.");
                    return null;
                }

                // Read and parse the maximum color value (usually 255)
                string maxColorValue = reader.ReadLine()?.Trim();
                int maxValue;
                if (!int.TryParse(maxColorValue, out maxValue) || maxValue <= 0 || maxValue > 255) {
                    Console.WriteLine("Invalid maximum color value.");
                    return null;
                }

                // Read and parse the pixel data
                byte[] pixelData = new byte[width * height * 3]; // Assuming RGB format
                int pixelIndex = 0;

                while (!reader.EndOfStream) {
                    string line = reader.ReadLine()?.Trim();
                    string[] pixelTokens = line?.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    if (pixelTokens == null || pixelTokens.Length != 3 || !byte.TryParse(pixelTokens[0], out byte red) || !byte.TryParse(pixelTokens[1], out byte green) || !byte.TryParse(pixelTokens[2], out byte blue)) {
                        Console.WriteLine("Invalid pixel color values.");
                        return null;
                    }

                    pixelData[pixelIndex++] = red;
                    pixelData[pixelIndex++] = green;
                    pixelData[pixelIndex++] = blue;
                }

                // Create a Bitmap from the pixel data
                Bitmap bitmap = new Bitmap(width, height);

                for (int y = 0; y < height; y++) {
                    for (int x = 0; x < width; x++) {
                        int pixelOffset = (y * width + x) * 3;
                        Color pixelColor = Color.FromArgb(pixelData[pixelOffset], pixelData[pixelOffset + 1], pixelData[pixelOffset + 2]);
                        bitmap.SetPixel(x, y, pixelColor);
                    }
                }

                return bitmap;
            }
        }
        catch (Exception ex) {
            Console.WriteLine($"Error reading P3 PPM file: {ex.Message}");
            return null;
        }
    }

    public void Dispose() {
        _bitmap.Dispose();
    }
    #endregion
    /// <summary>
    /// optimize with span (ref struct)
    /// </summary>
    //public void SetPixel(int x, int y, ColorBGRA color) {
    //    int index = (y * Width + x) * BYTE4;
    //    color.CopyTo(Pixels.AsSpan(index));
    //}
    //public ColorBGRA GetPixel(int x, int y) {
    //    / todo : optimize here
    //    int index = (y * Width + x) * BYTE4;
    //    return new ColorBGRA(Pixels, index, BYTE4);
    //}
}
