using ACGRT;
using System.Numerics;

public class Program {
    // Sky
    public static Vector3 RenderSky(Ray ray) {
        Vector3 topColor = Vector3.One * 255;
        Vector3 bottomColor = new Vector3(0.5f, 0.7f, 1.0f) * 255;
        // not hit so render sky
        Vector3 uniDirection = Vector3.Normalize(ray.direction);
        float t = 0.5f * (uniDirection.Y + 1.0f);
        return Vector3.Lerp(topColor, bottomColor, t);
    }
    public static Vector3 RayCast(Ray ray, Scene scene) {
        HitRecord rec;
        float min = 0;
        float max = float.MaxValue;
        if (scene.Hit(ray, ref min, ref max, out rec)) {
            //R=255*(x+1.0)/2.0, G=255*(y+1.0)/2.0, B=255*(z+1.0)/2.0
            Vector3 hp = rec.hitpoint;
            float R = 255 * (hp.X + 1.0f) / 2.0f;
            float G = 255 * (hp.Y + 1.0f) / 2.0f;
            float B = 255 * (hp.Z + 1.0f) / 2.0f;
            return new Vector3(R, G, B);
        }
        else {
            return Vector3.Zero;
            //return RenderSky(ray);
        }
    }
    // ----------------------------------
    private static void Main(string[] args) {


        #region Scene Setup
        (Camera eye, Scene scene) = InputParser.Parse("./Assets/hw1_input.txt", out int WIDTH, out int HEIGHT);

        RawImage raw = new RawImage(WIDTH, HEIGHT);

        #endregion
        for (int i = 0; i < WIDTH; i++) {
            for (int j = 0; j <HEIGHT; j++) {
                float u = i / (float)(WIDTH);
                float v = j / (float)(HEIGHT);
                Ray r = eye.GetRay(u, v);
                Vector3 color = RayCast(r, scene);
                raw.SetPixel(i, j, color);
            }
        }
        raw.Update();

        Console.WriteLine("Rendering Complete");

        Console.WriteLine("Write File ...");

        raw.FlipY();
        raw.WritePPM("hw1_output.ppm");
        raw.SaveFile("hw1_output.png");

        Console.WriteLine("Done");
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