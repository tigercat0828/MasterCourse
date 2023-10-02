using UnityEngine;

public interface IHitable {
    bool Hit(Ray ray, ref float t_min, ref float t_max, out Hit_record record);
};
