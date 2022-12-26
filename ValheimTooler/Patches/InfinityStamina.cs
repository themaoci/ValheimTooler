using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace ValheimTooler.Patches
{
    [HarmonyPatch(typeof(Player), "UseStamina", new Type[]
    {
        typeof(float)
    })]
    class InfinityStamina
    {
        private static void Prefix(ref float v)
        {
            if (EntryPoint.s_enableInfinityStamina)
            {
                //Debug.Log("NoStaminaUsage enabled");
                v = 0f;
            }
        }
    }
}
