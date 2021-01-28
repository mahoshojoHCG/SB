namespace AutoReport
{
    public record GeoInfo
    {
        public int GeoNameId { get; init; }
        public string Name { get; init; }
        public double Latitude { get; init; }
        public double Longitude { get; init; }
    }
}