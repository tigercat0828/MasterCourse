using System.Numerics;

namespace ACGRT {
    public class Camera {
        const float DEG2RAD = 0.0174532925f;
        private readonly Random random = new();
        public float AspectRatio { get; private set; } = 1.0f;
        public int ImageWidth { get; private set; } = 100;
        public int SampleNum { get; private set; } = 10;
        public int ImageHeight { get; private set; } = 100;
        public Vector3 Position { get; private set; }
        public int MaxDepth { get; private set; } = 10;
        public float FOV { get; private set; } = 90f; // field of view
        public Vector3 LookFrom = new(0, 0, -1);
        public Vector3 LookAt = new(0, 0, 0);
        public Vector3 Vup = new(0, 1, 0);
        public Vector3 Pixel00Loc { get; private set; }
        private Vector3 deltaU;
        private Vector3 deltaV;
        private Vector3 U, V, W;
        public void SetImageSize(int width, int height) {
            ImageWidth = width;
            ImageHeight = height;
        }
        public void SetMaxDepth(int depth) => MaxDepth = depth;
        public void SetPosition(Vector3 position) => Position = position;
        public void SetAspectRatio(float ratio) => AspectRatio = ratio;
        public void SetImageWidth(int width) => ImageWidth = width;
        public void SetSampleNum(int sample) => SampleNum = sample;
        public void SetFOV(float fov) => FOV = fov;
        public void Render(Scene world, string filename) {
            Console.WriteLine($"Size : {ImageWidth} x {ImageHeight} Sample : {SampleNum}");
            RawImage output = new(ImageWidth, ImageHeight);
            for (int y = 0; y < ImageHeight; y++) {
                //Console.WriteLine($"Scanline {y, 4} ...");
                for (int x = 0; x < ImageWidth; x++) {
                    Color fragment = new(0.0f, 0.0f, 0.0f);
                    for (int i = 0; i < SampleNum; i++) {
                        Ray ray = GetRay(x, y);
                        fragment += RayCast(ray, world, MaxDepth);
                    }
                    output.SetPixel(x, y, fragment, SampleNum);
                }
            }
            output.SaveFile(filename);
            Console.WriteLine("Done");
        }
        public void RenderParallel(Scene world, string filename) {
            Console.WriteLine($"Size : {ImageWidth} x {ImageHeight} ");
            Console.WriteLine($"Sample : {SampleNum}");
            Console.WriteLine($"Depth : {MaxDepth}");
            RawImage output = new(ImageWidth, ImageHeight);
            Parallel.For(0, ImageHeight, y => {
                for (int x = 0; x < ImageWidth; x++) {
                    Color fragment = new(0.0f, 0.0f, 0.0f);
                    for (int i = 0; i < SampleNum; i++) {
                        Ray ray = GetRay(x, y);
                        fragment += RayCast(ray, world, MaxDepth);
                    }
                    output.SetPixel(x, y, fragment, SampleNum);
                }
            });

            output.SaveFile(filename);
            Console.WriteLine("Done");
        }
        public void Initialize() {
            ImageHeight = (int)(ImageWidth / AspectRatio);
            ImageHeight = (ImageHeight < 1) ? 1 : ImageHeight;
            Position = LookFrom;
            // Camera
            float FocalLength = (LookFrom - LookAt).Length();
            float theta = FOV * DEG2RAD;
            float h = MathF.Tan(theta / 2);
            //  float h = MathF.Tan(theta / 2); YFLIP
            float ViewportHeight = 2.0f * h * FocalLength;
            float ViewportWidth = ViewportHeight * (ImageWidth / (float)ImageHeight);
            W = Vector3.Normalize(LookFrom - LookAt);
            U = Vector3.Normalize(Vector3.Cross(Vup, W));
            V = Vector3.Cross(W, U);


            Vector3 ViewportU = ViewportWidth * U;
            Vector3 ViewportV = ViewportHeight * -V;
            deltaU = ViewportU / ImageWidth;
            deltaV = ViewportV / ImageHeight;

            Vector3 ViewPortUpperLeft = Position - (FocalLength * W) - ViewportU / 2 - ViewportV / 2;
            Pixel00Loc = ViewPortUpperLeft + 0.5f * (deltaU + deltaV);
        }
        public Color RayCast(Ray ray, Scene world, int depth) {
            HitRecord record = new();
            // error
            Interval interval = new(0.001f, float.MaxValue);

            if (depth <= 0) return Color.None;
            if (world.Hit(ray, interval, ref record)) {
                Color attenuation = new ();
                Ray scattered = new ();
                if (record.Material.Scatter(ray, record, ref attenuation, ref scattered)) {
                    return attenuation * RayCast(scattered, world, depth - 1);
                }
                return Color.None;
            }
            // sky
            Vector3 uniDir = Vector3.Normalize(ray.Direction);
            float a = 0.5f * (uniDir.Y + 1.0f);
            return (1.0f - a) * new Color(1.0f, 1.0f, 1.0f) + a * new Color(0.5f, 0.7f, 1.0f);
        }
        private Ray GetRay(int i, int j) {
            Vector3 pixelCenter = Pixel00Loc + i * deltaU + j * deltaV;
            Vector3 pixelSample = pixelCenter + PixelSampleSquare();
            Vector3 rayDirection = pixelSample - Position;
            // non super-sample for anti-aliasing
            //Vector3 rayDirection = pixelCenter - Position;
            return new(Position, rayDirection);
        }
        private Vector3 PixelSampleSquare() {

            float px = -0.5f + random.NextSingle();
            float py = -0.5f + random.NextSingle();
            return px * deltaU + py * deltaV;
        }
    }
}
