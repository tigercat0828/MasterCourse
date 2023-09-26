using Microsoft.Win32.SafeHandles;
using System.Numerics;

namespace ACGRT; 
public struct Ray {

    public Vector3 origin;
    public Vector3 direction;

    public Ray(Vector3 origin, Vector3 direction) {
        this.origin = origin;
        this.direction = direction;
    }
    public Vector3 GetPoint(float t) {
        return origin + t * direction;
    }
}
