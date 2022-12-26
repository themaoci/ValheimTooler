using System;
using System.Collections.Generic;
using System.Linq;
using RapidGUI;
using UnityEngine;
using ValheimTooler.Core.Extensions;
using ValheimTooler.Utils;

namespace ValheimTooler.Core
{
    public static class PlayerHacks
    {
        private static bool s_isInfiniteStaminaMe = false;
        private static bool s_isInfiniteStaminaOthers = false;
        private static bool s_isNoStaminaOthers = false;
        private static string s_teleportPositionString;
        private static int s_teleportTargetIdx = -1;
        private static int s_healTargetIdx = -1;
        private static string s_guardianPowerIdx = "";
        private static IDictionary<string, string> s_guardianPowers;
        private static int s_skillNameIdx = -1;
        private static int s_skillLevelIdx = 0;

        private static float s_actionTimer = 0f;
        private static readonly float s_actionTimerInterval = 0.5f;

        private static float s_updateTimer = 0f;
        private static readonly float s_updateTimerInterval = 1.5f;

        private static List<ZNet.PlayerInfo> s_netPlayers = null;
        private static List<string> s_netPlayerNames = new List<string>();

        private static List<Player> s_players = new List<Player>();
        private static List<string> s_playerNames = new List<string>();

        private static readonly List<string> s_skills = new List<string>();
        private static readonly List<string> s_levels = new List<string>();

        public static void Start()
        {
            foreach (object obj in Enum.GetValues(typeof(Skills.SkillType)))
            {
                Skills.SkillType skillType = (Skills.SkillType)obj;

                s_skills.Add(skillType.ToString());
            }
            for (var i = 1; i <= 100; i++)
            {
                s_levels.Add(i.ToString());
            }

            s_guardianPowers = new Dictionary<string, string>() {
            { Localization.instance.Localize("$se_eikthyr_name"), "GP_Eikthyr" },
            { Localization.instance.Localize("$se_theelder_name"), "GP_TheElder" },
            { Localization.instance.Localize("$se_bonemass_name"), "GP_Bonemass" },
            { Localization.instance.Localize("$se_moder_name"), "GP_Moder" },
            { Localization.instance.Localize("$se_yagluth_name"), "GP_Yagluth" }
        };
        }

        public static void Update()
        {
            if (Time.time >= s_actionTimer)
            {
                if (s_isInfiniteStaminaMe)
                {
                    Player.m_localPlayer.VTSetMaxStamina();
                }
                if (s_isInfiniteStaminaOthers)
                {
                    AllOtherPlayersMaxStamina();
                }
                if (s_isNoStaminaOthers)
                {
                    AllOtherPlayerNoStamina();
                }
                s_actionTimer = Time.time + s_actionTimerInterval;
            }

            if (Time.time >= s_updateTimer)
            {
                s_netPlayerNames.Clear();
                s_playerNames.Clear();

                if (ZNet.instance == null)
                {
                    s_netPlayers = null;
                    s_teleportTargetIdx = -1;
                }
                else
                {
                    s_netPlayers = ZNet.instance.GetPlayerList();

                    if (s_netPlayers != null)
                    {
                        s_netPlayerNames = s_netPlayers.Select(p => p.m_name).ToList();
                    }
                }

                s_players = Player.GetAllPlayers();
                s_playerNames = s_players.Select(p => p.GetPlayerName()).ToList();

                s_updateTimer = Time.time + s_updateTimerInterval;
            }
        }

