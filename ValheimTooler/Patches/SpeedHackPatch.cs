using System;
using HarmonyLib;

namespace ValheimTooler.Patches
{
    [HarmonyPatch(typeof(Player), "GetRunSpeedFactor", new Type[]{})]
    class SpeedHackPatch
    {
        private static bool Prefix(ref float __result)
        {
            if (EntryPoint.s_enableSpeedHack)
            {
                __result = EntryPoint.v_valueSpeedHack;
                return false;
            }
            return true;
        }
    }
}
