using System;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.Runtime.InteropServices;
using Dalamud.Game.Gui;
using Dalamud.Game.Network;
using Dalamud.Game.Text;

namespace ServerIdTest
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Server ID Test";

        private readonly GameNetwork GameNetwork;
        private readonly ChatGui Chat;

        public Plugin(
            [RequiredVersion("1.0")] GameNetwork gameNetwork,
            [RequiredVersion("1.0")] ChatGui chat)
        {
            GameNetwork = gameNetwork;
            Chat = chat;
            GameNetwork.NetworkMessage += NetworkMessage;
            
        }
        
        private void NetworkMessage(
            IntPtr dataPtr, ushort opCode, uint sourceActorId, uint targetActorId, NetworkMessageDirection direction)
        {
            if (direction != NetworkMessageDirection.ZoneDown) return;
            if (opCode != 240) return;
            
            Chat.PrintChat(new XivChatEntry { Message = $"[{opCode}] Server ID: {Marshal.ReadInt16(dataPtr)}" });
        }

        public void Dispose()
        {
            GameNetwork.NetworkMessage -= NetworkMessage;
        }
        
        
    }
}
