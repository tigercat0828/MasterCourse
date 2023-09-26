using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;
using System.Runtime.InteropServices;

namespace ACGRT;

[SuppressMessage("Microsoft.Design", "CA1416:ValidatePlatformCompatibility")]
public class RawImage : ICloneable, IDisposable {

    public const int BYTE4 = 4;
    public Bitmap _bitmap;
    public int Width => _bitmap.Width;
    public int Height => _bitmap.Height;

    public byte[] Pixels;
    public RawImage() { }
    public RawImage(string filename) {
        
        _bitmap = new Bitmap(filename);
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
    public void SetPixel(int x, int y, Vector3 color) {
        int index = (y * Width + x) * BYTE4;
        Pixels[index] = (byte)color.Z;
        Pixels[index + 1] = (byte)color.Y;
        Pixels[index + 2] = (byte)color.X;
        Pixels[index + 3] = 255;
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
    public void FlipY() {
        _bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
    }
    public void FlipX() {
        _bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
    }
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
