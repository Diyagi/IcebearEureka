using System;
using System.Linq;
using Dalamud.Plugin;
using System.Runtime.InteropServices;
using Dalamud.Game.Gui;
using Dalamud.Game.Gui.Dtr;
using Dalamud.Game.Network;
using Dalamud.Game.Text;
using Dalamud.IoC;

namespace IcebearEureka
{
    public class IcebearEurekaPlugin : IDalamudPlugin
    {
        private const string ConstName = "Icebear Eureka";
        public string Name => ConstName;

        public static DalamudPluginInterface PluginInterface { get; private set; }
        public static GameNetwork GameNetwork { get; private set; }
        public static ChatGui Chat { get; private set; }
        public static DtrBar DtrBar { get; private set; }
        
        public static Configuration Configuration { get; private set; }
        public IcebearEurekaUI IcebearEurekaUI { get; }
        
        private DtrBarEntry dtrEntry;
        
        public IcebearEurekaPlugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] GameNetwork gameNetwork, 
            [RequiredVersion("1.0")] DtrBar dtrBar,
            [RequiredVersion("1.0")] ChatGui chat
        )
        {
            PluginInterface = pluginInterface;
            GameNetwork = gameNetwork;
            Chat = chat;
            DtrBar = dtrBar;

            Configuration = pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            Configuration.Initialize(pluginInterface, this);

            IcebearEurekaUI = new(this);
            
            if (Configuration.ShowInServerBar)
            {
                dtrEntry = dtrBar.Get(ConstName);
            }

            PluginInterface.UiBuilder.Draw += IcebearEurekaUI.Draw;
            PluginInterface.UiBuilder.OpenConfigUi += () => IcebearEurekaUI.SettingsVisible = true;
            
            GameNetwork.NetworkMessage += NetworkMessage;
        }

        public void SetShowInServerBar(bool value)
        {
            if (value == Configuration.ShowInServerBar) return;

            if (value)
            {
                dtrEntry = DtrBar.Get(ConstName);
                dtrEntry.Shown = false;
                dtrEntry.Text = "SID: ??";
            }
            else
                dtrEntry.Dispose();
        }

        private void NetworkMessage(
            IntPtr dataPtr, ushort opCode, uint sourceActorId, uint targetActorId, NetworkMessageDirection direction)
        {
            if (direction != NetworkMessageDirection.ZoneDown) return;
            if (opCode != 148) return;

            Int16 zoneId = Marshal.ReadInt16(dataPtr + 2);
            Int16 serverId = Marshal.ReadInt16(dataPtr);
            
            SendChatSid(zoneId, serverId);
            UpdateDtr(zoneId, serverId);
            
        }

        public void SendChatSid(Int16 zoneId, Int16 serverId)
        {
            if (!Configuration.ShowInChat) return;
            if (!Enum.IsDefined(typeof(ZoneId), zoneId)) return;
            
            ZoneId zone = (ZoneId)zoneId;
            Chat.PrintChat(new XivChatEntry { Type = Configuration.ShowInChatType, Message = $"[{zone.ToString()}] Server ID: {serverId}" });
        }

        private void UpdateDtr(Int16 zoneId, Int16 serverId)
        {
            if (!Configuration.ShowInServerBar) return;
            
            dtrEntry.Text = $"SID: {serverId}";
            dtrEntry.Shown = Enum.IsDefined(typeof(ZoneId), zoneId);
        }
        
        public void Dispose()
        {
            PluginInterface.UiBuilder.Draw -= IcebearEurekaUI.Draw;
            GameNetwork.NetworkMessage -= NetworkMessage;
            dtrEntry?.Dispose();
        }
    }
}
