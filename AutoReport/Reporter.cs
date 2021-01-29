using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Geolocation;
using KdTree;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutoReport
{
    public class Reporter
    {
        private readonly IKdTree<double, GeoInfo> _geoInfo;
        private readonly ILogger<Reporter> _logger;

        public Reporter(ILogger<Reporter> logger, IKdTree<double, GeoInfo> geoInfo)
        {
            _logger = logger;
            _geoInfo = geoInfo;
        }

        private async Task GenerateReportAsync(ReportInformation info, ChromeDriver driver, CancellationToken token)
        {
            await Task.Delay(1000, token);
            _logger.LogInformation("Make sure you acknowledge this:");
            _logger.LogInformation("1.全国中高风险地区");
            _logger.LogInformation("2.假期期间做好个人防护");
            _logger.LogInformation("3.个人及共同居住人有疫情相关情况的，及时向学校报告");
            await Task.Delay(1000, token);
            _logger.LogInformation(
                $"Your generated location is {driver.FindElementByXPath("/html/body/div[1]/div[3]/div[1]/input").GetProperty("value")}.");
            driver.FindElementByXPath("/html/body/div[7]/input").Click();
            await Task.Delay(100, token);
            driver.ExecuteScript("save()");
            await Task.Delay(100, token);
            driver.FindElementByXPath("/html/body/div[13]/div[3]/a[2]").Click();
            await Task.Delay(100, token);
            _logger.LogInformation("Report has generated.");
            await Task.Delay(5000, token);
            var reportStatus = driver.FindElementByXPath("/html/body/div[1]/div[2]/div[2]/div[1]/div[2]");
            if (reportStatus.Text == "审核状态：待辅导员审核")
                _logger.LogInformation("Report succeed!");
            else
                _logger.LogWarning($"Status for {info.UserName} is {reportStatus.Text} and might failed.");
        }

        public async Task ReportAsync(ReportInformation info, CancellationToken token = default)
        {
            _logger.LogInformation($"Begin report {info.UserName} at {info.Latitude}, {info.Longitude}");
            var result = _geoInfo.GetNearestNeighbours(new[] {info.Latitude, info.Longitude}, 1)[0];
            var origin = new Coordinate(info.Latitude, info.Longitude);
            var found = new Coordinate(result.Point[0], result.Point[1]);
            if (GeoCalculator.GetDistance(origin, found, 1, DistanceUnit.Kilometers) > 100)
            {
                _logger.LogCritical("Your location is possibly not in China and must be wrong.");
                throw new UnsafeLocationException("Report for {info.UserName} skipped due to wrong location.");
            }

            _logger.LogInformation($"Your location is possible {result.Value.Name}");

            //Start the driver
            var option = new ChromeOptions();
            //Only headless when not debugging
            if (!Debugger.IsAttached)
                option.AddArgument("--headless");
            //Disable GPU in linux
            if (OperatingSystem.IsLinux())
                option.AddArgument("--disable-gpu");
            //Allow run in root
            if (Environment.UserName == "root" || Environment.UserName == "Administrator")
                option.AddArgument("--no-sandbox");
            using var driver = new ChromeDriver(option);
            //Set geolocation
            driver.ExecuteChromeCommand("Browser.grantPermissions", new()
            {
                {"origin", "https://xg.hit.edu.cn/"},
                {"permissions", new[]{"geolocation"}}
            });
            driver.ExecuteChromeCommand("Emulation.setGeolocationOverride", new()
            {
                {"latitude", info.Latitude},
                {"longitude", info.Longitude},
                {"accuracy", 1}
            });
            //Login
            driver.Url = "https://xg.hit.edu.cn/zhxy-xgzs/xg_mobile/shsj/loginChange";
            await Task.Delay(10000, token);
            if (driver.Url != "https://xg.hit.edu.cn/zhxy-xgzs/xg_mobile/shsj/loginChange")
                throw new NotSupportedException(
                    "Login flow of the remote system has changed, " +
                    "please consider send an issue or make a pull request on GitHub.");
            driver.FindElementByXPath("/html/body/div/div[2]/button[1]").Click();
            await Task.Delay(500, token);
            driver.FindElementByXPath("/html/body/div[2]/div[2]/div[2]/div/div[3]/div/form/p[1]/input")
                .SendKeys(info.UserName);
            var passwordBox =
                driver.FindElementByXPath("/html/body/div[2]/div[2]/div[2]/div/div[3]/div/form/p[2]/input[1]");
            passwordBox.SendKeys(info.DeEncryptedPassword);
            await Task.Delay(500, token);
            passwordBox.SendKeys(Keys.Enter);
            await Task.Delay(500, token);
            if (driver.Url != "https://xg.hit.edu.cn/zhxy-xgzs/xg_mobile/xsHome")
            {
                _logger.LogError($"Login failed for {info.UserName}, please check your username and password.");
                throw new UnauthorizedAccessException();
            }

            _logger.LogInformation($"Login success for {info.UserName}.");
            //Load information screen
            driver.Url = "https://xg.hit.edu.cn/zhxy-xgzs/xg_mobile/xs/yqxx";
            await Task.Delay(5000, token);
            var latestDate = driver.FindElementByXPath("/html/body/div[1]/div[2]/div[2]/div[1]/div[1]");
            var date = DateTime.Parse(latestDate.Text[5..]);
            //Process generated report but not saved.
            if (date.Date == DateTime.Now.Date)
            {
                _logger.LogInformation($"Report for {info.UserName} has generated today.");
                var reportStatus = driver.FindElementByXPath("/html/body/div[1]/div[2]/div[2]/div[1]/div[2]");
                if (reportStatus.Text == "审核状态：未提交")
                {
                    _logger.LogInformation($"But report for {info.UserName} has not submitted, trying to submit.");
                    driver.FindElementByXPath("/html/body/div[1]/div[2]/div[2]/div[2]").Click();
                    await GenerateReportAsync(info, driver, token);
                }
                else
                    _logger.LogInformation($"User {info.UserName} has reported, and status is {reportStatus.Text}");
            }
            else
            {
                driver.ExecuteScript("add()");
                await GenerateReportAsync(info, driver, token);
            }
        }
    }
}