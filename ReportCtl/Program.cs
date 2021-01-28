using System;
using System.IO;
using System.Threading.Tasks;
using AutoReport;
using CommandLine;
using Geolocation;
using ReportCtl;
using YamlDotNet.Serialization;

double EnsureLocation(string hint)
{
    for (var input = ReadLine.Read($"Your {hint}, by dd/mm/ss:");; input = ReadLine.Read($"Your {hint}, by dd/mm/ss:"))
    {
        var result = input?.Split('/') ?? Array.Empty<string>();
        if (result.Length != 3)
            Console.WriteLine("Input error, please try again.");
        var location = 0d;
        var multiply = 1d;
        for (var i = 0; i < 3; ++i)
        {
            if (int.TryParse(result[i], out var val))
            {
                location += val * multiply;
                multiply /= 60;
            }
            else
            {
                Console.WriteLine("Input error, please try again.");
                break;
            }

            if (i == 2)
                return location;
        }
    }
}

string EnsurePassword()
{
    for (var s = ReadLine.ReadPassword("Your password:");; s = ReadLine.ReadPassword("Your password:"))
        if (!string.IsNullOrWhiteSpace(s))
            return s;
}

await Parser.Default.ParseArguments<CommandLineOption>(args).WithParsedAsync(async o =>
{
    var serializer = new SerializerBuilder().Build();
    var deserializer = new DeserializerBuilder().Build();
    if (o.Add)
    {
        Console.WriteLine("使用本程序表明您已经同意自己所填写位置真实");
        var latitude = EnsureLocation("latitude");
        var longitude = EnsureLocation("longitude");
        var password = EnsurePassword();
        Console.WriteLine("请熟知：");
        Console.WriteLine("1.全国中高风险地区");
        Console.WriteLine("2.假期期间做好个人防护");
        Console.WriteLine("3.个人及共同居住人有疫情相关情况的，及时向学校报告");
        await Task.Delay(1000);
        var kd = await GeoExtension.GetKdTreeAsync();
        var result = kd.GetNearestNeighbours(new[] {latitude, longitude}, 1)[0];
        var origin = new Coordinate(latitude, longitude);
        var found = new Coordinate(result.Point[0], result.Point[1]);
        if (GeoCalculator.GetDistance(origin, found, 1, DistanceUnit.Kilometers) > 100)
        {
            Console.WriteLine("Your location is possibly not in China and must be wrong.");
            Environment.Exit(1);
        }

        Console.WriteLine($"You are at {result.Value.Name}.");

        var config = deserializer.Deserialize<AppConfig>(await File.ReadAllTextAsync(StaticConfig.ConfigFile));
        config.Reports[o.UserName] = new ReportInformation
        {
            DeEncryptedPassword = password,
            Longitude = longitude,
            Latitude = latitude
        };

        await File.WriteAllTextAsync(StaticConfig.ConfigFile, serializer.Serialize(config));
        Console.WriteLine("Config added.");
    }
    else if (o.Remove)
    {
        var config = deserializer.Deserialize<AppConfig>(await File.ReadAllTextAsync(StaticConfig.ConfigFile));
        if (!config.Reports.ContainsKey(o.UserName))
        {
            Console.WriteLine("User not found.");
            Environment.Exit(1);
        }

        config.Reports.Remove(o.UserName);
        await File.WriteAllTextAsync(StaticConfig.ConfigFile, serializer.Serialize(config));
        Console.WriteLine("Config removed.");
    }
});