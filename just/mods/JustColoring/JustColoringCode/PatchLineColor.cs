using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;

namespace JustColoring.JustColoringCode
{
    // woof woof 69 nice 67 ehehe  - ty999999
    // patches sts2 NMapDrawings node CreateLineForPlayer method, adds our
    // stuff as a decorator
    [HarmonyPatch(typeof(NMapDrawings), "CreateLineForPlayer")]
    internal static class MapDrawingColorPatch
    {
        public static Dictionary<ulong, Color> UserColorMap = new();

        [HarmonyPostfix]
        private static void AfterCreatingLine(Player player, ref Line2D __result)
        {
            if (UserColorMap.TryGetValue(player.NetId, out Color color))
            {
                __result.DefaultColor = color;
            }
        }
    }
}