        public static void DisplayGUI()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical(VTLocalization.instance.Localize("$vt_player_general_title"), GUI.skin.box, GUILayout.ExpandWidth(false));
                {
                    GUILayout.Space(EntryPoint.s_boxSpacing);
                    if (!EntryPoint.AntiCheatLocated)
                    {
                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_god_mode : " + (Player.m_localPlayer.VTInGodMode() ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                        {
                            Player.m_localPlayer.VTSetGodMode(!Player.m_localPlayer.VTInGodMode());
                        }
                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_inf_stamina_me : " + (s_isInfiniteStaminaMe ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                        {
                            s_isInfiniteStaminaMe = !s_isInfiniteStaminaMe;
                        }
                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_inf_stamina_others : " + (s_isInfiniteStaminaOthers ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                        {
                            s_isInfiniteStaminaOthers = !s_isInfiniteStaminaOthers;
                        }
                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_no_stamina : " + (s_isNoStaminaOthers ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                        {
                            s_isNoStaminaOthers = !s_isNoStaminaOthers;
                        }
                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_fly_mode : " + (Player.m_localPlayer.VTInFlyMode() ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                        {
                            Player.m_localPlayer.VTSetFlyMode(!Player.m_localPlayer.VTInFlyMode());
                        }
                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_ghost_mode : " + (Player.m_localPlayer.VTInGhostMode() ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                        {
                            Player.m_localPlayer.VTSetGhostMode(!Player.m_localPlayer.VTInGhostMode());
                        }
                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_nop_lacement_cost : " + (Player.m_localPlayer.VTIsNoPlacementCost() ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                        {
                            Player.m_localPlayer.VTSetNoPlacementCost(!Player.m_localPlayer.VTIsNoPlacementCost());
                        }
                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_explore_minimap")))
                        {
                            Minimap.instance.VTExploreAll();
                        }
                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_reset_minimap")))
                        {
                            Minimap.instance.VTReset();
                        }
                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_tame_creatures")))
                        {
                            Player.m_localPlayer.VTTameNearbyCreatures();
                        }
                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_infinite_weight : " + (Player.m_localPlayer.VTIsInventoryInfiniteWeight() ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                        {
                            Player.m_localPlayer.VTInventoryInfiniteWeight(!Player.m_localPlayer.VTIsInventoryInfiniteWeight());
                        }
                    }
                    else
                    {
                        GUILayout.Label("Detected options are disabled");
                    }
                    if (GUILayout.Button($"Godmode (hidden) {((EntryPoint.s_enableGodmode)?"ON":"OFF")}"))
                    {
                        EntryPoint.s_enableGodmode = !EntryPoint.s_enableGodmode;
                    }
                    if (GUILayout.Button($"InfinityStamina (hidden) {((EntryPoint.s_enableInfinityStamina) ?"ON":"OFF")}"))
                    {
                        EntryPoint.s_enableInfinityStamina = !EntryPoint.s_enableInfinityStamina;
                    }
                    if (GUILayout.Button($"InstantCraft (hidden) {((EntryPoint.s_enableInstantCraft) ?"ON":"OFF")}"))
                    {
                        EntryPoint.s_enableInstantCraft = !EntryPoint.s_enableInstantCraft;
                    }
                    if (GUILayout.Button($"EdgeOfWorldKill (hidden) {((EntryPoint.s_enableEdgeOfWorldKill) ? "ON" : "OFF")}"))
                    {
                        EntryPoint.s_enableEdgeOfWorldKill = !EntryPoint.s_enableEdgeOfWorldKill;
                    }
                    if (GUILayout.Button($"NoCap (hidden) {((EntryPoint.s_enableNoWeightLimit) ? "ON" : "OFF")}"))
                    {
                        EntryPoint.s_enableNoWeightLimit = !EntryPoint.s_enableNoWeightLimit;
                    }
                    if (GUILayout.Button($"AllowTeleporting (hidden) {((EntryPoint.s_enableAlwaysTeleportAllowing) ? "ON" : "OFF")}"))
                    {
                        EntryPoint.s_enableAlwaysTeleportAllowing = !EntryPoint.s_enableAlwaysTeleportAllowing;
                    }
                    if (GUILayout.Button($"AutoPinMap {((EntryPoint.s_enableAutopinMap) ? "ON" : "OFF")}"))
                    {
                        EntryPoint.s_enableAutopinMap = !EntryPoint.s_enableAutopinMap;
                    }
                    if (GUILayout.Button($"SpeedHack({Mathf.Round(EntryPoint.v_valueSpeedHack)}) {((EntryPoint.s_enableSpeedHack) ? "ON" : "OFF")}"))
                    {
                        EntryPoint.s_enableSpeedHack = !EntryPoint.s_enableSpeedHack;
                    }
                    EntryPoint.v_valueSpeedHack = GUILayout.HorizontalSlider(EntryPoint.v_valueSpeedHack, 1f, 50f);
                    if (GUILayout.Button("Get All Recipes (can crash)"))
                    {
                        foreach (var recipe in ObjectDB.instance.m_recipes)
                        {
                            if (recipe.m_enabled)
                            {
                                if (recipe.m_item != null && recipe.m_item.m_itemData != null && recipe.m_item.m_itemData.m_shared != null)
                                {
                                    Debug.Log($"{recipe.m_item.GetHoverName()} || {recipe.m_craftingStation.GetHoverName()}");
                                    Player.m_localPlayer.AddKnownRecipe(recipe);
                                }
                            }
                        }
                    }
                    
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                {
                    GUILayout.BeginVertical(VTLocalization.instance.Localize("$vt_player_teleport_title"), GUI.skin.box, GUILayout.ExpandWidth(false));
                    {
                        GUILayout.Space(EntryPoint.s_boxSpacing);
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(VTLocalization.instance.Localize("$vt_player_teleport_player :"), GUILayout.ExpandWidth(false));

                            s_teleportTargetIdx = RGUI.SelectionPopup(s_teleportTargetIdx, s_netPlayerNames.ToArray());
                        }
                        GUILayout.EndHorizontal();

                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_teleport_button")))
                        {
                            if (s_netPlayers != null && s_teleportTargetIdx < s_netPlayers.Count && s_teleportTargetIdx >= 0)
                            {
                                Player.m_localPlayer.VTTeleportTo(s_netPlayers[s_teleportTargetIdx]);
                            }
                        }
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(VTLocalization.instance.Localize("$vt_player_teleport_title"), GUI.skin.box, GUILayout.ExpandWidth(false));
                    {
                        GUILayout.Space(EntryPoint.s_boxSpacing);
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(VTLocalization.instance.Localize("$vt_player_teleport_player :"), GUILayout.ExpandWidth(false));
                            s_teleportPositionString = GUILayout.TextField(s_teleportPositionString);
                        }
                        GUILayout.EndHorizontal();

                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_teleport_button")))
                        {
                            {
                                var pos = s_teleportPositionString.Split(',');
                                if (pos.Length == 3)
                                {
                                    float x = float.MaxValue;
                                    float y = float.MaxValue;
                                    float z = float.MaxValue;
                                    try
                                    {
                                        x = int.Parse(pos[0]);
                                        y = int.Parse(pos[1]);
                                        z = int.Parse(pos[2]);
                                    }
                                    catch { }
                                    if (x != float.MaxValue && y != float.MaxValue && z != float.MaxValue)
                                    {
                                        Player.m_localPlayer.TeleportTo(new Vector3(int.Parse(pos[0]), int.Parse(pos[1]), int.Parse(pos[2])), Player.m_localPlayer.transform.rotation, true);
                                    }
                                }
                            }
                        }
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(VTLocalization.instance.Localize("$vt_player_heal_manager_title"), GUI.skin.box, GUILayout.ExpandWidth(false));
                    {
                        GUILayout.Space(EntryPoint.s_boxSpacing);
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(VTLocalization.instance.Localize("$vt_player_heal_player :"), GUILayout.ExpandWidth(false));

                            s_healTargetIdx = RGUI.SelectionPopup(s_healTargetIdx, s_playerNames.ToArray());
                        }
                        GUILayout.EndHorizontal();

                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_heal_selected_player")))
                        {
                            if (s_healTargetIdx < s_players.Count && s_healTargetIdx >= 0)
                            {
                                s_players[s_healTargetIdx].VTHeal();
                            }
                        }
                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_heal_all_players")))
                        {
                            foreach (Player player in s_players)
                            {
                                player.VTHeal();
                            }
                        }
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(VTLocalization.instance.Localize("$vt_player_power_title"), GUI.skin.box, GUILayout.ExpandWidth(false));
                    {
                        GUILayout.Space(EntryPoint.s_boxSpacing);
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(VTLocalization.instance.Localize("$vt_player_power_name :"), GUILayout.ExpandWidth(false));
                            s_guardianPowerIdx = RGUI.SelectionPopup(s_guardianPowerIdx, s_guardianPowers.Keys.Select(p => VTLocalization.instance.Localize(p)).ToArray());
                        }
                        GUILayout.EndHorizontal();


                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_power_active_me")))
                        {
                            if (Player.m_localPlayer != null)
                            {
                                Player.m_localPlayer.VTActiveGuardianPower(s_guardianPowers[s_guardianPowerIdx]);
                            }
                        }
                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_power_active_all")))
                        {
                            if (s_guardianPowers.ContainsKey(s_guardianPowerIdx))
                            {
                                AllPlayersActiveGuardianPower(s_guardianPowers[s_guardianPowerIdx]);
                            }
                        }
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(VTLocalization.instance.Localize("$vt_player_skill_title"), GUI.skin.box, GUILayout.ExpandWidth(false));
                    {
                        GUILayout.Space(EntryPoint.s_boxSpacing);
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(VTLocalization.instance.Localize("$vt_player_skill_name :"), GUILayout.ExpandWidth(false));
                            s_skillNameIdx = RGUI.SelectionPopup(s_skillNameIdx, s_skills.ToArray());
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(VTLocalization.instance.Localize("$vt_player_skill_level :"), GUILayout.ExpandWidth(false));
                            s_skillLevelIdx = RGUI.SelectionPopup(s_skillLevelIdx, s_levels.ToArray());
                        }
                        GUILayout.EndHorizontal();

                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_skill_button")))
                        {
                            if (s_skillNameIdx < s_skills.Count && s_skillNameIdx >= 0)
                            {
                                if (int.TryParse(s_levels[s_skillLevelIdx], out int levelInt))
                                {
                                    Player.m_localPlayer.VTUpdateSkillLevel(s_skills[s_skillNameIdx], levelInt);
                                }
                            }
                        }
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private static void AllOtherPlayersMaxStamina()
        {
            List<Player> players = Player.GetAllPlayers();

            if (players != null && Player.m_localPlayer != null)
            {
                foreach (Player player in players)
                {
                    if (player.GetPlayerID() != Player.m_localPlayer.GetPlayerID())
                    {
                        player.VTSetMaxStamina();
                    }
                }
            }
        }

        private static void AllOtherPlayerNoStamina()
        {
            List<Player> players = Player.GetAllPlayers();

            if (players != null && Player.m_localPlayer != null)
            {
                foreach (Player player in players)
                {
                    if (player.GetPlayerID() != Player.m_localPlayer.GetPlayerID())
                    {
                        player.VTSetNoStamina();
                    }
                }
            }
        }

        private static void AllPlayersActiveGuardianPower(string guardianPower)
        {
            List<Player> players = Player.GetAllPlayers();

            if (players != null)
            {
                foreach (Player player in players)
                {
                    player.VTActiveGuardianPower(guardianPower);
                }
            }
        }
    }
}
