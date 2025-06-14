using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace ACE.Server
{
    partial class Program
    {
        private static async Task CheckForServerUpdate()
        {
            log.Info($"Automatic Server version check started...");
            try
            {
                var worldDb = new Database.WorldDatabase();
                var currentVersion = worldDb.GetVersion();
                log.Info($"Current Server Binary: {ServerBuildInfo.FullVersion}");

                var url = "https://api.github.com/repos/ACEmulator/ACE/releases/latest";
                using var client = new WebClient();
                var html = await client.GetStringFromURL(url);

                var json = JsonSerializer.Deserialize<JsonElement>(html);

                string tag = json.GetProperty("tag_name").GetString();
               
                //Split the tag from "v{version}.{build}" into discrete components  - "tag_name": "v1.39.4192"
                Version v = new Version(tag.Remove(0, 1));
                Version currentServerVersion = ServerBuildInfo.GetServerVersion();

                var versionStatus = v.CompareTo(currentServerVersion);
                // Status returns > 0 if the GitHub version is newer. (0 if the same, or < 0 if older.)
                if (versionStatus > 0)
                {
                    log.Warn("There is a newer version of ACE available!");
                    log.Warn($"Please visit {json.GetProperty("html_url").GetString()} for more information.");

                    // the Console.Title.Get() only works on Windows...
                    #pragma warning disable CA1416 // Validate platform compatibility
                    Console.Title += " -- Server Binary Update Available";
                    #pragma warning restore CA1416 // Validate platform compatibility
                }
                else
                {
                    log.Info($"Latest Server Version is {tag} -- No Update Required!");
                }
                return;
            }
            catch (Exception ex)
            {
                log.Info($"Unable to continue with Automatic Server Version Check due to the following error: {ex}");
            }
        }
    }
}
