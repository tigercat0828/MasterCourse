using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Chapter3
{
    private static Vector3 topColor = new Vector3(1,1,1);
    private static Vector3 bottomColor = new Vector3(.5f, .7f, 1);
    public static Vector3 RayCast(Ray ray) {
        Vector3 unit_diretion = ray.direction.normalized;
        float t = 0.5f * (1+unit_diretion.y);
        return Vector3.Lerp(topColor, bottomColor, t);  
    }
    [MenuItem("Raytracing/Chapter3")]
    public static void Main() {
        int nx = 1280;
        int ny = 720;

        Vector3 lowerLeftCorner = new Vector3(-2, -1, -1);
        Vector3 horizontal = new Vector3(4, 0, 0);
        Vector3 vertical = new Vector3(0, 2, 0);
        Vector3 origin = Vector3.zero;
        Texture2D tex = ImageHelper.CreateImg(nx, ny);
        for (int j = ny - 1; j >= 0; j--) {
            for (int i = 0; i < nx; i++) {
                float u = i / (float)nx;
                float v = j / (float)ny;
                Ray r = new(origin, lowerLeftCorner + u * horizontal + v * vertical);
                Vector3 color = RayCast(r);
                ImageHelper.SetPixel(tex, i, j, color); 
            }
        }
        ImageHelper.SaveImg(tex, "img/Chapter3.png");
    }
}
