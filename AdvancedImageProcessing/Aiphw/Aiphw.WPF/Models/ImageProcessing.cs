using System;

namespace Aiphw.WPF.Models;
public static class ImageProcessing {
    #region DEBUG
    public static void PrintPixelImage(RawImage image) {

        int Width = image.Width;
        int Height = image.Height;
        Console.WriteLine($"Width = {Width}");
        Console.WriteLine($"Height = {Height}");
        Console.WriteLine($"Pixel count = {image.Pixels.Length / 4}");

        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                int index = (y * Width + x) * 4;
                byte B = image.Pixels[index + 0];
                byte G = image.Pixels[index + 1];
                byte R = image.Pixels[index + 2];
                byte A = image.Pixels[index + 3];
                Console.Write($"<{R,3} {G,3} {B,3} {A,3}>, ");
            }
            Console.WriteLine();
        }
    }
    public static void PrintPixelStream(RawImage image) {

        for (int i = 0; i < image.Pixels.Length; i += RawImage.BYTE4) {
            byte B = image.Pixels[i];
            byte G = image.Pixels[i + 1];
            byte R = image.Pixels[i + 2];
            byte A = image.Pixels[i + 3];
            Console.WriteLine($"{R,3} {G,3} {B,3} {A,3}");
        }
    }
    #endregion
    public static RawImage GrayScale(RawImage image) {
        RawImage grayscale = new(image.Width, image.Height);
        Span<byte> inputPixels = new Span<byte>(image.Pixels);
        Span<byte> outputPixels = new Span<byte>(grayscale.Pixels);

        for (int x = 0; x < image.Width; x++) {
            for (int y = 0; y < image.Height; y++) {

                int index = (y * image.Width + x) * 4;

                byte gray = (byte)((inputPixels[index] + inputPixels[index + 1] + inputPixels[index + 2]) / 3);

                outputPixels[index] = gray;
                outputPixels[index + 1] = gray;
                outputPixels[index + 2] = gray;
                outputPixels[index + 3] = 255;
            }
        }
        grayscale.FinishEdit();
        return grayscale;
    }

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
