using ACGRT;

public class Scene {
    public List<IHitable> Items = new();

    public void AddItem(IHitable item) {
        Items.Add(item);
    }
    public bool Hit(Ray ray, Interval interval, ref HitRecord record) {
        // traverse all the item the scene hold
        HitRecord tempRec = new();
        bool hitAny = false;
        float currentCloset = interval.Max;

        foreach (var item in Items) {
            if (item.Hit(ray, new Interval(interval.Min, currentCloset), ref tempRec)) {
                hitAny = true;
                currentCloset = tempRec.t;
                record = tempRec;
            }
        }
        return hitAny;
    }
};
