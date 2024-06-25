using MoBro.Plugin.RestApi;
using MoBro.Plugin.SDK;

// create and start the plugin to test it locally
var plugin = MoBroPluginBuilder
  .Create<Plugin>()
  .WithSetting("port", "8080")
  .Build();

// prevent the program from exiting immediately
Console.ReadLine();
