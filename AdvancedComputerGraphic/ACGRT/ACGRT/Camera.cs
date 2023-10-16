using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ACGRT {
    public class Camera {
        public float AspectRatio = 1.0f;
        public int ImageWidth { get; private set; } = 100;
        public int SampleNum { get; private set; }= 1;
        public int ImageHeight { get; private set; } = 100;
        public Vector3 Position { get; private set; }

        public Vector3 Pixel00Loc { get; private set; }
        private Vector3 deltaU;
        private Vector3 deltaV;
        Random random = new ();
        public void SetAspectRatio(float ratio) => AspectRatio = ratio;
        public void SetImageWidth(int width) => ImageWidth = width;
        public void SetSampleNum(int sample) => SampleNum = sample;
        public void Render(Scene world) {

            Console.WriteLine($"{ImageWidth} x {ImageHeight}");
            RawImage output = new(ImageWidth, ImageHeight);
            for (int y = 0; y < ImageHeight; y++) {
                //Console.WriteLine($"Scanline {y, 4} ...");
                for (int x = 0; x < ImageWidth; x++) {
                    Ray ray = CastRay(x, y);
                    Color fragment = 255 * RayColor(ray, world);
                    output.SetPixel(x, y, fragment);
                }
            }
            output.SaveFile("Hello.ppm");
            Console.WriteLine("Done");
        }
        public void Initialize() {
            ImageHeight = (int)(ImageWidth / AspectRatio);
            Position = new Vector3(0, 0, 0);
            // Camera
            float FocalLength = 1.0f;
            float ViewportHeight = 2.0f;
            float ViewportWidth = ViewportHeight * ImageWidth / ImageHeight;
            Vector3 CameraPosition = Vector3.Zero;
            Vector3 ViewportU = new(ViewportWidth, 0, 0);
            Vector3 ViewportV = new(0, -ViewportHeight, 0);
            deltaU = ViewportU / ImageWidth;
            deltaV = ViewportV / ImageHeight;

            Vector3 ViewPortUpperLeft = CameraPosition - new Vector3(0, 0, FocalLength) - ViewportU / 2 - ViewportV / 2;
            Pixel00Loc = ViewPortUpperLeft + 0.5f * (deltaU + deltaV);
        }
        public Color RayColor(Ray ray, Scene world) {
            HitRecord record = new();
            Interval interval = new (0, float.MaxValue);
            if (world.Hit(ray, interval, ref record)) {
                return 0.5f * new Color(record.Normal + Vector3.One);
            }
            // sky
            Vector3 uniDir = Vector3.Normalize(ray.direction);
            float a = 0.5f * (uniDir.Y + 1.0f);
            return (1.0f - a) * new Color(1.0f, 1.0f, 1.0f) + a * new Color(0.5f, 0.7f, 1.0f);
        }
        private Ray CastRay(int i, int j) {
            Vector3 pixelCenter = Pixel00Loc + i * deltaU + j * deltaV;
            //Vector3 pixelSample = pixelCenter + PixelSampleSquare();
            //Vector3 rayDirection = pixelSample - Position;
            Vector3 rayDirection = pixelCenter - Position;
            return new(Position, rayDirection);
        }
        private Vector3 PixelSampleSquare() {

            float px = -0.5f + random.NextSingle();
            float py = -0.5f + random.NextSingle();
            return px * deltaU + py * deltaV;
        }
    }
}
