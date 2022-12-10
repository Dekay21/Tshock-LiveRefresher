using Newtonsoft.Json;
using System.Diagnostics;

namespace Tshock_LiveRefresher
{
    internal class Program
    {
        static readonly string ConfigName = "tshock-liverefresher.json";
        static readonly string ServerPluginsName = "ServerPlugins";
        static Config Config;
        static Process Process;

        static void Main(string[] args)
        {
            if (!File.Exists(ConfigName))
            {
                File.WriteAllText(ConfigName, JsonConvert.SerializeObject(new Config(), Formatting.Indented));
                Console.WriteLine("Could not find config. Please fill the " + ConfigName + " next to the executable");
                Environment.Exit(1);
            }

            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigName));
            if (Config == null)
            {
                Console.WriteLine("Invalid config. Please check " + ConfigName);
                Environment.Exit(1);
            }

            while (true)
            {
                List<string> toCopy = new List<string>();
                foreach (string path in Config.Plugins)
                {
                    if (CheckFile(path))
                    {
                        toCopy.Add(path);
                    }

                }

                if (toCopy.Count > 0 || Process == null || Process.HasExited)
                {
                    RestartServer(toCopy);
                }

                Thread.Sleep(Config.RefreshInterval);
            }
        }

        static bool CheckFile(string pluginPath)
        {
            string filename = Path.GetFileName(pluginPath);
            string newLocation = Path.Combine(Config.TshockPath, ServerPluginsName, filename);
            if (!File.Exists(newLocation) || File.GetLastWriteTimeUtc(pluginPath) > File.GetLastWriteTimeUtc(newLocation))
            {
                Console.WriteLine($"{filename} has changed.");
                return true;
            }
            return false;
        }

        static void RestartServer(List<string> filesToCopy)
        {
            Console.WriteLine("Restarting server.");
            if (Process != null && !Process.HasExited)
            {
                Process.Kill();
            }
            foreach (string file in filesToCopy)
            {
                string newLocation = Path.Combine(Config.TshockPath, ServerPluginsName, Path.GetFileName(file));
                File.Copy(file, newLocation, true);
            }
            var startInfo = new ProcessStartInfo(Path.Combine(Config.TshockPath, Config.ExecutableName), Config.ServerParameters);
            startInfo.WorkingDirectory = Config.TshockPath;
            Process = Process.Start(startInfo);
        }
    }
}