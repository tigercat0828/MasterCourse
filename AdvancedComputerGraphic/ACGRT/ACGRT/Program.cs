using ACGRT;
using System.Numerics;

public class Program {
    // Sky
    const int B = 0, G = 8, R = 16, A = 24;
    private static void Main(string[] args) {

        // Image
        float AspectRatio = 16.0f/ 9.0f;
        int WIDTH = 400;
        int HEIGHT = (int)(WIDTH / AspectRatio);
        // Camera
        float FocalLength = 1.0f;
        float ViewportHeight = 2.0f;
        float ViewportWidth =  ViewportHeight *  WIDTH / HEIGHT;
        Vector3 CameraPosition = Vector3.Zero;
        Vector3 ViewportU = new(ViewportWidth, 0, 0);
        Vector3 ViewportV = new(0, -ViewportHeight, 0);
        Vector3 deltaU = ViewportU / WIDTH;
        Vector3 deltaV = ViewportV / HEIGHT;

        Vector3 ViewPortUpperLeft = CameraPosition - new Vector3(0, 0, FocalLength) - ViewportU / 2 - ViewportV / 2;
        Vector3 Pixel00Loc = ViewPortUpperLeft + 0.5f * (deltaU + deltaV);

        Console.WriteLine($"{WIDTH} x {HEIGHT}");
        RawImage output = new(WIDTH, HEIGHT);
        for (int y = 0; y < HEIGHT; y++) {
            Console.WriteLine($"Scanline {y,4} ...");
            for (int x = 0; x < WIDTH; x++) {
                Vector3 currentPixelPosition = Pixel00Loc + x * deltaU + y * deltaV;
                Vector3 rayDirection = currentPixelPosition - CameraPosition;
                Ray r = new(CameraPosition, rayDirection);
                Color pixel = 255*RayCast(r);
                output.SetPixel(x, y,pixel);
            }
        }
        output.SaveFile("Hello.png");
        Console.WriteLine("Done");
    }
    private static Color RayCast(Ray r) {
        if(HitSphere(new Vector3(0,0,-1.0f), 0.5f, r)){
            return new Color(1, 0, 0);
        }
        return new Color(1,1,1);
        Vector3 uniDir = Vector3.Normalize(r.direction);
        float a = 0.5f * (uniDir.Y + 1.0f);
        return (1.0f - a) * new Color(1.0f, 1.0f, 1.0f) + a * new Color(0.5f, 0.7f, 1.0f);
    }
    static bool HitSphere(Vector3 center, float radius, Ray r) {
        Vector3 oc = r.origin - center;
        float a = Vector3.Dot(r.direction, r.direction);
        float b =2.0f* Vector3.Dot(r.direction, oc);
        float c = Vector3.Dot(oc, oc) - radius * radius;
        float discriminant = b*b-4*a*c;
        return discriminant >= 0;
    }
}
/*
 Default Scene
int WIDTH = 1280;
int HEIGHT = 720;
RawImage raw = new RawImage(WIDTH, HEIGHT);

Scene scene = new Scene();
Camera eye = new Camera();
eye.SetOrigin(Vector3.Zero);
eye.Configure(
        new Vector3(-2.0f, -1.0f, -1.0f),  // lowerleftcorner
        new Vector3(4.0f, 0.0f, 0.0f),     // horizontal
        new Vector3(0.0f, 2.0f, 0.0f)      // vertical
);
scene.AddItem(new Sphere(new Vector3(0, 0, -1), 0.5f));
scene.AddItem(new Sphere(new Vector3(0, -100.5f, -1), 100));
 */