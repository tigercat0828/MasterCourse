using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
namespace Aiphw.WPF.Models;

[SuppressMessage("Microsoft.Design", "CA1416:ValidatePlatformCompatibility")]
public class RawImage : ICloneable, IDisposable {
    private const int B = 0, G = 1, R = 2, A = 3;

    public const int BYTE4 = 4;
    private Bitmap _bitmap;

    public int Width => _bitmap.Width;
    public int Height => _bitmap.Height;

    public byte[] Pixels { get; private set; }
    public RawImage() { }
    public RawImage(string filename) {
        string extension = Path.GetExtension(filename);
        if (extension == ".ppm") {
            _bitmap = PpmReadWriter.ReadPPM(filename);
        }
        else {
            _bitmap = new Bitmap(filename);
        }
        Bitmap2Pixels();
    }
    public RawImage(RawImage other) {
        _bitmap = other._bitmap.Clone() as Bitmap;
        Pixels = other.Pixels.ToArray();
    }
    public RawImage(int width, int height) {
        _bitmap = new Bitmap(width, height);
        Bitmap2Pixels();
    }
    public Bitmap ToBitmap() {
        return new Bitmap(_bitmap);
    }
    public void FinishEdit() {
        Pixels2Bitmap();
    }
    public void SetPixel(int x, int y, byte[] color) {
        int index = (y * Width + x) * BYTE4;
        Pixels[index + B] = color[B];
        Pixels[index + G] = color[G];
        Pixels[index + R] = color[R];
        Pixels[index + A] = color[A];
    }
    public byte[] GetPixel(int x, int y) {
        /// todo : optimize here
        int index = (y * Width + x) * BYTE4;
        return new byte[] {
                Pixels[index + B],
                Pixels[index + G],
                Pixels[index + R],
                Pixels[index + A],
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
                PpmReadWriter.WritePPM(filename, Pixels, Width, Height);
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

    public void Dispose() {
        _bitmap.Dispose();
    }

    private void Bitmap2Pixels() {
        BitmapData bitmapData = _bitmap.LockBits(
            new Rectangle(0, 0, _bitmap.Width, _bitmap.Height),
            ImageLockMode.ReadOnly,
            PixelFormat.Format32bppArgb
        );
        Pixels = new byte[bitmapData.Stride * _bitmap.Height];
        Marshal.Copy(bitmapData.Scan0, Pixels, 0, Pixels.Length);
        _bitmap.UnlockBits(bitmapData);
    }
    private void Pixels2Bitmap() {
        BitmapData bitmapData = _bitmap.LockBits(
            new Rectangle(0, 0, _bitmap.Width, _bitmap.Height),
            ImageLockMode.WriteOnly,
            PixelFormat.Format32bppArgb
        );
        Marshal.Copy(Pixels, 0, bitmapData.Scan0, Pixels.Length);
        _bitmap.UnlockBits(bitmapData);
    }

}