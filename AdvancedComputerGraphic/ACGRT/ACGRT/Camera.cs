using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
namespace ACGRT;
public class Camera {
    public Vector3 origin;
    private Vector3 LLC;
    public Viewport viewport;
    public Camera(Vector3 position) {
        this.origin = position;
    }


}
