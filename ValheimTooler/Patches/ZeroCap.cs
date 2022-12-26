using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace ValheimTooler.Patches
{
    [HarmonyPatch(typeof(Inventory), "GetTotalWeight")]
    class ZeroCap
    {
        private static bool Prefix(ref float __result)
        {
            if (EntryPoint.s_enableNoWeightLimit)
            {
                __result = 0f;
                return false;
            }
            return true;
        }
    }
}
