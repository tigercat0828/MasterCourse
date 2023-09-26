using System.Numerics;
using ACGRT;


public class Program {

    private static void Main(string[] args) {

        int nx = 1280;
        int ny = 720;

        RawImage tex = new(nx, ny);
        for (int j = ny - 1; j >= 0; --j) {
            for (int i = 0; i < nx; ++i) {
                int r = (byte)(i / (float)(nx) * 255);
                int g = (byte)(j / (float)(ny) * 255);
                int b = (byte) 0.2f * 255;
                Vector3 color = new (r, g, b);
                tex.SetPixel(i, j, color);
            }
        }
        tex.FinishEdit();
        tex.SaveFile("chapter1.png");

        Scene scene = InputParser.Parse("./Assets/input.txt");
    }
}