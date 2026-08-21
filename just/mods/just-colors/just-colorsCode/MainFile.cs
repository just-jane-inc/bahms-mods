using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using BaseLib.Config;

namespace JustColors {

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node {
    public const string ModId = "JustColors";

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize() {
        Harmony harmony = new(ModId);
        ModConfigRegistry.Register(ModId, new JustColors.JustColorsConfig());
        harmony.PatchAll();
    }
}

}
