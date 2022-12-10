namespace Tshock_LiveRefresher
{
    internal class Config
    {
        public string TshockPath = "./";
        public string ExecutableName = "Tshock.Server.exe";
        public int RefreshInterval = 3000;
        public string[] Plugins = Array.Empty<string>();
        public string ServerParameters = "";
    }
}
