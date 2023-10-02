using UnityEngine;

public class Sphere : IHitable {
        public Vector3 center;

        public float radius;

        public Sphere() {
            center = Vector3.zero;
            radius = 1;
        }

        public Sphere(Vector3 c, float r) {
            center = c;
            radius = r;
        }

        public bool Hit(Ray ray, ref float t_min, ref float t_max, out Hit_record record) {
            record = new Hit_record();
            Vector3 oc = ray.origin - center;
            float a = Vector3.Dot(ray.direction, ray.direction);
            float b = Vector3.Dot(oc, ray.direction);
            float c = Vector3.Dot(oc, oc) - radius * radius;
            float d = b * b - a * c;
            if (d > 0) {
                float temp = (-b - Mathf.Sqrt(d)) / (a);
                if (temp < t_max && temp > t_min) {
                    record.t = temp;
                    record.hitpoint = ray.GetPoint(temp);
                    record.normal = (record.hitpoint - center) / radius;
                    return true;
                }
                temp = (-b + Mathf.Sqrt(d)) / (a );
                if (temp < t_max && temp > t_min) {
                    record.t = temp;
                    record.hitpoint = ray.GetPoint(temp);
                    record.normal = (record.hitpoint - center) / radius;
                    return true;
                }
            }
            return false;
        }
    }
