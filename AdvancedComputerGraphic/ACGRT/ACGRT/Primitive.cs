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
        normal = Vector3.Cross(v0_v1, v0_v2);
        area2 = normal.Length();
    }

    public bool Hit(Ray ray, ref float t_min, ref float t_max, out HitRecord record) {
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

        record.hitpoint = P;
        record.t = t;
        record.normal = normal;
        return true; 
    }
}
public class Sphere : IHitable {
    public Vector3 origin;
    public float radius;
    public Sphere(Vector3 origin, float radius) {
        this.origin = origin;
        this.radius = radius;
    }
    public bool Hit(Ray ray, ref float tMin, ref float tMax, out HitRecord record) {
        record = new HitRecord();
        Vector3 oc = ray.origin - this.origin;
        float a = Vector3.Dot(ray.direction, ray.direction);
        float b = Vector3.Dot(oc, ray.direction);
        float c = Vector3.Dot(oc, oc) - radius * radius;
        float d = b * b - a * c;
        // 0, 1, 2 roots
        if (d > 0) {

            float temp = (-b - MathF.Sqrt(d)) / a;
            if (temp < tMax && temp > tMin) {
                record.t = temp;
                record.hitpoint = ray.GetPoint(temp);
                record.normal = (record.hitpoint - this.origin) / radius;
                return true;
            }
            temp = (-b + MathF.Sqrt(d)) / a;
            if (temp < tMax && temp > tMin) {
                record.t = temp;
                record.hitpoint = ray.GetPoint(temp);
                record.normal = (record.hitpoint - this.origin) / radius;
                return true;
            }
        }
        return false;
    }
}
