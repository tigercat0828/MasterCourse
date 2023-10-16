using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGRT; 
public struct Interval {
    public float Min;
    public float Max;
    public static readonly Interval Empty = new(float.MaxValue, float.MinValue);
    public static readonly Interval Universe = new(float.MinValue, float.MaxValue);

    public Interval(float min, float max) {
        Min = min;
        Max = max;
    }
    public Interval()
    {
        Min = float.MinValue;
        Max = float.MaxValue;
    }

    public bool Contains(float x) => Min <= x && x <= Max;
    public bool Surrounds(float x) => Min < x && x < Max;
    public float Clamp(float x) {
        if (x < Min) return Min;
        if (x > Max) return Max;
        return x;
    }
}
