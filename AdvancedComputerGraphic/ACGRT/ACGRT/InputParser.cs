using System.Numerics;

namespace ACGRT;
public static class InputParser {
    public static (Cam, Scene) Parse(string filename, out int Width, out int Height) {
        Width = 0; Height = 0;

        Scene scene = new Scene();
        Cam camera = new Cam();

        // Parse the input lines
        string[] textLines = File.ReadAllLines(filename);
        foreach (string line in textLines) {
            string[] tokens = line.Split(' ');
            if (tokens.Length < 2) {
                Console.WriteLine("Invalid input line: " + line);
                continue;
            }
            string type = tokens[0];

            switch (type) {

                case "S":
                    Sphere sphere = new Sphere(
                        new Vector3(
                            float.Parse(tokens[1]),
                            float.Parse(tokens[2]),
                            float.Parse(tokens[3])
                        ),
                        float.Parse(tokens[4])
                    );
                    Console.WriteLine($"S {sphere.Center}, {sphere.Radius}");
                    scene.Items.Add(sphere);
                    break;
                case "T":
                    Triangle triangle = new Triangle(
                        new Vector3(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3])),
                        new Vector3(float.Parse(tokens[4]), float.Parse(tokens[5]), float.Parse(tokens[6])),
                        new Vector3(float.Parse(tokens[7]), float.Parse(tokens[8]), float.Parse(tokens[9]))
                    );
                    Console.WriteLine($"T {triangle.pos1}, {triangle.pos2}, {triangle.pos3}");
                    scene.Items.Add(triangle);
                    break;
                case "E":
                    camera.SetOrigin(new Vector3(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3])));
                    Console.WriteLine($"E {camera.origin}");
                    break;
                case "O":
                    Vector3 UL = new Vector3(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3]));
                    Vector3 UR = new Vector3(float.Parse(tokens[4]), float.Parse(tokens[5]), float.Parse(tokens[6]));
                    Vector3 LL = new Vector3(float.Parse(tokens[7]), float.Parse(tokens[8]), float.Parse(tokens[9]));
                    Vector3 LR = new Vector3(float.Parse(tokens[10]), float.Parse(tokens[11]), float.Parse(tokens[12]));
                    camera.Configure(UL, UR, LL, LR);
                    Console.WriteLine($"O {UL}, {UR}, {LL}, {LR}");
                    break;
                case "R":
                    Width = int.Parse(tokens[1]);
                    Height = int.Parse(tokens[2]);

                    Console.WriteLine($"R {Width} x {Height}");
                    break;
                default:
                    Console.WriteLine("Unknown input type: " + type);
                    break;
            }

        }
        // add viewport to raycam
        Console.WriteLine("Parse Complete");
        return (camera, scene);
    }
}
