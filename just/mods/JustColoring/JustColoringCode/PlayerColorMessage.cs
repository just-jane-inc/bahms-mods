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

    public record PlayerConnectedPayload
    {
        public string Message { get; init; } = "nya";
    }

    public struct PlayerConnectedMessage : INetMessage, IPacketSerializable
    {
        public PlayerConnectedMessage()
        {
            Payload = new();
        }

        public PlayerConnectedPayload Payload;

        public bool ShouldBroadcast => true;

        public bool ShouldBuffer => false;

        public NetTransferMode Mode => NetTransferMode.Reliable;

        public LogLevel LogLevel => LogLevel.Info;

        public void Serialize(PacketWriter writer)
        {
            writer.WriteString(JsonSerializer.Serialize(Payload));
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
                Payload = JsonSerializer.Deserialize<PlayerConnectedPayload>(message);
            }
        }
    }
}
