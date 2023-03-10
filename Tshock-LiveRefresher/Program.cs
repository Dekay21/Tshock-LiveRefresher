using System.Text.Json;
using System.Diagnostics;

namespace Tshock_LiveRefresher
{
    internal class Program
    {
        static readonly string ConfigName = "tshock-liverefresher-config.json";
        private static Config Config;
        static Process? Process;

        static void Main(string[] args)
        {
            if (!File.Exists(ConfigName))
            {
                var options = new JsonSerializerOptions { WriteIndented=true, IncludeFields=true};
                File.WriteAllText(ConfigName, JsonSerializer.Serialize(new Config(), options));
                Console.WriteLine("Could not find config. Please fill the " + ConfigName + " next to the executable");
                Console.ReadLine();
                Environment.Exit(1);
            }
            var tmp = JsonSerializer.Deserialize<Config>(File.ReadAllText(ConfigName));
            if (tmp == null)
            {
                Console.WriteLine("Invalid config. Please check " + ConfigName);
                Environment.Exit(1);
            }
            Config = tmp;

            while (true)
            {
                var toCopy = Config.Plugins.Where(path => CheckFile(path));
                if (toCopy.Any() || Process == null || Process.HasExited)
                {
                    RestartServer(toCopy);
                }

                Thread.Sleep(Config.RefreshInterval);
            }
        }

        static bool CheckFile(string pluginPath)
        {
            string filename = Path.GetFileName(pluginPath);
            string newLocation = Path.Combine(Config.TshockPath, Config.PluginsFolder, filename);
            if (!File.Exists(newLocation) || File.GetLastWriteTimeUtc(pluginPath) > File.GetLastWriteTimeUtc(newLocation))
            {
                Console.WriteLine($"{filename} has changed.");
                return true;
            }
            return false;
        }

        static void RestartServer(IEnumerable<string> filesToCopy)
        {
            if (Config == null)
            {
                Environment.Exit(1);
            }

            Console.WriteLine("Restarting server.");
            if (Process != null && !Process.HasExited)
            {
                Process.Kill();
            }

            foreach (string file in filesToCopy)
            {
                string newLocation = Path.Combine(Config.TshockPath, Config.PluginsFolder, Path.GetFileName(file));
                File.Copy(file, newLocation, true);
            }

            var startInfo = new ProcessStartInfo(Path.Combine(Config.TshockPath, Config.ExecutableName), Config.ServerParameters)
            {
                WorkingDirectory = Config.TshockPath
            };
            Process = Process.Start(startInfo);
        }
    }
}