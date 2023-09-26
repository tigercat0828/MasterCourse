using System.Numerics;

namespace ACGRT;

public struct HitRecord {
    public float t;
    public Vector3 hitpoint;
    public Vector3 normal;
}
public interface IHitable {
    public bool Hit(Ray r, ref float t_min, ref float t_max, out HitRecord record);
}
