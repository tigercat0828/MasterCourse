using System;
using UnityEngine;
using UnityEngine.UI;

public class RayCamera {
    private Vector3 origin;
    private Vector3 lower_left_corner;
    private Vector3 horizontal;
    private Vector3 vertical;
    public RayCamera() {
        origin = Vector3.zero;
        lower_left_corner = new Vector3(-2, -1, -1);
        horizontal = new Vector3(4, 0, 0);
        vertical = new Vector3(0, 2, 0);
    }
    public RayCamera(Vector3 ori, Vector3 corner, Vector3 hori, Vector3 verti) {
        origin = ori;
        lower_left_corner = corner;
        horizontal = hori;
        vertical = verti;
    }
    public RayCamera(float fov, float aspect) {
        float theta = Mathf.Deg2Rad * fov;
        float halfHeight = Mathf.Tan(theta * 0.5f);
        float halfWidth = fov * halfHeight;
        lower_left_corner = new Vector3(-halfWidth, -halfHeight, -1f);
        horizontal = new Vector3(2 * halfWidth, 0, 0);
        vertical = new Vector3(0, 2 * halfHeight, 0);
        origin = Vector3.zero;

    }
    public Ray GetRay(float u, float v) {
        return new Ray(origin, lower_left_corner + u * horizontal + v * vertical - origin);
    }
}