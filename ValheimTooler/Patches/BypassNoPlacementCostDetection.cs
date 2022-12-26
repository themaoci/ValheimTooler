using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace ValheimTooler.Patches
{
    class BypassNoPlacementCostDetection
    {
        [HarmonyPatch(typeof(Player), "NoCostCheat", new Type[] { })]
        public class NoPlacementCostPatch
        {
            private static bool Prefix(ref Player __instance, ref bool __result)
            {
                //bypass no placement cost detection xD but ssome staff will be skipped...
                __result = false;
                return false;
            }
        }

        // override can repair function to be able to use repair anywhere :)
        [HarmonyPatch(typeof(InventoryGui), "CanRepair", new Type[] { typeof(ItemDrop.ItemData) })]
        public class CanRepairPatch
        {
            private static bool Prefix(ref InventoryGui __instance, ref bool __result, ItemDrop.ItemData item)
            {
                var m_noPlacementCost = (bool)typeof(Player).GetField("m_noPlacementCost", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Player.m_localPlayer);
                if (m_noPlacementCost)
                {
                    __result = m_noPlacementCost;
                    return false;
                }
                return true;
            }
        }
    }
}
