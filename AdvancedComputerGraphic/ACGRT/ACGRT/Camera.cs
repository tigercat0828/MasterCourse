using System.Numerics;
namespace ACGRT;
public class Camera {
    public Vector3 origin { get; private set; }
    public Vector3 lowerLeftCorner { get; private set; }
    public Vector3 horizontal { get; private set; }
    public Vector3 vertical { get; private set; }
    public Camera() {

    }
    public void Configure(Vector3 UL, Vector3 UR, Vector3 LL, Vector3 LR) {
        lowerLeftCorner = LL;
        horizontal = LR - LL;
        vertical = UL - LL;
    }
    public void Configure(Vector3 lowerLeftCorner, Vector3 horizontal, Vector3 vertical) {
        this.lowerLeftCorner = lowerLeftCorner;
        this.horizontal = horizontal;
        this.vertical = vertical;
    }
    public void SetOrigin(Vector3 origin) {
        this.origin = origin;
    }
    public Ray GetRay(float u, float v) {
        return new Ray(origin, lowerLeftCorner + u * horizontal + v * vertical - origin);
    }

}
