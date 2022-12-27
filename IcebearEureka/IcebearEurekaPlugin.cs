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
    public sealed class IcebearEurekaPlugin : IDalamudPlugin
    {
        private const string ConstName = "IcebearEureka";
        public string Name => ConstName;

        public static GameNetwork GameNetwork { get; private set; }
        public static ChatGui Chat { get; private set; }
        public static DtrBar DtrBar { get; private set; }

        private readonly Int16[] zoneIds = {732, 763, 795, 827};
        private readonly DtrBarEntry dtrEntry;
        
        public IcebearEurekaPlugin(
            [RequiredVersion("1.0")] GameNetwork gameNetwork, 
            [RequiredVersion("1.0")] DtrBar dtrBar,
            [RequiredVersion("1.0")] ChatGui chat
        )
        {
            GameNetwork = gameNetwork;
            Chat = chat;
            DtrBar = dtrBar;

            GameNetwork.NetworkMessage += NetworkMessage;

            dtrEntry = dtrBar.Get(ConstName);

            if (dtrEntry != null)
            {
                dtrEntry.Text = "SID: ???";
                dtrEntry.Shown = false;
            }
        }

        private void NetworkMessage(
            IntPtr dataPtr, ushort opCode, uint sourceActorId, uint targetActorId, NetworkMessageDirection direction)
        {
            if (direction != NetworkMessageDirection.ZoneDown) return;
            if (opCode != 240) return;

            Int16 zoneId = Marshal.ReadInt16(dataPtr + 2);
            Int16 serverId = Marshal.ReadInt16(dataPtr);
            
            if (zoneIds.Contains(zoneId))
            {
                ZoneId zone = (ZoneId)zoneId;
                Chat.PrintChat(new XivChatEntry { Message = $"[{zone.ToString()}] Server ID: {serverId}" });
            }
            
            if (dtrEntry != null)
            {
                dtrEntry.Text = $"SID: {serverId}";
                dtrEntry.Shown = zoneIds.Contains(zoneId);
            }
        }
        
        public void Dispose()
        {
            GameNetwork.NetworkMessage -= NetworkMessage;
            dtrEntry?.Dispose();
        }
    }
}
