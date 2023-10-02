using ACGRT;
using System;
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
    static int i =0;
    public static Vector3 RayCast(Ray ray, Scene scene) {
        HitRecord rec;
        float min = 0;
        float max = float.MaxValue;
        if (scene.Hit(ray, ref min, ref max, out rec)) {
            Console.WriteLine(i++);
            Random rd = new();
            float rnd1 = (float)rd.NextDouble() * 2.0f - 1.0f;
            float rnd2 = (float)rd.NextDouble() * 2.0f - 1.0f;
            float rnd3 = (float)rd.NextDouble() * 2.0f - 1.0f;
            var target = Vector3.Normalize(rec.normal) + Vector3.Normalize(new Vector3(rnd1, rnd2, rnd3));

            return 0.5f * RayCast(new Ray(rec.hitpoint, target), scene);
        }
        else {
            //return Vector3.Zero;
            return RenderSky(ray);
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