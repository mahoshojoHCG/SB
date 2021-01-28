using System.IO;
using System.Text;
using System.Threading.Tasks;
using KdTree;
using KdTree.Math;
using Microsoft.Extensions.DependencyInjection;

namespace AutoReport
{
    public static class GeoExtension
    {
        public static async Task<KdTree<double, GeoInfo>> GetKdTreeAsync()
        {
            var kd = new KdTree<double, GeoInfo>(2, new DoubleMath());
            using var database = new StreamReader("CN.txt", Encoding.UTF8);
            for (var line = await database.ReadLineAsync(); line != null; line = await database.ReadLineAsync())
            {
                var content = line.Split('\t');
                if (content[6] != "A")
                    continue;
                var geo = new GeoInfo
                {
                    GeoNameId = int.Parse(content[0]),
                    Name = content[1],
                    Latitude = double.Parse(content[4]),
                    Longitude = double.Parse(content[5])
                };
                kd.Add(new[] {geo.Latitude, geo.Longitude}, geo);
            }

            return kd;
        }

        public static IServiceCollection AddGeoInformation(this IServiceCollection service)
        {
            var kd = new KdTree<double, GeoInfo>(2, new DoubleMath());
            using var database = new StreamReader("CN.txt", Encoding.UTF8);
            for (var line = database.ReadLine(); line != null; line = database.ReadLine())
            {
                var content = line.Split('\t');
                if (content[6] != "A")
                    continue;
                var geo = new GeoInfo
                {
                    GeoNameId = int.Parse(content[0]),
                    Name = content[1],
                    Latitude = double.Parse(content[4]),
                    Longitude = double.Parse(content[5])
                };
                kd.Add(new[] {geo.Latitude, geo.Longitude}, geo);
            }

            service.AddSingleton<IKdTree<double, GeoInfo>>(kd);
            return service;
        }
    }
}