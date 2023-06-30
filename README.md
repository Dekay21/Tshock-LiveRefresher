# Tshock-LiveRefresher

Inspired by ZakFahey's [live-refresher](https://github.com/ZakFahey/live-refresher)
Used for developing tshock plugins and reloading the server on relevant changes

Configured through `tshock-liverefresher-config.json`
```json
{
  "TshockPath": "./",
  "ExecutableName": "Tshock.Server.exe",
  "PluginsFolder": "ServerPlugins",
  "RefreshInterval": 2000,
  "Plugins": ["absolute path to plugin dlls"],
  "ServerParameters": "-ip 127.0.0.1 -port 7777 -maxplayers 1 -world \"C:/Users/[user]/Documents/My Games/Terraria/Worlds/[your world].wld\""
}
```

`TshockPath`: Path to the folder where the tshock folder is located (Only change if the exceutable is not next to the tshock executable)

`ExecutableName`: Name of the executable (Only change if you renamed it)

`PluginsFolder`: Name of the folder for server plugins (Relative to TshockPath)

`RefreshInterval`: Interval in milliseconds to look for changes in plugin dlls

`Plugins`: An array of all plugin dlls to check for changes

`ServerParameters`: Needed for automatic server startup (Check out the [tshock documentation](https://ikebukuro.tshock.co/#/command-line-parameters) for more)
