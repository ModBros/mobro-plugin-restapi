using MoBro.Plugin.RestApi;
using MoBro.Plugin.SDK;

// create and start the plugin to test it locally
using var plugin = MoBroPluginBuilder
  .Create<Plugin>()
  .WithSetting("port", "8080")
  .WithSetting("swagger_enable", "true")
  .Build();

// prevent the program from exiting immediately
Console.ReadLine();
