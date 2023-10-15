using System.Numerics;

namespace ACGRT;
public struct Ray {

    public Vector3 origin { get; private set; }
    public Vector3 direction { get; private set; }

    public Ray(Vector3 origin, Vector3 direction) {
        this.origin = origin;
        this.direction = direction;
    }
    public readonly Vector3 At(float t) {
        return origin + t * direction;
    }
}
