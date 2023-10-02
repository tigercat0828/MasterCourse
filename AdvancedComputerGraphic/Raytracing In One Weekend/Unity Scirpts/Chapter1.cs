using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Chapter1
{
    [MenuItem("Raytracing/Chapter1")]
    public static void Main() {
        int nx = 1280;
        int ny = 720;
        Texture2D tex = ImageHelper.CreateImg(nx, ny);
        for (int j = ny - 1; j >= 0; --j) {
            for (int i = 0; i < nx; ++i) {
                float r = (float)(i) / (float)(nx);
                float g = (float)(j) / (float)(ny);
                float b = 0.2f;
                ImageHelper.SetPixel(tex, i, j, r, g, b);
            }
        }

        ImageHelper.SaveImg(tex, "img/Chapter1.png");
    }
}
