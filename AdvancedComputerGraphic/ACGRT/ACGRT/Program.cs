using ACGRT;
using System.Diagnostics;
using System.Numerics;

public class Program {
    // Sky
    const int B = 0, G = 8, R = 16, A = 24;
    private static void Main(string[] args) {
        (Camera cam, Scene world) = InputParser.Parse("./Assets/hw2_input.txt");



        Material M_Ground = new Lambertian(new Color(0.8f, 0.8f, 0.0f));
        Material M_CenterBall = new Lambertian(new Color(0.7f, 0.3f, 0.3f));
        Material M_LeftBall = new Metal(new Color(0.8f, 0.8f, 0.8f));
        Material M_RightBall = new Metal(new Color(0.8f, 0.6f, 0.2f));
        world.AddItem(new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), 0.5f, M_LeftBall));
        world.AddItem(new Sphere(new Vector3(0, 0, -1), 0.5f, M_CenterBall));
        world.AddItem(new Sphere(new Vector3(0, -100.5f, -1), 100f, M_Ground));
        world.AddItem(new Sphere(new Vector3(1.0f, 0.0f, -1.0f), 0.5f, M_RightBall));

        Camera camera = new();
        camera.SetAspectRatio(16 / 9.0f);
        camera.SetImageWidth(800);
        camera.SetFOV(50);
        camera.LookFrom = new Vector3(-2, 2, 1);
        camera.LookAt = new Vector3(0, 0, -1);
        camera.Vup = Vector3.UnitY;
        camera.Initialize();


        Stopwatch stopwatch = new();
        stopwatch.Start();

        camera.SetSampleNum(10);
        camera.RenderParallel(world, "output.ppm");

        stopwatch.Stop();

        TimeSpan elapsedTime = stopwatch.Elapsed;
        Console.WriteLine($"Time : {elapsedTime.TotalSeconds:F2}");
        stopwatch.Reset();
        //Console.Beep();

    }

}
