using HarmonyLib;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Runs;

namespace JustColoring.JustColoringCode;

public class JustRunManagerPatches
{
    [HarmonyPatch(typeof(RunManager), "InitializeShared")]
    internal static class RunManagerAfterInitializeSharedPatch
    {
        [HarmonyPostfix]
        private static void AfterInitializeShared(INetGameService netService)
        {
            JustNetworkHelpers.OnNetServiceInitialized(netService);
        }
    }
    
    [HarmonyPatch(typeof(RunManager), nameof(RunManager.CleanUp))]
    internal static class RunManagerPreCleanUpPatch
    {
        [HarmonyPrefix]
        private static void PreCleanUp()
        {
            JustNetworkHelpers.OnNetServiceCleanUp();
        }
    }
}