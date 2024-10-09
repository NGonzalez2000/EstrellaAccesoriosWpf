using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.IO;

namespace EstrellaAccesoriosWpf.Models.Common;

public class ConfigurationFile(IConfiguration configuration)
{
    private readonly string _configFilePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

    public void UpdateWordPath(string newPath)
    {
        var json = File.ReadAllText(_configFilePath);
        var jsonObj = JObject.Parse(json);

        // Modify the Word path in the "Storages" section
        jsonObj["Storages"]!["Word"] = newPath;

        // Save the changes back to appsettings.json
        File.WriteAllText(_configFilePath, jsonObj.ToString());


        Reload();
    }

    public string GetWordPath()
    {
        return configuration["Storages:Word"] ?? string.Empty;
    }

    private void Reload()
    {
        if (configuration is IConfigurationRoot configurationRoot)
        {
            configurationRoot.Reload();
        }
    }
}
