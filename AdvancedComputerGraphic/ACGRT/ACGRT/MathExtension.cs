using System.Numerics;

namespace ACGRT {
    public static class MathExtension {
        public static bool NearZero(this Vector3 vec) {
            float error = 1e-8f;
            return MathF.Abs(vec.X) < error && MathF.Abs(vec.Y) < error && MathF.Abs(vec.Z) < error;
        }
    }
}
