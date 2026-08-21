using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;

namespace JustColors.JustColorsCode {
    // woof woof 69 nice 67 ehehe  - ty999999
    [HarmonyPatch(typeof(NMapDrawings), "CreateLineForPlayer")]
    internal static class MapDrawingColorPatch {
        public static Dictionary<ulong, Color> UserColorMap = new();

        [HarmonyPostfix]
        private static void AfterCreatingLine(Player player, ref Line2D __result) {
          __result.DefaultColor = Color.Color8(0xFF, 0xFF, 0xFF, 0xFF);
          if (UserColorMap.TryGetValue(player.NetId, out Color color)) {
            MainFile.Logger.Info("hello world");
            __result.DefaultColor = Color.Color8(0xFF, 0xFF, 0xFF, 0xFF);
          }
        }
    }
}
