using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ACGRT {
    public abstract class Material {
        public abstract bool Scatter(Ray ray, HitRecord record, out Color attenuation, out Ray scattered );
    }
    public class Lambertian : Material {
        static Random Random = new Random();
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
}
