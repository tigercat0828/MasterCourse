using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ACGRT; 
public struct HitRecord {
    public float t;
    public Vector3 hitpoint;
    public Vector3 normal;
}
public interface IHitable {
    public bool IsHit(Ray r, ref float t_min, float t_max, out HitRecord record);
}
