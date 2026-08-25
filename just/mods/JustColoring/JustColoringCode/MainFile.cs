using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using BaseLib.Config;
using MegaCrit.Sts2.Core.Nodes;

namespace JustColoring.JustColoringCode;

//You're recommended but not required to keep all your code in this package and all your assets in the JustColoring folder.
[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "JustColoring"; //At the moment, this is used only for the Logger and harmony names.

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } = new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);

        // registers a configuration object to display in the config menu
        ModConfigRegistry.Register(ModId, new JustColorsConfig());

        // adds a node to the slay the spire scene, adding a node in the ready
        // of this main does not work
        NGame.Instance?.CallDeferred(Node.MethodName.AddChild, new NetworkIO());
        harmony.PatchAll();
    }
}
