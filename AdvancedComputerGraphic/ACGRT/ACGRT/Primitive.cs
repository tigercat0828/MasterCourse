using System.Numerics;
namespace ACGRT;
public class Triangle : IHitable {
    public Vector3 pos1 { get; }
    public Vector3 pos2 { get; }
    public Vector3 pos3 { get; }
    public Vector3 normal { get; private set; }
    public Material Material { get; private set; }
    public float area2;
    private const float ERROR = 0.0001f;
    public Triangle(Vector3 pos1, Vector3 pos2, Vector3 pos3, Material material) {

        Material = material;
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
    
        // if parallel -> return
        float normalDotRay = Vector3.Dot(normal, ray.Direction);
        if (MathF.Abs(normalDotRay) < ERROR) return false;

        float d = -Vector3.Dot(normal, pos1);
        float t = -(Vector3.Dot(normal, ray.Origin) + d) / normalDotRay;

        // check whether the triangle is behind the eye
        if (t < 0) return false;

        // intersection with the "plane"
        Vector3 P = ray.Origin + t * ray.Direction;

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
        record.Material = Material;
        return true;
    }

}
public class Sphere : IHitable {
    public Vector3 Center { get; private set; }
    public float Radius { get; private set; }
    public Material Material { get; set; }
    public Sphere(Vector3 center, float radius, Material material) {
        Center = center;
        Radius = radius;
        Material = material;
    }

    public bool Hit(Ray ray, Interval interval, ref HitRecord record) {
        Vector3 oc = ray.Origin - Center;
        float a = ray.Direction.LengthSquared();
        float halfb = Vector3.Dot(ray.Direction, oc);
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
        record.Material = Material;
        record.SetNormalFace(ray, outwardNormal);

        return true;
    }
}

public class Quad : IHitable {
    public Vector3 Corner;
    public Vector3 U;
    public Vector3 V;
    public Material Material;

    public Quad(Vector3 corner, Vector3 u, Vector3 v, Material material) {
        Corner = corner;
        U = u;
        V = v;
        Material = material;
    }

    public bool Hit(Ray ray, Interval interval, ref HitRecord record) {
        throw new NotImplementedException();
    }
}
 