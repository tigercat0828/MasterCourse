using ACGRT;
using System.Diagnostics.Tracing;
using System.Numerics;

public class Program {
    // Sky
    const int B = 0, G = 8, R = 16, A = 24;
    private static void Main(string[] args) {
        Scene world = new ();
        world.AddItem(new Sphere(new Vector3(0, 0, -1), 0.5f));
        world.AddItem(new Sphere(new Vector3(0, -100.5f, -1), 100f));
        Camera cam = new() {
            AspectRatio = 16.0f / 9.0f
        };
        cam.SetImageWidth(800);
        cam.Initialize();
        cam.Render(world);
    }

}
