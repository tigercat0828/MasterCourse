using ACGRT;
using System.Diagnostics;
using System.Numerics;

public class Program {
    // Sky
    const int B = 0, G = 8, R = 16, A = 24;
    private static void Main(string[] args) {
        Scene world = new();
        Material M_Ground = new Lambertian(new Color(0.8f, 0.8f, 0.0f));
        Material M_CenterBall = new Lambertian(new Color(0.7f, 0.3f, 0.3f));
        Material M_LeftBall = new Metal(new Color(0.8f, 0.8f, 0.8f));
        Material M_RightBall = new Metal(new Color(0.8f, 0.6f, 0.2f));
        world.AddItem(new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), 0.5f, M_LeftBall));
        world.AddItem(new Sphere(new Vector3(0, 0, -1), 0.5f, M_CenterBall));
        world.AddItem(new Sphere(new Vector3(0, -100.5f, -1), 100f, M_Ground));
        world.AddItem(new Sphere(new Vector3(1.0f, 0.0f, -1.0f), 0.5f, M_RightBall));
        Camera cam = new();
        cam.SetAspectRatio(16 / 9.0f);
        cam.SetImageWidth(1920);
        cam.Initialize();


        Stopwatch stopwatch = new();
        stopwatch.Start();

        cam.SetSampleNum(10);
        cam.RenderParallel(world, "output.ppm");

        stopwatch.Stop();

        TimeSpan elapsedTime = stopwatch.Elapsed;
        Console.WriteLine($"Time : {elapsedTime.TotalSeconds:F2}");
        stopwatch.Reset();
        Console.Beep();

    }

}
