using HarmonyLib;
using RapidGUI;
using UnityEngine;

namespace ValheimTooler
{
    public class Loader
    {
        public static void Init()
        {
            if (Loader.s_entryPoint == null)
            {
                RunPatches();
                Loader.s_entryPoint = new GameObject();
                Loader.s_entryPoint.AddComponent<EntryPoint>();
                Loader.s_entryPoint.AddComponent<RapidGUIBehaviour>();
                Object.DontDestroyOnLoad(Loader.s_entryPoint);
                
            }
        }
        private static readonly Harmony harmony = new Harmony("ValheimTooler");
        private static void RunPatches()
        {
            harmony.PatchAll();
        }

        public static void Unload()
        {
            _Unload();
        }
        private static void _Unload()
        {
            GameObject.Destroy(Loader.s_entryPoint);
        }

        private static GameObject s_entryPoint;
    }

}
