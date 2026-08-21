using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Potions;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;

namespace JustColors {
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

      ulong id = PlatformUtil.GetLocalPlayerId(_netService.Platform);
      JustColors.MapDrawingColorPatch.UserColorMap[id] = JustColors.JustColorsConfig.ConfiguredColor;


      if (runtimeManager.NetService.IsConnected) {
        return;
      }

      _netService = runtimeManager.NetService;
      _netService.RegisterMessageHandler<UserConfigMessage>(HandleUserConfigMessage);

      UserConfigMessage msg = new() {
        Configuration = new UserConfig() {
          UserId= id,
          Color= JustColors.JustColorsConfig.DrawingColorThing,
        },
      };

      _netService.SendMessage<UserConfigMessage>(msg);
      base._Process(delta);
    }

    private void HandleUserConfigMessage(UserConfigMessage message, ulong senderId) {
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
