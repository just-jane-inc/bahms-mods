using Godot;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;

namespace JustColoring.JustColoringCode;

public static class JustNetworkHelpers
{
    public static void BroadcastConfigurationUpdate()
    {
        var netService = RunManager.Instance.NetService;

        ulong id = PlatformUtil.GetLocalPlayerId(netService.Platform);
        MapDrawingColorPatch.UserColorMap[id] = JustColorsConfig.DrawingColorThing;
        PlayerColorMessage msg = new()
        {
            Configuration = new PlayerColorConfig
            {
                Color = JustColorsConfig.DrawingColorThing.ToHtml()
            },
        };

        netService.SendMessage(msg);
    }

    public static void OnNetServiceInitialized(INetGameService netService)
    {
        MainFile.Logger.Info("Initializing Just Network Services!");
        netService.RegisterMessageHandler<PlayerColorMessage>(HandlePlayerColorMessage);
        MapDrawingColorPatch.UserColorMap.Clear();
    }

    public static void OnNetServiceCleanUp()
    {
        MainFile.Logger.Info("Cleaning Up Just Network Services!");
        RunManager.Instance.NetService.UnregisterMessageHandler<PlayerColorMessage>(HandlePlayerColorMessage);
        MapDrawingColorPatch.UserColorMap.Clear();
    }

    public static void OnRunStarted(RunState state)
    {
        MainFile.Logger.Info("Run started, broadcasting color!");
        BroadcastConfigurationUpdate();
    }

    private static void HandlePlayerColorMessage(PlayerColorMessage message, ulong senderId)
    {
        MainFile.Logger.Info($"color message received from [{senderId}]");
        var color = Color.FromString(message.Configuration.Color, default);
        MapDrawingColorPatch.UserColorMap[senderId] = color;
    }
}