using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using ValheimTooler.Utils;

namespace ValheimTooler.Patches
{
    [HarmonyPatch(typeof(Destructible), "Start")]
    class AutoPinResources
    {
        private static void Postfix(ref Destructible __instance)
        {
            if (!EntryPoint.s_enableAutopinMap)
                return;
            HoverText component = __instance.GetComponent<HoverText>();
            if (component)
            {
                if (__instance.gameObject.GetComponent<PinnedObject>() != null)
                    return;
                string text = component.m_text;
                var splitted = text.Split('_');
                string subtype = (splitted.Count() > 2)?splitted[splitted.Count() - 2]:"";
                subtype = subtype.Replace("deposit", "dp").Replace("destructible", "");
                string Name = text.Split('_').Last();
                Name = Name.Replace("stone", "").Replace("bundle", "");
                bool ShouldPin = __instance.GetDestructibleType() != DestructibleType.Tree && __instance.GetDestructibleType() != DestructibleType.Character || __instance.name.ToLower().Contains("ygg");
                if (ShouldPin)
                {
                    __instance.gameObject.AddComponent<PinnedObject>().Init($"{Name}-{subtype}");
                    Debug.Log($"Pin candidate: Name:{Name}({subtype})");
                }
            }
        }
    }
}
