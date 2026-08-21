using Godot;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Multiplayer.Game;

namespace JustColors.JustColorsCode {
  public partial class NetworkIO : Node {
    private INetGameService _netService;

    public override void _Process(double delta)
    {
      if (_netService is not null) {
        return;
      }

      var runtimeManager = RunManager.Instance;
      if (runtimeManager is null) {
        return;
      }

      if (!runtimeManager.IsInProgress) {
        return;
      }

      if (runtimeManager.NetService is null) {
        return;
      }

      MainFile.Logger.Info("we are here, hear us roar");
      ulong id = PlatformUtil.GetLocalPlayerId(_netService.Platform);
      MapDrawingColorPatch.UserColorMap[id] = JustColorsConfig.ConfiguredColor;
      MainFile.Logger.Info($"this machines user id is [{id}]");

      if (runtimeManager.NetService.IsConnected) {
        return;
      }

      _netService = runtimeManager.NetService;
      _netService.RegisterMessageHandler<UserConfigMessage>(HandleUserConfigMessage);

      UserConfigMessage msg = new() {
        Configuration = new UserConfig() {
          UserId= id,
          Color= JustColorsConfig.DrawingColorThing,
        },
      };

      _netService.SendMessage<UserConfigMessage>(msg);
      base._Process(delta);
    }

    private void HandleUserConfigMessage(UserConfigMessage message, ulong senderId) {
      MainFile.Logger.Info($"message received from [{senderId}]");
      Color color = Color.FromString(message.Configuration.Color, default);
      MapDrawingColorPatch.UserColorMap[senderId] = color;
    }

    string GetPlayerName(ulong senderId)
    {
      if (_netService is null)
          return senderId.ToString();

      try
      {
          return PlatformUtil.GetPlayerName(_netService.Platform, senderId);
      }
      catch
      {
          return senderId.ToString();
      }
    }
  }
}
