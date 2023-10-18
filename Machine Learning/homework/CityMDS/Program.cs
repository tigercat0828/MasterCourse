
using MathNet.Numerics.Integration;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

var citiesCoords = new Dictionary<City, (double Latitude, double Longitude)>() {
    { City.基隆, (25.12825,   121.740517)     },
    { City.臺北, (25.0329694, 121.5654177)    },
    { City.桃園, (24.9936286, 121.3009798)    },
    { City.新竹, (24.813829,  120.967482)     },
    { City.苗栗, (24.5634503, 120.8190585)    },
    { City.臺中, (24.1477359, 120.6736482)    },
    { City.彰化, (24.0809827, 120.5396557)    },
    { City.雲林, (23.7092037, 120.431335)     },
    { City.嘉義, (23.45889,   120.301019)     },
    { City.台南, (22.9908254, 120.2133239)    },  
    { City.高雄, (22.6272784, 120.3014358)    },
    { City.澎湖, (23.5697335, 119.580432)     },
    { City.屏東, (22.54951,   120.548455)     },
    { City.臺東, (22.7556252, 121.1404535)    },
    { City.花蓮, (23.7092037, 121.4310282)    },
    { City.宜蘭, (24.7021064, 121.7377503)    },
};
foreach (var city in citiesCoords) {
    Console.WriteLine($"{city.Key} 緯度：{city.Value.Latitude, 12}, 經度：{city.Value.Longitude, 12}");
}
int cityNum = Enum.GetValues(typeof(City)).Length;

// Example array of values
double[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

// Number of rows and columns
int rows = 3;
int columns = 3;

// Create a matrix from the array, rows, and columns
var matrix = new DenseMatrix(rows, columns, array);

// Print the created matrix
Console.WriteLine("Created Matrix:");
Console.WriteLine(matrix);

enum City {
    基隆, 臺北, 桃園, 新竹, 苗栗, 臺中, 彰化, 雲林, 嘉義, 台南, 高雄, 澎湖, 屏東, 臺東, 花蓮, 宜蘭,
};
