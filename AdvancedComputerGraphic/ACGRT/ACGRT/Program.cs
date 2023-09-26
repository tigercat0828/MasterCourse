using System.Numerics;
using ACGRT;

public class Program {
    private static Vector3 topColor = Vector3.One *255;
    private static Vector3 bottomColor = new Vector3(128, 176, 255);
    private static Vector3 ballColor = new Vector3(255*1, 0, 0);

    private static Vector3 center = new Vector3(0, 0, -1);
    private static float radius = 0.5f;

    public static Vector3 RayCast(Ray ray) {
        if (Hit_sphere(center, radius, ray)) {
            return ballColor;
        }
        Vector3 unit_direction = Vector3.Normalize(ray.direction);
        float t = 0.5f * (unit_direction.Y + 1.0f);
        return Vector3.Lerp(topColor, bottomColor, t);
    }
    public static bool Hit_sphere(Vector3 center, float radius, Ray ray) {
        Vector3 oc = ray.origin - center;
        float a = Vector3.Dot(ray.direction, ray.direction);
        float b = 2.0f * Vector3.Dot(oc, ray.direction);
        float c = Vector3.Dot(oc, oc) - radius * radius;
        float d = b * b - 4 * a * c;
        return d > 0;
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
        raw.SaveFile("chapter4.png");
    }
}