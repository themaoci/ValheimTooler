using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace ValheimTooler.Patches
{
    [HarmonyPatch(typeof(Inventory), "IsTeleportable")]
    class AlwaysTeleportAllow
    {
        private static bool Prefix(ref bool __result)
        {
            if (EntryPoint.s_enableAlwaysTeleportAllowing)
            {
                __result = true;
                return false;
            }
            return true;
        }
    }
}
