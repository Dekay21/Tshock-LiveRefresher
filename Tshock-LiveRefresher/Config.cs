namespace Tshock_LiveRefresher
{
    internal class Config
    {
        public string TshockPath { get; set; } = "./";
        public string ExecutableName { get; set; } = "Tshock.Server.exe";
        public string PluginsFolder { get; set; } = "ServerPlugins";
        public int RefreshInterval { get; set; } = 3000;
        public string[] Plugins { get; set; } = Array.Empty<string>();
        public string ServerParameters { get; set; } = "-ip 127.0.0.1 -port 7777 -maxplayers 1 -world \"path/to/your/world\"";
    }
}
