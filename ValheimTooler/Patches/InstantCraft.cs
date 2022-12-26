using System;
using HarmonyLib;
using UnityEngine;

namespace ValheimTooler.Patches
{
    [HarmonyPatch(typeof(InventoryGui), "UpdateRecipe", new Type[]
    {
        typeof(Player),
        typeof(float)
    })]
    public class InstantCraft
    {
        private static void Prefix(ref Player player, ref float dt)
        {
            if (EntryPoint.s_enableInstantCraft)
            {
                dt = 2f;
                //Debug.Log("InstantCraft enabled");
            }
                
        }

    }
}
