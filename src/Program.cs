using GeoJSON.Net.Feature;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

var url = "https://service.pdok.nl/rws/ahn/atom/downloads/dtm_05m/kaartbladindex.json";

var httpClient = new HttpClient();
var json = await httpClient.GetAsync(url);

var features = JsonConvert.DeserializeObject<FeatureCollection>(await json.Content.ReadAsStringAsync());

var result = new StringBuilder();

foreach(var feature in features.Features)
{
    var cogurl = feature.Properties["url"];
    result.Append($" /vsicurl/{cogurl}");
}

// System.Diagnostics.Process.Start("CMD.exe", $"gdalbuildvrt test.vrt {result}");


ProcessStartInfo processStartInfo = new ProcessStartInfo();
processStartInfo.FileName = @"C:\Windows\system32\cmd.exe";
// processStartInfo.Arguments = "/c date /t";
processStartInfo.Arguments = $"gdalbuildvrt test.vrt {result}";

processStartInfo.CreateNoWindow = true;
processStartInfo.UseShellExecute = false;
processStartInfo.RedirectStandardOutput = true;

Process process = new Process();
process.StartInfo = processStartInfo;
process.Start();

string output = process.StandardOutput.ReadToEnd();
process.WaitForExit();

Console.WriteLine("Current date (received from CMD):");
Console.Write(output);

Console.WriteLine(result.ToString());







