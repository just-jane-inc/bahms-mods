using Godot;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Multiplayer.Game;

namespace JustColoring.JustColoringCode
{
    public partial class NetworkIO : Node
    {
        private static INetGameService? _netService;

        public override void _Ready()
        {
            MainFile.Logger.Info("network io ready!");
            _netService?.UnregisterMessageHandler<PlayerColorMessage>(HandlePlayerColorMessage);
            _netService?.UnregisterMessageHandler<PlayerConnectedMessage>(HandlePlayerConnectedMessage);
            _netService = null;

            base._Ready();
        }

        public override void _Process(double delta)
        {
            RunManager instance = RunManager.Instance;
            if (instance is null)
            {
                return;
            }

            if (!instance.IsInProgress)
            {
                if (_netService is not null)
                {
                    MainFile.Logger.Info("runtime instance marked as not in progress - tearing down");
                    _netService?.UnregisterMessageHandler<PlayerColorMessage>(HandlePlayerColorMessage);
                    _netService?.UnregisterMessageHandler<PlayerConnectedMessage>(HandlePlayerConnectedMessage);
                    _netService = null;
                }

                return;
            }

            if (instance.NetService is null)
            {
                return;
            }

            if (_netService is not null && instance.NetService != _netService)
            {
                MainFile.Logger.Info("instance netservice is different from currently tracked one");
                _netService?.UnregisterMessageHandler<PlayerColorMessage>(HandlePlayerColorMessage);
                _netService?.UnregisterMessageHandler<PlayerConnectedMessage>(HandlePlayerConnectedMessage);
                _netService = null;
                return;
            }

            if (_netService is not null)
            {
                return;
            }

            if (!instance.NetService.IsConnected)
            {
                return;
            }

            MainFile.Logger.Info("initializing network services...");
            _netService = instance.NetService;
            _netService.RegisterMessageHandler<PlayerColorMessage>(HandlePlayerColorMessage);
            _netService.RegisterMessageHandler<PlayerConnectedMessage>(HandlePlayerConnectedMessage);
            _netService.SendMessage<PlayerConnectedMessage>(new PlayerConnectedMessage());
            SendConfigurationUpdate();
            MainFile.Logger.Info("initialized...");

            base._Process(delta);
        }

        public static void SendConfigurationUpdate()
        {
            if (_netService is null)
                return;

            ulong id = PlatformUtil.GetLocalPlayerId(_netService.Platform);
            MapDrawingColorPatch.UserColorMap[id] = JustColorsConfig.ConfiguredColor;
            PlayerColorMessage msg = new()
            {
                Configuration = new PlayerColorConfig()
                {
                    Color = JustColorsConfig.DrawingColorThing,
                },
            };

            _netService.SendMessage<PlayerColorMessage>(msg);
        }

        private void HandlePlayerConnectedMessage(PlayerConnectedMessage message, ulong senderId)
        {
            MainFile.Logger.Info($"received player connect from [{GetPlayerName(senderId)}]");
            SendConfigurationUpdate();
        }

        private void HandlePlayerColorMessage(PlayerColorMessage message, ulong senderId)
        {
            MainFile.Logger.Info($"received updated color message received from [{GetPlayerName(senderId)}]");
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

        private void DetachNetService()
        {
            if (_netService is null)
            {
                return;
            }

            _netService?.UnregisterMessageHandler<PlayerColorMessage>(HandlePlayerColorMessage);
            _netService?.UnregisterMessageHandler<PlayerConnectedMessage>(HandlePlayerConnectedMessage);
            _netService = null;
        }

        public override void _ExitTree()
        {
            MainFile.Logger.Info("removing network io node from tree");
            DetachNetService();
            base._ExitTree();
        }
    }
}
