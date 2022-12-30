using System;
using Dalamud.Configuration;
using Dalamud.Game.Text;
using Dalamud.Plugin;
using Newtonsoft.Json;

namespace IcebearEureka;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 1;
    public bool ShowInChat { get; set; } = true;
    public XivChatType ShowInChatType { get; set; } = XivChatType.Debug;
    
    [JsonProperty]
    private bool showInServerBar = true;
    
    [JsonIgnore]
    public bool ShowInServerBar
    {
        get => showInServerBar;
        set
        {
            plugin.SetShowInServerBar(value);
            showInServerBar = value;
        }
    }

    [NonSerialized]
    private DalamudPluginInterface pluginInterface;

    [NonSerialized] private IcebearEurekaPlugin plugin;

    public void Initialize(DalamudPluginInterface pluginInterface, IcebearEurekaPlugin plugin)
    {
        this.pluginInterface = pluginInterface;
        this.plugin = plugin;
    }

    public void Save()
    {
        pluginInterface.SavePluginConfig(this);
    }
}
