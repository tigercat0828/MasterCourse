using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Chapter6 {

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

    [MenuItem("Raytracing/Chapter6")]
    public static void Main() {
        int width = 1440;
        int height = 720;
        int numSample = 64;
        RayCamera camera = new ();
        HitList list = new();
        list.Add(new Sphere(new Vector3(0, 0, -1), 0.5f));
        list.Add(new Sphere(new Vector3(0, -100.5f, -1), 100));

        Texture2D tex = ImageHelper.CreateImg(width, height);

        for (int j = height - 1; j >= 0; j--)
        {
            for (int i = 0; i < width; i++) {
                Vector3 color = Vector3.zero;
                for (int k = 0; k < numSample; k++) {
                    float u = (float)(i + Random.Range(-1, 1)) / (float)width;
                    float v = (float)(j + Random.Range(-1, 1)) / (float)height;

                    Ray ray = camera.GetRay(u, v);
                    color += RayCast(ray, list);
                }
                color = color / (float)numSample;
                ImageHelper.SetPixel(tex, i,j,color);
            }
        }
        ImageHelper.SaveImg(tex, "img/Chapter6.png");
    }
}
