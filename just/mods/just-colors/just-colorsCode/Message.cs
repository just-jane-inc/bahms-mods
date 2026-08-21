using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using System.Text.Json;

namespace JustColors {

public record UserConfig {
  public ulong UserId {get; init;}
  public string Color {get; init;}
}

public struct UserConfigMessage : INetMessage, IPacketSerializable
{
    public UserConfig Configuration;

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
        Configuration = JsonSerializer.Deserialize<UserConfig>(reader.ReadString());
    }
}
}
