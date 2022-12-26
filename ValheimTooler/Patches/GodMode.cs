using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace ValheimTooler.Patches
{
    [HarmonyPatch(typeof(Character), "ApplyDamage", new Type[]
    {
        typeof(HitData),
        typeof(bool),
        typeof(bool),
        typeof(HitData.DamageModifier)
    })]
    class GodMode
    {
        private static bool Prefix(ref Character __instance,
            ref HitData hit, ref bool showDamageText, ref bool triggerEffects, ref HitData.DamageModifier mod)
        {
            
            if (EntryPoint.s_enableGodmode)
            {
                if (__instance.m_name.ToLower() == Player.m_localPlayer.m_name.ToLower())
                {
                    Debug.Log($"ApplyDamage: {hit.m_damage} nullified for {__instance.m_name}");
                    return false;
                }
            }
            return true;
        }
    }
}
