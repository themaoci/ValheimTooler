using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace ValheimTooler.Patches
{
    [HarmonyPatch(typeof(Player), "EdgeOfWorldKill", new Type[]
    {
            typeof(float)
    })]
    class EdgeMapKillPatch
    {
        private static bool Prefix()
        {
            if (EntryPoint.s_enableEdgeOfWorldKill)
            {
                return false;
            }
            return true;
        }
    }
}
