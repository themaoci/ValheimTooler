using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using ValheimTooler.Core;
using ValheimTooler.UI;
using ValheimTooler.Utils;

namespace ValheimTooler
{
    public class EntryPoint : MonoBehaviour
    {
        public static readonly int s_boxSpacing = 30;
        private Rect _valheimToolerRect;

        private bool _showMainWindow = true;
        public static bool s_showItemGiver = false;
        public static bool s_showPlayerESP = false;
        public static bool s_showMonsterESP = false;
        public static bool s_showDroppedESP = false;
        public static bool s_showDepositESP = false;
        public static bool s_showPickableESP = false;
        public static bool s_showESPBoxes = false;
        public static bool s_enableInstantCraft = false;
        public static bool s_enableInfinityStamina = false;
        public static bool s_enableGodmode = false;
        public static bool s_enableEdgeOfWorldKill = false;

        private WindowToolbar _windowToolbar = WindowToolbar.PLAYER;
        private readonly string[] _toolbarChoices = {
            "$vt_toolbar_player",
            "$vt_toolbar_entities",
            "$vt_toolbar_misc"
        };

        private string _version;

        public static bool AntiCheatLocated = false;
        internal static bool s_enableAutopinMap;
        internal static bool s_enableNoWeightLimit;
        internal static bool s_enableAlwaysTeleportAllowing;
        internal static bool s_enableSpeedHack;
        internal static float v_valueSpeedHack;

        public void Start()
        {
            LocateAnticheat();

            _valheimToolerRect = new Rect(5, 5, 800, 300);

            PlayerHacks.Start();
            EntitiesItemsHacks.Start();
            ItemGiver.Start();
            MiscHacks.Start();
            ESP.Start();

            _version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public void LocateAnticheat()
        {
            var list = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in list)
            {
                if (assembly.FullName.ToLower().Contains("anticheat") || assembly.FullName.ToLower().Contains("azumatt"))
                {
                    AntiCheatLocated = true;
                }
            }
        }
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                _showMainWindow = !_showMainWindow;
            }

            PlayerHacks.Update();
            EntitiesItemsHacks.Update();
            ItemGiver.Update();
            MiscHacks.Update();
            ESP.Update();
        }

        public void OnGUI()
        {
            GUI.skin = InterfaceMaker.CustomSkin;

            if (_showMainWindow)
            {
                _valheimToolerRect = GUILayout.Window(1001, _valheimToolerRect, ValheimToolerWindow, VTLocalization.instance.Localize($"$vt_main_title (v{_version})"), GUILayout.Height(10), GUILayout.Width(10));

                if (s_showItemGiver)
                {
                    ItemGiver.DisplayGUI();
                }
            }

            ESP.DisplayGUI();
        }

        void ValheimToolerWindow(int windowID)
        {
            GUILayout.Space(10);

            _windowToolbar = (WindowToolbar)GUILayout.Toolbar((int)_windowToolbar, _toolbarChoices.Select(choice => VTLocalization.instance.Localize(choice)).ToArray());

            switch (_windowToolbar)
            {
                case WindowToolbar.PLAYER:
                    PlayerHacks.DisplayGUI();
                    break;
                case WindowToolbar.ENTITIES_ITEMS:
                    EntitiesItemsHacks.DisplayGUI();
                    break;
                case WindowToolbar.MISC:
                    MiscHacks.DisplayGUI();
                    break;
            }

            GUI.DragWindow();
        }
    }
}
