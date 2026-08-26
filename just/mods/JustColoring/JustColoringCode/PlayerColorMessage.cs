using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using System.Text.Json;

namespace JustColoring.JustColoringCode
{

    public record PlayerColorConfig
    {
        public required string Color { get; init; }
    }

    public struct PlayerColorMessage : INetMessage, IPacketSerializable
    {
        public PlayerColorConfig Configuration;

        public bool ShouldBroadcast => true;

        public bool ShouldBuffer => false;

        public NetTransferMode Mode => NetTransferMode.Reliable;

        public LogLevel LogLevel => LogLevel.Info;

        public void Serialize(PacketWriter writer)
        {
            writer.WriteString(JsonSerializer.Serialize(Configuration));
        }

        public void Deserialize(PacketReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            string message = reader.ReadString();
            if (!string.IsNullOrWhiteSpace(message))
            {
                Configuration = JsonSerializer.Deserialize<PlayerColorConfig>(message);
            }
        }
    }

}
