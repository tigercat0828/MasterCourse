#define LOAD_SLOW_CODE

namespace Aiphw.Models;

public static class ImageProcessing {
    const int cB = 0, cG = 1, cR = 2, cA = 3;
    const int B = 0, G = 8, R = 16, A = 24;
    #region DEBUG
    public static void PrintPixelImage(RawImage2 image) {
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
    public static void PrintPixelStream(RawImage2 image) {

        for (int i = 0; i < image.Pixels.Length; i += 4) {
            byte b = image.Pixels[i + cB];
            byte g = image.Pixels[i + cG];
            byte r = image.Pixels[i + cR];
            byte a = image.Pixels[i + cA];
            Console.WriteLine($"[{r,3} {g,3} {b,3} {a,3} ], ");
        }
    }
    #endregion
    public static RawImage2 Padding(RawImage2 image, int size) {

        int oldWidth = image.Width;
        int oldHeight = image.Height;
        int newWidth = oldWidth + size - 1;
        int newHeight = oldHeight + size - 1;
        size /= 2;
        byte[] destin = new byte[newWidth * newHeight * 4];
        byte[] source = image.Pixels;
        for (int i = 0; i < oldHeight; i++) {
            Array.Copy(
                source,                                 // source array
                i * oldWidth * 4,                         // source index
                destin,                                 // destination array
                ((i + size) * newWidth + size) * 4,       // destination index
                oldWidth * 4                              // length
            );
        }
        return new RawImage2(destin, newWidth, newHeight);
    }
    public static RawImage2 Extract(int x, int y, int width, int height) {
        throw new NotImplementedException();
    }
    public static RawImage2 ConvolutionRGB(RawImage2 input, MaskKernel kernel) {
        int kernelSize = kernel.Size;
        int kernelOffset = kernelSize / 2;
        int width = input.Width;
        int height = input.Height;
        RawImage2 output = new RawImage2(width, height);

        Parallel.For(0, height, y => {
            for (int x = 0; x < width; x++) {
                float[] rgba = new float[] { 0.0f, 0.0f, 0.0f, 0.0f };

                for (int i = -kernelOffset; i <= kernelOffset; i++) {
                    for (int j = -kernelOffset; j <= kernelOffset; j++) {
                        int pixelX = Math.Clamp(x + i, 0, width - 1);
                        int pixelY = Math.Clamp(y + j, 0, height - 1);
                        int pixelIndex = (pixelY * width + pixelX) * 4;
                        float kernelValue = kernel[i + kernelOffset][j + kernelOffset];

                        rgba[cB] += input.Pixels[pixelIndex + cB] * kernelValue;
                        rgba[cG] += input.Pixels[pixelIndex + cG] * kernelValue;
                        rgba[cR] += input.Pixels[pixelIndex + cR] * kernelValue;
                        // rgba[A] += input.Pixels[pixelIndex + A] * 255;
                    }
                }

                int index = (y * width + x) * 4;
                output.Pixels[index + cB] = (byte)Math.Clamp(rgba[cB] / kernel.Scalar, 0, 255);
                output.Pixels[index + cG] = (byte)Math.Clamp(rgba[cG] / kernel.Scalar, 0, 255);
                output.Pixels[index + cR] = (byte)Math.Clamp(rgba[cR] / kernel.Scalar, 0, 255);
                output.Pixels[index + cA] = 255;
            }
        });

        output.FinishEdit();
        return output;
    }
    public static RawImage2 ConvolutionGray(RawImage2 input, MaskKernel kernel) {
        int kernelSize = kernel.Size;
        int kernelOffset = kernelSize / 2;
        int width = input.Width;
        int height = input.Height;
        RawImage2 output = new(width, height);

        Parallel.For(0, height, y => {
            for (int x = 0; x < width; x++) {
                float[] rgba = new float[] { 0.0f, 0.0f, 0.0f, 0.0f };

                for (int i = -kernelOffset; i <= kernelOffset; i++) {
                    for (int j = -kernelOffset; j <= kernelOffset; j++) {
                        int X = Math.Clamp(x + i, 0, width - 1);
                        int Y = Math.Clamp(y + j, 0, height - 1);
                        int pIndex = (Y * width + X) * 4;
                        float kernelValue = kernel[i + kernelOffset][j + kernelOffset];

                        rgba[cB] += input.Pixels[pIndex + cB] * kernelValue;
                    }
                }
                int index = (y * width + x) * 4;
                output.Pixels[index + cB] = (byte)Math.Clamp(rgba[cB] / kernel.Scalar, 0, 255);
                output.Pixels[index + cG] = (byte)Math.Clamp(rgba[cB] / kernel.Scalar, 0, 255);
                output.Pixels[index + cR] = (byte)Math.Clamp(rgba[cB] / kernel.Scalar, 0, 255);
                output.Pixels[index + cA] = 255;
            }
        });

        output.FinishEdit();
        return output;
    }
    public static RawImage2 Smooth(RawImage2 input) {
        MaskKernel smoothMask = MaskKernel.LoadPreBuiltMask(DefaultMask.GaussianSmooth);
        return ConvolutionRGB(input, smoothMask);
    }
    public static RawImage2 Reverse(RawImage2 input) {
        int width = input.Width;
        int height = input.Height;
        RawImage2 output = new(width, height);
        Parallel.For(0, height, y => {
            for (int x = 0; x < width; x++) {
                int index = (y * width + x) * 4;
                output.Pixels[index + cB] = (byte)(255 - input.Pixels[index + cB]);
                output.Pixels[index + cG] = (byte)(255 - input.Pixels[index + cG]);
                output.Pixels[index + cR] = (byte)(255 - input.Pixels[index + cR]);
                output.Pixels[index + cA] = 255;
            }
        });

        output.FinishEdit();
        return output;
    }
    public static RawImage2 EdgeDetection(RawImage2 input) {
        MaskKernel sobelXmask = MaskKernel.LoadPreBuiltMask(DefaultMask.SobelX);
        MaskKernel sobelYmask = MaskKernel.LoadPreBuiltMask(DefaultMask.SobelY);
        RawImage2 grayscale = GrayScale2(input);
        RawImage2 sobelXimage = ConvolutionGray(input, sobelXmask);
        RawImage2 sobelYimage = ConvolutionGray(input, sobelYmask);
        Func<byte, byte, byte> gradient = (a, b) => (byte)Math.Clamp(Math.Sqrt(a * a + b * b), 0, 255);
        RawImage2 edgeDetected = Overlay(sobelYimage, sobelYimage, gradient);
        return Reverse(edgeDetected);
        // return sobelYimage;
    }
    
    public static RawImage GrayScale(RawImage input) {
        int width = input.Width;
        int height = input.Height;
        int numPx = width * height;
        RawImage output = new RawImage(width, height);

        Parallel.For(0, numPx, i =>
        {
            byte b = (byte)(input.Pixels[i] >> B & 0xFF);
            byte g = (byte)(input.Pixels[i] >> G & 0xFF);
            byte r = (byte)(input.Pixels[i] >> R & 0xFF);
            byte gray = (byte)((b + g + r) / 3.0f);
            //byte gray = (byte)(0.299 * r + 0.587 * g + 0.114 * b);
            output.Pixels[i] = (uint)(gray << B | gray << G | gray << R | 0xFF000000);
        });

        return output;
    }
    public static RawImage RightRotate(RawImage input) {
        int width = input.Width;
        int height = input.Height;

        int newWidth = width;
        int newHeight = height;

        RawImage output = new(newHeight, newWidth);

        Parallel.For(0, height, j => {
            for (int i = 0; i < width; i++) {
                int inIndex = j * width + i;
                int x = newHeight - 1 - j;
                int y = i;
                int outIndex = y * newHeight + x;
                output.Pixels[outIndex] = input.Pixels[inIndex];
            }
        });
        return output;
    }
    public static RawImage LeftRotate(RawImage input) {
        int width = input.Width;
        int height = input.Height;

        int newWidth = height;
        int newHeight = width;

        RawImage output = new(newWidth, newHeight);

        Parallel.For(0, height, j => {
            for (int i = 0; i < width; i++) {
                int inIndex = j * width + i;
                int x = j;
                int y = width - 1 - i;
                int outIndex = y * newWidth + x;
                output.Pixels[outIndex] = input.Pixels[inIndex];
            }
        });

        return output;
    }
    public static RawImage SaltPepperNoise(RawImage input, out RawImage noise, int noiseValue) {
        Random random = new ();
        RawImage output = new (input);
        int width = input.Width;
        int height = input.Height;

        noise = new RawImage(width, height);
        uint black = 0xFF000000; // ARGB
        uint white = 0xFFFFFFFF;
        uint gray = 0xFF808080;
        noiseValue /= 2;
        for (int i = 0; i < width * height; i++) {
            int rnd = random.Next(100);
            if (rnd <= noiseValue) {
                output.Pixels[i] = black;
                noise.Pixels[i] = black;
            }
            else if (rnd >= 100 - noiseValue) {
                output.Pixels[i] = white;
                noise.Pixels[i] = white;
            }
            else {
                noise.Pixels[i] = gray;
            }
        }
        return output;
    }
    public static RawImage GaussianNoise(RawImage input, out RawImage outNoise, float sigma) {
        Random varphi = new();
        Random gamma = new();

        int width = input.Width;
        int height = input.Height;
        RawImage output = new(width, height);
        RawImage noise = new RawImage(width, height);
        Parallel.For(0, height, y => {
            for (int x = 1; x < width; x += 2) {

                float sqrt = MathF.Sqrt(-2.0f * MathF.Log(gamma.NextSingle()));
                float trian = varphi.NextSingle() * 2.0f * MathF.PI;
                float z1 = sigma * MathF.Cos(trian) * sqrt;
                float z2 = sigma * MathF.Sin(trian) * sqrt;

                byte z1b = (byte)z1;
                byte z2b = (byte)z2;

                uint noise1 = (uint)(z1b << B | z1b << G | z1b << R | 0xFF000000);
                uint noise2 = (uint)(z2b << B | z2b << G | z2b << R | 0xFF000000);
                noise.SetPixel(x - 1, y, noise1);
                noise.SetPixel(x, y, noise2);

                uint p1 = input.GetPixel(x - 1, y);
                uint p2 = input.GetPixel(x, y);

                byte B1 = (byte)Math.Clamp((p1 >> B & 0xFF) + z1, 0, 255);
                byte G1 = (byte)Math.Clamp((p1 >> G & 0xFF) + z1, 0, 255);
                byte R1 = (byte)Math.Clamp((p1 >> R & 0xFF) + z1, 0, 255);

                byte B2 = (byte)Math.Clamp((p2 >> B & 0xFF) + z2, 0, 255);
                byte G2 = (byte)Math.Clamp((p2 >> G & 0xFF) + z2, 0, 255);
                byte R2 = (byte)Math.Clamp((p2 >> R & 0xFF) + z2, 0, 255);

                uint noised1 = (uint)(B1 << B | G1 << G | R1 << R | 0xFF000000);
                uint noised2 = (uint)(B2 << B | G2 << G | R2 << R | 0xFF000000);

                output.SetPixel(x - 1, y, noised1);
                output.SetPixel(x, y, noised2);
            }
        });

        outNoise = noise;
        return output;
    }

    public static RawImage2 Overlay(RawImage2 input1, RawImage2 input2, Func<byte, byte, byte> func) {


        if (input1.Width != input2.Width || input1.Height != input2.Height) {
            return null;
        }
        int width = input1.Width;
        int height = input1.Height;

        RawImage2 output = new RawImage2(width, height);

        Parallel.For(0, height, y => {
            for (int x = 0; x < width; x++) {
                int index = (y * width + x) * 4;
                output.Pixels[index + cB] = func(input1.Pixels[index + cB], input2.Pixels[index + cB]);
                output.Pixels[index + cG] = func(input1.Pixels[index + cG], input2.Pixels[index + cG]);
                output.Pixels[index + cR] = func(input1.Pixels[index + cR], input2.Pixels[index + cR]);
                output.Pixels[index + cA] = 255;
            }
        });

        output.FinishEdit();
        return output;
    }

#if LOAD_SLOW_CODE

    

    public static RawImage GaussianNoise_ST(RawImage input, out RawImage outNoise, float sigma) {
        Random varphi = new();
        Random gamma = new();

        int width = input.Width;
        int height = input.Height;
        RawImage output = new(width, height);
        RawImage noise = new RawImage(width, height);
        for (int y = 0; y < height; y++) {
            for (int x = 1; x < width; x += 2) {

                float sqrt = MathF.Sqrt(-2.0f * MathF.Log(gamma.NextSingle()));
                float trian = varphi.NextSingle() * 2.0f * MathF.PI;
                float z1 = sigma * MathF.Cos(trian) * sqrt;
                float z2 = sigma * MathF.Sin(trian) * sqrt;

                byte z1b = (byte)z1;
                byte z2b = (byte)z2;

                uint noise1 = (uint)(z1b << B | z1b << G | z1b << R | 0xFF000000);
                uint noise2 = (uint)(z2b << B | z2b << G | z2b << R | 0xFF000000);
                noise.SetPixel(x - 1, y, noise1);
                noise.SetPixel(x, y, noise2);

                uint p1 = input.GetPixel(x - 1, y);
                uint p2 = input.GetPixel(x, y);

                byte B1 = (byte)Math.Clamp((p1 >> B & 0xFF) + z1, 0, 255);
                byte G1 = (byte)Math.Clamp((p1 >> G & 0xFF) + z1, 0, 255);
                byte R1 = (byte)Math.Clamp((p1 >> R & 0xFF) + z1, 0, 255);

                byte B2 = (byte)Math.Clamp((p2 >> B & 0xFF) + z2, 0, 255);
                byte G2 = (byte)Math.Clamp((p2 >> G & 0xFF) + z2, 0, 255);
                byte R2 = (byte)Math.Clamp((p2 >> R & 0xFF) + z2, 0, 255);

                uint noised1 = (uint)(B1 << B | G1 << G | R1 << R | 0xFF000000);
                uint noised2 = (uint)(B2 << B | G2 << G | R2 << R | 0xFF000000);

                output.SetPixel(x - 1, y, noised1);
                output.SetPixel(x, y, noised2);
            }
        };
        outNoise = noise;
        return output;
    }
    public static RawImage2 SaltPepperNoise2(RawImage2 input, out RawImage2 noise, int noiseValue) {
        Random random = new Random();
        RawImage2 output = new RawImage2(input);
        noise = new RawImage2(input.Width, input.Height);

        Span<byte> outputPixels = output.Pixels;
        Span<byte> noisePixels = noise.Pixels;

        noiseValue /= 2;
        for (int i = 0; i < input.Width * input.Height; i++) {
            noisePixels[i * 4 + cB] = 128;
            noisePixels[i * 4 + cG] = 128;
            noisePixels[i * 4 + cR] = 128;
            noisePixels[i * 4 + cA] = 255;

            int rnd = random.Next(100);
            if (rnd <= noiseValue) {
                // black
                outputPixels[i * 4 + cB] = 0;
                outputPixels[i * 4 + cG] = 0;
                outputPixels[i * 4 + cR] = 0;
                outputPixels[i * 4 + cA] = 255;

                noisePixels[i * 4 + cB] = 0;
                noisePixels[i * 4 + cG] = 0;
                noisePixels[i * 4 + cR] = 0;
                noisePixels[i * 4 + cA] = 255;
            }
            else if (rnd >= 100 - noiseValue) {
                // white
                outputPixels[i * 4 + cB] = 255;
                outputPixels[i * 4 + cG] = 255;
                outputPixels[i * 4 + cR] = 255;
                outputPixels[i * 4 + cA] = 255;

                noisePixels[i * 4 + cB] = 255;
                noisePixels[i * 4 + cG] = 255;
                noisePixels[i * 4 + cR] = 255;
                noisePixels[i * 4 + cA] = 255;
            }
        }
        output.FinishEdit();
        noise.FinishEdit();
        return output;
    }
    public static RawImage2 GrayScale2(RawImage2 input) {
        int width = input.Width;
        int height = input.Height;
        RawImage2 output = new(width, height);

        Parallel.For(0, height, y => {
            for (int x = 0; x < width; x++) {
                int index = (y * width + x) * 4;
                byte gray = (byte)((input.Pixels[index + cB] + input.Pixels[index + cG] + input.Pixels[index + cR]) / 3);

                output.Pixels[index + cB] = gray;
                output.Pixels[index + cG] = gray;
                output.Pixels[index + cR] = gray;
                output.Pixels[index + cA] = 255;
            }
        });
        output.FinishEdit();
        return output;

    }
    public static RawImage2 GaussianNoise2(RawImage2 input, out RawImage2 noise, float sigma) {
        Random varphi = new();
        Random gamma = new();

        int width = input.Width;
        int height = input.Height;
        RawImage2 output = new RawImage2(width, height);
        noise = new RawImage2(width, height);
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

                noised1[cB] = (byte)Math.Clamp(pixel1[cB] + z1, 0, 255);
                noised1[cG] = (byte)Math.Clamp(pixel1[cG] + z1, 0, 255);
                noised1[cR] = (byte)Math.Clamp(pixel1[cR] + z1, 0, 255);
                noised1[cA] = 255;

                noised2[cB] = (byte)Math.Clamp(pixel2[cB] + z2, 0, 255);
                noised2[cG] = (byte)Math.Clamp(pixel2[cG] + z2, 0, 255);
                noised2[cR] = (byte)Math.Clamp(pixel2[cR] + z2, 0, 255);
                noised2[cA] = 255;

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
#endif
}
