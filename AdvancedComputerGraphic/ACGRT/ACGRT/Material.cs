using System.Net;
using System.Numerics;

namespace ACGRT;
public class Material {
    public virtual bool Scatter(Ray ray, HitRecord record, out Color attenuation, out Ray scattered) {
        attenuation = Color.None;
        scattered = new Ray();
        return true;
    }
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
        // ambient
        // diffuse
        // spcular



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

        Vector3 lightPosition = new (5, 5, -5);
        Vector3 lightDirection = lightPosition - record.HitPoint;
        Color lightColor = new (.1f, .1f, .1f);
        Vector3 CameraPos = new (0, 0, -1);
        Vector3 Normal = Vector3.Normalize(record.Normal);
        // ambient
        Color ambient = lightColor * Albedo;

        // diffuse
        float diffStrength = MathF.Max(Vector3.Dot(Normal, lightPosition), 0);
        Color diffuse = diffStrength * lightColor * Albedo ;
        // specular
        Vector3 viewDir = Vector3.Normalize(CameraPos - record.HitPoint);
        Vector3 lightReflectionDir = Vector3.Reflect(-lightDirection, Normal);
        float specStrength = MathF.Pow(MathF.Max(Vector3.Dot(viewDir, lightReflectionDir), 0), Exponent);
        Color specular = specStrength * lightColor;

        Vector3 scatterDir = Vector3.Reflect(Vector3.Normalize(ray.Direction), record.Normal);

        scattered = new Ray(record.HitPoint, scatterDir);
        attenuation = (Ka * ambient + Kd * diffuse + Ks * Reflexive * specular) ;
        return true;
    }

    public override string ToString() {
        return $"{Albedo}, {Ka}, {Kd}, {Ks}, {Exponent}, {Reflexive}";
    }
}
