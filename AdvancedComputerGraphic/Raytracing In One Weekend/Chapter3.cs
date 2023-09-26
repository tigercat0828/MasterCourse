using System.Numerics;
using ACGRT;

public class Program {
    static Vector3 topColor = Vector3.One;
    static Vector3 bottomColor = new Vector3(0.5f, 0.7f, 1.0f);
    private static Vector3 RayCast(Ray ray) {
        Vector3 uniDirection = Vector3.Normalize(ray.direction);
        float t = 0.5f * (uniDirection.Y + 1.0f);
        return Vector3.Lerp(topColor, bottomColor, t) * 255;
    }

    private static void Main(string[] args) {

        int nx = 1280;
        int ny = 720;
        RawImage raw = new RawImage(nx, ny);

        Vector3 lower_left_corner = new Vector3(-2.0f, -1.0f, -1.0f);
        Vector3 horizontal = new Vector3(4.0f, 0.0f, 0.0f);
        Vector3 vertical = new Vector3(0.0f, 2.0f, 0.0f);

        Vector3 origin = Vector3.Zero;

        for (int j = 0; j < ny; ++j) {
            for (int i = 0; i < nx; ++i) {
                float u = (float)(i) / (float)(nx);
                float v = (float)(j) / (float)(ny);
                Ray r = new Ray(origin, lower_left_corner + u * horizontal + v * vertical);
                Vector3 color = RayCast(r);
                
                raw.SetPixel(i, j, color);
            }
        }
        raw.FinishEdit();
        raw.FlipY();
        raw.SaveFile("chapter3.png");
    }
}