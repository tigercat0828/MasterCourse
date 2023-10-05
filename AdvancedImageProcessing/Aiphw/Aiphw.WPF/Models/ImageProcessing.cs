using System;

namespace Aiphw.WPF.Models;
public static class ImageProcessing {
    #region DEBUG
    public static void PrintPixelImage(RawImage image) {
        const int B = 0, G = 1, R = 2, A = 3;
        int Width = image.Width;
        int Height = image.Height;
        Console.WriteLine($"Width = {Width}");
        Console.WriteLine($"Height = {Height}");
        Console.WriteLine($"Pixel count = {image.Pixels.Length / 4}");

        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                int index = (y * Width + x) * 4;
                byte b = image.Pixels[index + B];
                byte g = image.Pixels[index + G];
                byte r = image.Pixels[index + R];
                byte a = image.Pixels[index + A];
                Console.Write($"[{r,3} {g,3} {b,3} {a,3} ], ");
            }
            Console.WriteLine();
        }
    }
    public static void PrintPixelStream(RawImage image) {

        for (int i = 0; i < image.Pixels.Length; i += RawImage.BYTE4) {
            byte b = image.Pixels[i + B];
            byte g = image.Pixels[i + G];
            byte r = image.Pixels[i + R];
            byte a = image.Pixels[i + A];
            Console.WriteLine($"[{r,3} {g,3} {b,3} {a,3} ], ");
        }
    }
    #endregion

    const int B = 0, G = 1, R = 2, A = 3;
    public static byte[] PaddingForKernel(RawImage image, int size) {

        int oldWidth = image.Width;
        int oldHeight = image.Height;
        int newWidth = oldWidth + size - 1;
        int newHeight = oldHeight + size - 1;
        size /= 2;
        byte[] destin = new byte[newWidth * newHeight];
        byte[] source = image.Pixels;
        for (int i = 0; i < oldWidth; i++) {
            Array.Copy(
                source,                     // source array
                i * oldWidth,               // source index
                destin,                     // destination array
                (i + size) * newHeight + size,    // destination index
                oldWidth                    // length
            );
        }
        return destin;
    }

    public static RawImage GrayScale(RawImage image) {
        RawImage grayscale = new(image.Width, image.Height);
        Span<byte> inputPixels = new(image.Pixels);
        Span<byte> outputPixels = new(grayscale.Pixels);

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

    public static RawImage SaltPepperNoise(RawImage input, out RawImage noise, int noiseValue) {
        Random random = new Random();
        RawImage output = new RawImage(input);
        noise = new RawImage(input.Width, input.Height);

        Span<byte> outputPixels = output.Pixels;
        Span<byte> noisePixels = noise.Pixels;

        noiseValue /= 2;
        for (int i = 0; i < input.Width * input.Height; i++) {
            noisePixels[i * 4] = 128; // R
            noisePixels[i * 4 + 1] = 128; // G
            noisePixels[i * 4 + 2] = 128; // B
            noisePixels[i * 4 + 3] = 255; // A

            int rnd = random.Next(100);
            if (rnd <= noiseValue) {
                // black
                outputPixels[i * 4 + B] = 0;
                outputPixels[i * 4 + G] = 0;
                outputPixels[i * 4 + R] = 0;
                outputPixels[i * 4 + A] = 255;

                noisePixels[i * 4 + B] = 0;
                noisePixels[i * 4 + G] = 0;
                noisePixels[i * 4 + R] = 0;
                noisePixels[i * 4 + A] = 255;
            }
            else if (rnd >= 100 - noiseValue) {
                // white
                outputPixels[i * 4 + B] = 255;
                outputPixels[i * 4 + G] = 255;
                outputPixels[i * 4 + R] = 255;
                outputPixels[i * 4 + A] = 255;

                noisePixels[i * 4 + B] = 255;
                noisePixels[i * 4 + G] = 255;
                noisePixels[i * 4 + R] = 255;
                noisePixels[i * 4 + A] = 255;
            }
        }

        output.FinishEdit();
        noise.FinishEdit();
        return output;
    }


    public static RawImage GaussianNoise(RawImage input, out RawImage noise, float sigma) {
        Random varphi = new();
        Random gamma = new();

        int width = input.Width;
        int height = input.Height;
        RawImage output = new RawImage(width, height);
        noise = new RawImage(width, height);
        for (int y = 0; y < height; y++) {
            for (int x = 1; x < width; x += 2) {

                float sqrt = MathF.Sqrt(-2.0f * MathF.Log(gamma.NextSingle()));
                float trian = varphi.NextSingle() * 2.0f * MathF.PI;
                float z1 = sigma * MathF.Cos(trian) * sqrt;
                float z2 = sigma * MathF.Sin(trian) * sqrt;

                byte[] noised1 = new byte[4];
                byte[] noised2 = new byte[4];
                byte[] pixel1 = input.GetPixel(x - 1, y);
                byte[] pixel2 = input.GetPixel(x, y);

                noised1[B] = (byte)Math.Clamp(pixel1[B] + z1, 0, 255);
                noised1[G] = (byte)Math.Clamp(pixel1[G] + z1, 0, 255);
                noised1[R] = (byte)Math.Clamp(pixel1[R] + z1, 0, 255);
                noised1[A] = 255;

                noised2[B] = (byte)Math.Clamp(pixel2[B] + z2, 0, 255);
                noised2[G] = (byte)Math.Clamp(pixel2[G] + z2, 0, 255);
                noised2[R] = (byte)Math.Clamp(pixel2[R] + z2, 0, 255);
                noised2[A] = 255;

                output.SetPixel(x - 1, y, noised1);
                output.SetPixel(x, y, noised2);

                byte z1b = (byte)z1;
                byte z2b = (byte)z2;
                noise.SetPixel(x - 1, y, new byte[] { z1b, z1b, z1b, 255 });
                noise.SetPixel(x, y, new byte[] { z2b, z2b, z2b, 255 });
            }
        }
        output.FinishEdit();
        noise.FinishEdit();
        return output;
    }

    public static RawImage Overlay(RawImage input, RawImage noise) {

        if (input.Width != noise.Width || input.Height != noise.Height) {
            return null;
        }
        RawImage output = new(input.Width, input.Height);
        for (int y = 0; y < input.Height; y++) {
            for (int x = 0; x < input.Width; x++) {
                byte[] ip = input.GetPixel(x, y);    // input pixel
                byte[] np = noise.GetPixel(x, y);    // noise pixel
                byte[] op = new byte[4];
                op[3] = 255;
                for (int i = 0; i < 3; i++) {
                    op[i] = (byte)Math.Clamp(ip[i] + np[i], 0, 255);
                }
            }
        }
        output.FinishEdit();
        return output;
    }
}
