using System.Numerics;

namespace ACGRT;
public abstract class Material {
    public abstract bool Scatter(Ray ray, HitRecord record, out Color attenuation, out Ray scattered);
}
public class Lambertian : Material {
    static readonly Random Random = new();
    private Color Albedo;

    public Lambertian(Color albedo) {
        Albedo = albedo;
    }

    public override bool Scatter(Ray ray, HitRecord record, out Color attenuation, out Ray scattered) {
        Vector3 ScatterDirection = record.Normal + Random.UnitVector();
        if (ScatterDirection.NearZero()) ScatterDirection = record.Normal;
        scattered = new Ray(record.HitPoint, ScatterDirection);
        attenuation = Albedo;
        return true;
    }
}
public class Metal : Material {
    private Color Albedo;
    public Metal(Color albedo) {
        Albedo = albedo;
    }

    public override bool Scatter(Ray ray, HitRecord record, out Color attenuation, out Ray scattered) {
        Vector3 reflected = Vector3.Reflect(Vector3.Normalize(ray.Direction), record.Normal);
        scattered = new Ray(record.HitPoint, reflected);
        attenuation = Albedo;
        return true;
    }
}
public class PhongMat : Material {
    public Color Albedo { get; set; }
    public float Ka;
    public float Kd;
    public float Ks;
    public float Exponent;
    public float Reflexive;
    // light here maybe
    public PhongMat() {

    }
    public PhongMat(Color albedo, float ka, float kd, float ks, float exponent, float reflexive) {
        Albedo = albedo;
        Ka = ka;
        Kd = kd;
        Ks = ks;
        Exponent = exponent;
        Reflexive = reflexive;

    }

    public override bool Scatter(Ray ray, HitRecord record, out Color attenuation, out Ray scattered) {
        attenuation = Albedo;
        scattered = new Ray();
        return true;
        // phong modeling here
    }

    public override string ToString() {
        return $"{Albedo}, {Ka}, {Kd}, {Ks}, {Exponent}, {Reflexive}";
    }
}
