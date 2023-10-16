using System.Numerics;
namespace ACGRT;
public class Triangle : IHitable {
    public Vector3 pos1 { get; }
    public Vector3 pos2 { get; }
    public Vector3 pos3 { get; }
    public Vector3 normal { get; private set; }
    public float area2;
    private const float ERROR = 0.0001f;
    public Triangle(Vector3 pos1, Vector3 pos2, Vector3 pos3) {

        this.pos1 = pos1;
        this.pos2 = pos2;
        this.pos3 = pos3;
        // pre-computed
        Vector3 v0_v1 = pos3 - pos1;
        Vector3 v0_v2 = pos3 - pos2;
        normal = Vector3.Cross(v0_v2, v0_v1);
        area2 = normal.Length();
    }

    public bool Hit(Ray ray, Interval interval, ref HitRecord record) {
        record = new();

        // if parallel -> return
        float normalDotRay = Vector3.Dot(normal, ray.direction);
        if (MathF.Abs(normalDotRay) < ERROR) return false;

        float d = -Vector3.Dot(normal, pos1);
        float t = -(Vector3.Dot(normal, ray.origin) + d) / normalDotRay;

        // check whether the triangle is behind the eye
        if (t < 0) return false;

        // intersection with the "plane"
        Vector3 P = ray.origin + t * ray.direction;

        // test whether inside
        Vector3 edgeA = pos2 - pos1;
        Vector3 vp0 = P - pos1;
        Vector3 C = Vector3.Cross(edgeA, vp0);
        if (Vector3.Dot(normal, C) < 0) return false;

        Vector3 edgeB = pos3 - pos2;
        Vector3 vp1 = P - pos2;
        C = Vector3.Cross(edgeB, vp1);
        if (Vector3.Dot(normal, C) < 0) return false;

        Vector3 edgeC = pos1 - pos3;
        Vector3 vp2 = P - pos3;
        C = Vector3.Cross(edgeC, vp2);
        if (Vector3.Dot(normal, C) < 0) return false;

        record.HitPoint = P;
        record.t = t;
        record.Normal = normal;
        return true;
    }

    public bool Hit(ref Ray r, float tMin, float tMax, ref HitRecord record) {
        throw new NotImplementedException();
    }
}
public class Sphere : IHitable {
    public Vector3 Center { get; private set; }
    public float Radius { get; private set; }
    public Sphere(Vector3 center, float radius) {
        Center = center;
        Radius = radius;
    }

    public bool Hit(Ray ray, Interval interval, ref HitRecord record) {
        Vector3 oc = ray.origin - Center;
        float a = ray.direction.LengthSquared();
        float halfb = Vector3.Dot(ray.direction, oc);
        float c = oc.LengthSquared() - Radius * Radius;
        float discriminant = halfb * halfb - a * c;
        if (discriminant < 0) return false;
        float sqrtDis = MathF.Sqrt(discriminant);
        float root = (-halfb - sqrtDis) / a;
        if (!interval.Surrounds(root)) {
            root = (-halfb + sqrtDis) / a;
            if (!interval.Surrounds(root)) return false;
        }
        record.t = root;
        record.HitPoint = ray.At(root);
        Vector3 outwardNormal = (record.HitPoint - Center) / Radius;
        record.SetNormalFace(ray, outwardNormal);

        return true;
    }
}
