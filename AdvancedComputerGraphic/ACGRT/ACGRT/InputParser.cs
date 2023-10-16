using System.Numerics;

namespace ACGRT;
public static class InputParser {
    public static (Camera, Scene) Parse(string filename) {

        Scene scene = new();
        Camera camera = new();
        Material currentMaterial = new PhongMat();
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
                case "M":
                    currentMaterial = new PhongMat(
                        new Color(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3])),
                        float.Parse(tokens[4]), float.Parse(tokens[5]), float.Parse(tokens[6]),
                        float.Parse(tokens[7]), float.Parse(tokens[8]));

                    break;
                case "S":
                    Sphere sphere = new Sphere(
                        new Vector3(
                            float.Parse(tokens[1]),
                            float.Parse(tokens[2]),
                            float.Parse(tokens[3])
                        ),
                        float.Parse(tokens[4]),
                        currentMaterial
                    );
                    Console.WriteLine($"S {sphere.Center}, {sphere.Radius}");
                    scene.Items.Add(sphere);
                    break;
                case "T":
                    Triangle triangle = new(
                        new Vector3(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3])),
                        new Vector3(float.Parse(tokens[4]), float.Parse(tokens[5]), float.Parse(tokens[6])),
                        new Vector3(float.Parse(tokens[7]), float.Parse(tokens[8]), float.Parse(tokens[9])),
                        currentMaterial
                    );
                    Console.WriteLine($"T {triangle.pos1}, {triangle.pos2}, {triangle.pos3}");
                    scene.Items.Add(triangle);
                    break;
                case "E":
                    camera.SetPosition(new Vector3(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3])));
                    Console.WriteLine($"E {camera.Position}");
                    break;
                case "F":
                    camera.SetFOV(float.Parse(tokens[1]));
                    Console.WriteLine($"FOV = {camera.vFOV}");
                    break;
                case "R":
                    int Width = int.Parse(tokens[1]);
                    int Height = int.Parse(tokens[2]);
                    camera.SetImageSize(Width, Height);
                    Console.WriteLine($"R {camera.ImageWidth} x {camera.ImageHeight}");
                    break;
                case "V":
                    Vector3 viewDirection = new Vector3(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3]));
                    Vector3 cameraUp = new Vector3(float.Parse(tokens[4]), float.Parse(tokens[5]), float.Parse(tokens[6]));
                    camera.LookAt = camera.Position + viewDirection;
                    // up as default
                    break;
                case "L":
                    Vector3 lightPos = new(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3]));
                    Console.WriteLine(lightPos);
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
