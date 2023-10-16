using ACGRT;
using System.Diagnostics;
using System.Numerics;

public class Program {
    // Sky
    const int B = 0, G = 8, R = 16, A = 24;
    private static void Main(string[] args) {
        Scene world = new();
        world.AddItem(new Sphere(new Vector3(0, 0, -1), 0.5f));
        world.AddItem(new Sphere(new Vector3(0, -100.5f, -1), 100f));
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
