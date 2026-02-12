using CG.Client.UI;
using HarmonyLib;

namespace $safeprojectname$
{
    [HarmonyPatch(typeof(FadeController), "Start")]
    class Patch
    {
        static void Postfix()
        {
            BepinPlugin.Log.LogInfo("Example Patch Executed");
        }
    }
}
