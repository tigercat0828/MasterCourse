using System.Numerics;

public class Viewport {

    public Vector3 positionUL;
    public Vector3 positionUR;
    public Vector3 positionLL;
    public Vector3 positionLR;

    public Viewport(Vector3 positionUL, Vector3 positionUR, Vector3 positionLL, Vector3 positionLR) {
        this.positionUL = positionUL;
        this.positionUR = positionUR;
        this.positionLL = positionLL;
        this.positionLR = positionLR;
    }
}
