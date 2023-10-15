namespace ACGRT;
public struct Color {
    public float R;
    public float G;
    public float B;
    public Color(float r, float g, float b) {
        R = r;
        G = g;
        B = b;
    }
    public static Color operator +(Color lhs, Color rhs) {
        return new Color(lhs.R + rhs.R, lhs.G + rhs.G, lhs.B + rhs.B);
    }
    public static Color operator *(float scalar, Color color) {
        return new Color(scalar * color.R, scalar * color.G, scalar * color.B);
    }
}
/*
 Default Scene
int WIDTH = 1280;
int HEIGHT = 720;
RawImage raw = new RawImage(WIDTH, HEIGHT);

Scene scene = new Scene();
Camera eye = new Camera();
eye.SetOrigin(Vector3.Zero);
eye.Configure(
        new Vector3(-2.0f, -1.0f, -1.0f),  // lowerleftcorner
        new Vector3(4.0f, 0.0f, 0.0f),     // horizontal
        new Vector3(0.0f, 2.0f, 0.0f)      // vertical
);
scene.AddItem(new Sphere(new Vector3(0, 0, -1), 0.5f));
scene.AddItem(new Sphere(new Vector3(0, -100.5f, -1), 100));
 */