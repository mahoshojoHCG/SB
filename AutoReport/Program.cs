using System;
using System.IO;
using System.Threading;
using AutoReport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

Directory.CreateDirectory(StaticConfig.LogLocation);
var configBuilder = new ConfigurationBuilder();
configBuilder.AddYamlFile(StaticConfig.ConfigFile, false, true);
var service = new ServiceCollection();
service.AddGeoInformation();
service.AddLogging(builder =>
{
    builder.AddConsole();
    builder.AddFile(o => { o.RootPath = StaticConfig.LogLocation; });
});
service.AddSingleton<IConfiguration>(configBuilder.Build());
service.AddSingleton<Reporter>();
service.AddSingleton<ReportRunner>();
await using var provider = service.BuildServiceProvider();
var runner = provider.GetRequiredService<ReportRunner>();
var source = new CancellationTokenSource();
Console.CancelKeyPress += (_, _) => { source.Cancel(); };
try
{
    await runner.RunAsync(source.Token);
}
catch (Exception e)
{
    Console.WriteLine(e);
}