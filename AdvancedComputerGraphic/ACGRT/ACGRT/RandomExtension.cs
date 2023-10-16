using System.Numerics;

namespace ACGRT;
public static class RandomExtension {

    public static Vector3 RandomVec3(this Random random) {
        return new Vector3(random.NextSingle(), random.NextSingle(), random.NextSingle());
    }
    public static Vector3 RandomVec3(this Random random, float min, float max) {
        float x = random.NextSingle(min, max);
        float y = random.NextSingle(min, max);
        float z = random.NextSingle(min, max);
        return new Vector3(x, y, z);
    }
    public static float NextSingle(this Random random, float min, float max) {
        float t = random.NextSingle();
        return t * (max - min) + min;
    }
    public static Vector3 UnitVector(this Random random) {
        while (true) {
            Vector3 tmp = new(random.NextSingle() * 2.0f - 1.0f, random.NextSingle() * 2.0f - 1.0f, random.NextSingle() * 2.0f - 1.0f);
            if (tmp.LengthSquared() < 1) {
                return Vector3.Normalize(tmp);
            }
        }
    }
    public static Vector3 UnitHemisphere(this Random random, Vector3 normal) {
        Vector3 unit = random.UnitVector();
        if (Vector3.Dot(unit, normal) > 0.0)
            return unit;
        else
            return -unit;
    }
}
