using ACGRT;
using System.Numerics;

public class Program {
    // Sky
    const int B = 0, G = 8, R = 16, A = 24;
    private static void Main(string[] args) {
        PickScene(1);
    }
    static void PickScene(int sceneID) {
        Camera cam = new();
        switch (sceneID) {
            case 1: // parse input
                (cam, Scene world) = InputParser.Parse("./Assets/hw2_input.txt");
                cam.Initialize();
                cam.Render(world, "hw_output.png");
                break;
            case 2: // Quads
                Scene QuadScene = new();
                Material MRed = new Lambertian(Color.Red);
                Material MBlue = new Lambertian(Color.Blue);
                Material MYellow = new Lambertian(Color.Yellow);
                Material MGreen = new Lambertian(Color.Green);
                Material MCyan = new Lambertian(Color.Cyan);
                QuadScene.AddItem(new Quad(new(-3, -2, 5), new(0, 0, -4), new(0, 4, 0), MRed));
                QuadScene.AddItem(new Quad(new(-2, -2, 0), new(4, 0, 0), new(0, 4, 0), MGreen));
                QuadScene.AddItem(new Quad(new(3, -2, 1), new(0, 0, 4), new(0, 4, 0), MBlue));
                QuadScene.AddItem(new Quad(new(-2, 3, 1), new(4, 0, 0), new(0, 0, 4), MYellow));
                QuadScene.AddItem(new Quad(new(-2, -3, 5), new(4, 0, 0), new(0, 0, -4), MCyan));

                cam.SetAspectRatio(1.0f);
                cam.SetImageWidth(800);
                cam.SetSampleNum(100);
                cam.SetMaxDepth(10);
                cam.SetFOV(80f);
                cam.LookFrom = new(0, 0, 9);
                cam.LookAt = new(0, 0, 0);
                cam.Vup = new(0, 1, 0);
                cam.Initialize();
                cam.RenderParallel(QuadScene, "Quad.ppm");
                Console.WriteLine("Quad Scene");
                break;
            case 3:
                Scene sphere3Scene = new();
                Material M_Ground = new Lambertian(new Color(0.8f, 0.8f, 0.0f));
                Material M_CenterBall = new Lambertian(new Color(0.7f, 0.3f, 0.3f));
                Material M_LeftBall = new Metal(new Color(0.8f, 0.8f, 0.8f));
                Material M_RightBall = new Metal(new Color(0.8f, 0.6f, 0.2f));
                sphere3Scene.AddItem(new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), 0.5f, M_LeftBall));
                sphere3Scene.AddItem(new Sphere(new Vector3(0, 0, -1), 0.5f, M_CenterBall));
                sphere3Scene.AddItem(new Sphere(new Vector3(0, -100.5f, -1), 100f, M_Ground));
                sphere3Scene.AddItem(new Sphere(new Vector3(1.0f, 0.0f, -1.0f), 0.5f, M_RightBall));
                cam.SetAspectRatio(16 / 9.0f);
                cam.SetImageWidth(800);
                cam.SetFOV(50);
                cam.LookFrom = new Vector3(-2, 2, 1);
                cam.LookAt = new Vector3(0, 0, -1);
                cam.Vup = Vector3.UnitY;
                cam.SetSampleNum(10);
                cam.Initialize();

                cam.RenderParallel(sphere3Scene, "Sphere.ppm");
                Console.Beep();
                break;
            default:
                break;
        }
    }
}
