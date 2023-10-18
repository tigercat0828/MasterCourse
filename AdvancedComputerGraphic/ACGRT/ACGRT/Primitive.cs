using System.Numerics;
namespace ACGRT;
using static MathF;
using static Vector3;
public class Triangle : IHitable {
    public Vector3 pos1;
    public Vector3 pos2;
    public Vector3 pos3;

    public Vector3 Corner;
    public Vector3 U;
    public Vector3 V;
    private Vector3 W;
    public Vector3 Normal;
    private float D;    // D = Ax+By+Cz
    public Material Material;
    //public Triangle(Vector3 corner, Vector3 u, Vector3 v, Material material) {
    //    Corner = corner;
    //    U = u;
    //    V = v;
    //    Material = material;
    //    Vector3 n = Cross(u, v);
    //    Normal = Normalize(n);
    //    D = Dot(Normal, Corner); // Ax+By+Cz = N dot (x,y,z)
    //    W = n / Dot(n, n);
    //}
    public Triangle(Vector3 pos1, Vector3 pos2, Vector3 pos3, Material material) {
        this.pos1 = pos1;
        this.pos2 = pos2;
        this.pos3 = pos3;
        Corner = pos1;
        U = pos2 - pos1;
        V = pos3 - pos1;
        Material = material;
        Vector3 n = Cross(U, V);
        Normal = Normalize(n);
        D = Dot(Normal, Corner); // Ax+By+Cz = N dot (x,y,z)
        W = n / Dot(n, n);
    }

    public bool Hit(Ray ray, Interval interval, ref HitRecord record) {
        float denom = Dot(Normal, ray.Direction);
        if (Abs(denom) < 1e-8f) return false; // parallel to the plane
        float t = (D - Dot(Normal, ray.Origin)) / denom;
        if (!interval.Contains(t)) return false;

        Vector3 intersection = ray.At(t);
        Vector3 PlanarHitVector = intersection - Corner;
        float alpha = Dot(W, Cross(PlanarHitVector, V));
        float beta = Dot(W, Cross(U, PlanarHitVector));

        if (!IsInterior(alpha, beta, ref record)) return false;

        record.t = t;
        record.HitPoint = intersection;
        record.Material = Material;
        record.SetNormalFace(ray, Normal);
        return true;
    }
    bool IsInterior(float a, float b, ref HitRecord rec) {
        // Given the hit point in plane coordinates, return false if it is outside the
        // primitive, otherwise set the hit record UV coordinates and return true.

        if ((a < 0) || (1 < a) || (b < 0) || (1 < b))
            return false;

        if (a + b > 1) return false;
        //rec.u = a;
        //rec.v = b;
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
        float halfb = Dot(ray.Direction, oc);
        float c = oc.LengthSquared() - Radius * Radius;
        float discriminant = halfb * halfb - a * c;
        if (discriminant < 0) return false;
        float sqrtDis = Sqrt(discriminant);
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
    public override string ToString() {
        return $"({Center}, {Radius})";
    }
}

public class Quad : IHitable {
    public Vector3 Corner;
    public Vector3 U;
    public Vector3 V;
    private Vector3 W;
    public Vector3 Normal;
    private float D;    // D = Ax+By+Cz
    public Material Material;

    public Quad(Vector3 corner, Vector3 u, Vector3 v, Material material) {
        Corner = corner;
        U = u;
        V = v;
        Material = material;
        Vector3 n = Cross(u, v);
        Normal = Normalize(n);
        D = Dot(Normal, Corner); // Ax+By+Cz = N dot (x,y,z)
        W = n / Dot(n, n);
    }

    public bool Hit(Ray ray, Interval interval, ref HitRecord record) {
        float denom = Dot(Normal, ray.Direction);
        if (Abs(denom) < 1e-8f) return false; // parallel to the plane
        float t = (D - Dot(Normal, ray.Origin)) / denom;
        if (!interval.Contains(t)) return false;

        Vector3 intersection = ray.At(t);
        Vector3 PlanarHitVector = intersection - Corner;
        float alpha = Dot(W, Cross(PlanarHitVector, V));
        float beta = Dot(W, Cross(U, PlanarHitVector));

        if (!IsInterior(alpha, beta, ref record)) return false;

        record.t = t;
        record.HitPoint = intersection;
        record.Material = Material;
        record.SetNormalFace(ray, Normal);
        return true;
    }
    bool IsInterior(float a, float b, ref HitRecord rec) {
        // Given the hit point in plane coordinates, return false if it is outside the
        // primitive, otherwise set the hit record UV coordinates and return true.

        if ((a < 0) || (1 < a) || (b < 0) || (1 < b))
            return false;
        //rec.u = a;
        //rec.v = b;
        return true;
    }
}
