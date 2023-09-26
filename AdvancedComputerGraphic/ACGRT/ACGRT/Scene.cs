using ACGRT;

public class Scene {
    public List<IHitable> Items = new();

    public void AddItem(IHitable item) {
        Items.Add(item);
    }
    public bool Hit(Ray ray, ref float tMin, ref float tMax, out HitRecord record) {
        // traverse all the item the scene hold
        HitRecord tempRec = new();
        record = tempRec;
        bool hitAny = false;
        float currentCloset = tMax;

        foreach (var item in Items) {
            if (item.Hit(ray, ref tMin, ref currentCloset, out tempRec)) {
                hitAny = true;
                currentCloset = tempRec.t;
                record = tempRec;
            }
        }
        return hitAny;
    }
};
