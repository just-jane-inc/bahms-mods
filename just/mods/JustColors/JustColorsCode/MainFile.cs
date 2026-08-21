using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using BaseLib.Config;

namespace JustColors.JustColorsCode {
  [ModInitializer(nameof(Initialize))]
  public partial class MainFile : Node {
      public const string ModId = "just_colors";

      public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
          new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

      public static void Initialize() {
          Logger.Info("initializing mod!");
          Harmony harmony = new(ModId);
          ModConfigRegistry.Register(ModId, new JustColorsConfig());
          harmony.PatchAll();
      }
  }
}
