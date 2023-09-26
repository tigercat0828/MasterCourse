using System.Numerics;
namespace ACGRT;
public class Primitive {

}
public class Triangle : Primitive {
    public Vector3 position1;
    public Vector3 position2; 
    public Vector3 position3;
    public Triangle(Vector3 position1, Vector3 position2, Vector3 position3) {
        this.position1 = position1;
        this.position2 = position2;
        this.position3 = position3;
    }
}
public class Sphere : Primitive {
    public Vector3 origin;
    public float radius;
    public Sphere( Vector3 origin, float radius) {
        this.origin = origin;
        this.radius = radius;
    }
}
