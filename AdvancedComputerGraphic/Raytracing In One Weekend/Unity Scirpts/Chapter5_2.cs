using UnityEngine;
using UnityEditor;

public class Chapter5_2 {

    private static Vector3 topColor = Vector3.one;
    private static Vector3 bottomColor = new Vector3(0.5f, 0.7f, 1.0f);

    public static Vector3 RayCast(Ray ray, IHitable world) {
        Hit_record rec;
        float min = 0;
        float max = float.MaxValue;
        if (world.Hit(ray, ref min, ref max, out rec)) {
            return 0.5f * (rec.normal.normalized + Vector3.one);
        }
        else {
            Vector3 unit_direction = ray.direction.normalized;
            float t = 0.5f * (unit_direction.y + 1.0f);
            return Vector3.Lerp(topColor, bottomColor, t);
        }
    }

    [MenuItem("Raytracing/Chapter5_2")]
    public static void Main() {
        int nx = 1440;
        int ny = 720;

        Vector3 lower_left_corner = new Vector3(-2.0f, -1.0f, -1.0f);
        Vector3 horizontal = new Vector3(4.0f, 0.0f, 0.0f);
        Vector3 vertical = new Vector3(0.0f, 2.0f, 0.0f);
        Vector3 origin = Vector3.zero;

        HitList list = new HitList();
        list.Add(new Sphere(new Vector3(0, 0, -1), 0.5f));
        list.Add(new Sphere(new Vector3(0, -100.5f, -1), 100));



        Texture2D tex = ImageHelper.CreateImg(nx, ny);
        for (int j = ny - 1; j >= 0; --j) {
            for (int i = 0; i < nx; ++i) {


                float u = (float)(i) / (float)(nx);
                float v = (float)(j) / (float)(ny);

                Ray r = new Ray(origin, lower_left_corner + u * horizontal + v * vertical);
                Vector3 color = RayCast(r, list);

                ImageHelper.SetPixel(tex, i, j, color);
            }
        }

        ImageHelper.SaveImg(tex, "Img/Chapter5_2.png");
    }
}
