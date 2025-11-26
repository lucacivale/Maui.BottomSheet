using System.Runtime.CompilerServices;

[assembly: XmlnsDefinition("http://pluginmauibottomsheet.com", "Plugin.BottomSheet", AssemblyName = "Plugin.BottomSheet")]
[assembly: XmlnsDefinition("http://pluginmauibottomsheet.com", "Plugin.Maui.BottomSheet")]
[assembly: XmlnsDefinition("http://pluginmauibottomsheet.com/platformconfiguration/android", "Plugin.Maui.BottomSheet.PlatformConfiguration.AndroidSpecific")]

[assembly: InternalsVisibleTo("Plugin.BottomSheet.Tests.Maui.Unit")]
[assembly: InternalsVisibleTo("Plugin.BottomSheet.Tests.Maui.Unit.Application")]
[assembly: InternalsVisibleTo("Plugin.BottomSheet.Tests.Maui.UI.Application")]