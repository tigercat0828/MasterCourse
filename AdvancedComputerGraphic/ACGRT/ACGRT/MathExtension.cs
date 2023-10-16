using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ACGRT {
    public static class MathExtension {
        public static bool NearZero(this Vector3 vec) {
            float error = 1e-8f;
            return MathF.Abs(vec.X) < error && MathF.Abs(vec.Y) < error && MathF.Abs(vec.Z) < error;
        }
    }
}
