using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ACGRT; 
public static class InputParser {
    public static Scene Parse(string filename) {
        Scene scene = new Scene();
        
        // Parse the input lines
        string[] lines = File.ReadAllLines(filename);

        foreach (string line in lines) {
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
                    Console.WriteLine($"S {sphere.origin}, {sphere.radius}");
                    scene.Objects.Add(sphere);
                    break;
                case "T":
                    Triangle triangle = new Triangle(
                        new Vector3(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3])),
                        new Vector3(float.Parse(tokens[4]), float.Parse(tokens[5]), float.Parse(tokens[6])),
                        new Vector3(float.Parse(tokens[7]), float.Parse(tokens[8]), float.Parse(tokens[9]))
                    );
                    Console.WriteLine($"T {triangle.position1}, {triangle.position2}, {triangle.position3}");
                    scene.Objects.Add(triangle);
                    break;
                case "E":
                    scene.MainCam = new Camera(new Vector3(
                            float.Parse(tokens[1]),
                            float.Parse(tokens[2]),
                            float.Parse(tokens[3])
                        )
                    );
                    Console.WriteLine($"E {scene.MainCam.origin}");
                    break;
                case "O":
                    scene.Viewport = new Viewport(
                        new Vector3(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3])),
                        new Vector3(float.Parse(tokens[4]), float.Parse(tokens[5]), float.Parse(tokens[6])),
                        new Vector3(float.Parse(tokens[7]), float.Parse(tokens[8]), float.Parse(tokens[9])),
                        new Vector3(float.Parse(tokens[10]), float.Parse(tokens[11]), float.Parse(tokens[12]))
                    );
                    Console.WriteLine($"O {scene.Viewport.positionUL}, {scene.Viewport.positionUR}, {scene.Viewport.positionLL}, {scene.Viewport.positionLR}");
                    break;
                case "R":
                    scene.WIDTH = int.Parse(tokens[1]);
                    scene.HEIGHT = int.Parse(tokens[2]);
                    scene.Output = new RawImage(scene.WIDTH, scene.HEIGHT);
                    Console.WriteLine($"R {scene.WIDTH} x {scene.HEIGHT}");
                    break;
                default:
                    Console.WriteLine("Unknown input type: " + type);
                    break;
            }
        }

        // add viewport to raycam
        Console.WriteLine("Parse Finish ...");
        return scene;
    }
}
