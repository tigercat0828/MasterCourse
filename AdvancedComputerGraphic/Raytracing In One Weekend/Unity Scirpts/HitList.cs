using UnityEngine;
using System.Collections.Generic;

public class HitList : IHitable {
    private List<IHitable> list = new List<IHitable>();

    public HitList() {
    }

    public int GetCount() => list.Count;

    public void Add(IHitable item) {
        list.Add(item);
    }

    public bool Hit(Ray r, ref float t_min, ref float t_max, out Hit_record record) {
        Hit_record temp_rec = new Hit_record();
        record = temp_rec;
        bool hit_anything = false;
        float closest_so_far = t_max;

        for (int i = 0; i < list.Count; ++i) {
            if (list[i].Hit(r, ref t_min, ref closest_so_far, out temp_rec)) {
                hit_anything = true;
                closest_so_far = temp_rec.t;
                record = temp_rec;
            }
        }

        return hit_anything;
    }
}
