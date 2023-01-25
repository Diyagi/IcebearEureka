using System;
using System.Linq;
using System.Numerics;
using Dalamud.Game.Text;
using Dalamud.Interface.Colors;
using ImGuiNET;

namespace IcebearEureka;

public class IcebearEurekaUI : IDisposable
{
    private readonly IcebearEurekaPlugin icebear;
    
    public bool SettingsVisible;

    public IcebearEurekaUI(IcebearEurekaPlugin icebear)
    {
        this.icebear = icebear;
    }
    
    public void Draw()
    {
        DrawSettings();
    }
    
    private void DrawSettings()
    {
        if (!SettingsVisible)
            return;
        
        ImGui.SetNextWindowSize(new Vector2(350, 120), ImGuiCond.FirstUseEver);
        if (ImGui.Begin("Icebear Eureka Settings", ref SettingsVisible))
        {
            ImGui.Spacing();

            bool showInServerBar = IcebearEurekaPlugin.Configuration.ShowInServerBar;
            if (ImGui.Checkbox("Show Server ID in the \"server info\" bar", ref showInServerBar))
            {
                IcebearEurekaPlugin.Configuration.ShowInServerBar = showInServerBar;
                IcebearEurekaPlugin.Configuration.Save();
            }
            
            bool showInChat = IcebearEurekaPlugin.Configuration.ShowInChat;
            if (ImGui.Checkbox("Show Server ID in chat when changing instances", ref showInChat))
            {
                IcebearEurekaPlugin.Configuration.ShowInChat = showInChat;
                IcebearEurekaPlugin.Configuration.Save();
            }
            
            XivChatType showInChatType = IcebearEurekaPlugin.Configuration.ShowInChatType;
            string[] chatTypes = Enum.GetNames(typeof(XivChatType)).Skip(1).ToArray();  ;
            int showInChatInt = Array.IndexOf(chatTypes, showInChatType.ToString());
            if (ImGui.Combo("Chat type", ref showInChatInt, chatTypes, chatTypes.Length))
            {
                XivChatType chatType = (XivChatType)Enum.Parse(typeof(XivChatType), chatTypes[showInChatInt]);
                IcebearEurekaPlugin.Configuration.ShowInChatType = chatType;
                IcebearEurekaPlugin.Configuration.Save();
            }
        }

        ImGui.End();
    }

    public void Dispose()
    {
        
    }
}
