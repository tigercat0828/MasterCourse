using UnityEditor;
using UnityEngine;

public class Chapter4 {
    private static Vector3 topColor = new Vector3(1, 1, 1);
    private static Vector3 bottomColor = new Vector3(.5f, .7f, 1);
    private static Vector3 SphereColor = new Vector3(0, 0, 1);
    private static int Width = 1440;
    private static int Height = 720;

    public static Vector3 RayCast(Ray ray) {
        if (HitSphere(new Vector3(0,0,-1), 0.5f, ray)) {
            return SphereColor;
        }
        Vector3 unit_diretion = ray.direction.normalized;
        float t = 0.5f * (1 + unit_diretion.y);
        return Vector3.Lerp(topColor, bottomColor, t);
    }
    [MenuItem("Raytracing/Chapter4")]
    public static void Main() {

        Vector3 lowerLeftCorner = new Vector3(-2, -1, -1);
        Vector3 horizontal = new Vector3(4, 0, 0);
        Vector3 vertical = new Vector3(0, 2, 0);
        Vector3 origin = Vector3.zero;
        Texture2D tex = ImageHelper.CreateImg(Width, Height);
        for (int j = Height - 1; j >= 0; j--) {
            for (int i = 0; i < Width; i++) {
                float u = i / (float)Width;
                float v = j / (float)Height;
                Ray r = new(origin, lowerLeftCorner + u * horizontal + v * vertical);

                Vector3 color = RayCast(r);
                ImageHelper.SetPixel(tex, i, j, color);
            }
        }
        ImageHelper.SaveImg(tex, "img/Chapter4.png");
    }
    private static bool HitSphere(Vector3 center, float radius, Ray ray) {
        Vector3 oc = ray.origin - center;   // A-C
        float a = Vector3.Dot(ray.direction, ray.direction);
        float b = Vector3.Dot(ray.direction, oc)*2f;
        float c = Vector3.Dot(oc, oc) - radius * radius;
        float d = b * b - 4 * a * c;
        return d > 0;
    }
}
