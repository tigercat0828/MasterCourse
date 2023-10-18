using System.Numerics;

namespace ACGRT;

public struct HitRecord {
    public float t;
    public Vector3 HitPoint;
    public Vector3 Normal;
    public bool IsFrontFace;
    public Material Material;
    public void SetNormalFace(Ray ray, Vector3 outwardNormal) {
        IsFrontFace = Vector3.Dot(ray.Direction, outwardNormal) < 0;
        Normal = IsFrontFace ? outwardNormal : -outwardNormal;
    }
}
public interface IHitable {
    public bool Hit(Ray ray, Interval interval, ref HitRecord record);
}
