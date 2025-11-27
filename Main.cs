using ChangeBuildText.Core;
using ChangeBuildText.Patchs;
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;
using static UnityModManagerNet.UnityModManager;

namespace ChangeBuildText {
    public class Main {
        private static Main instance = new Main();

        private Harmony harmony;
        
        private Setting set;
        private TextRenderer renderer;
        private UIController ui;
        private MethodInfo targetMethod;

        private void Clear() {
            ui = null;
            renderer = null;
            set = null;
        }

        private void patchMethod(ModEntry entry, MethodInfo method) {
            instance.harmony = new Harmony(entry.Info.Id);
            instance.harmony.Patch(
                original: method,
                postfix: new HarmonyMethod(typeof(P_scrEnableIfBeta.Patch_Awake), "Postfix")
            );
        }

        public static bool On(ModEntry entry) {
            Type betaType = AccessTools.TypeByName("scrEnableIfBeta");
            if(betaType == null) {
                entry.Logger.Log("[WARN] scrEnableIfBeta class not found. Patch skipped.");
            } else {
                instance.targetMethod = AccessTools.Method(betaType, "Awake");
            }

            if(instance.targetMethod == null) {
                entry.Logger.Log("[WARN] Required class not found. Mod disabled.");
                entry.OnToggle = (_, v) => true;
                entry.Info.DisplayName += " (FAILED TO RUN)";
                entry.OnGUI = (_) => {
                    GUILayout.Label("Cannot run. ADOFAI has been updated, so the internal code has changed!");
                    GUILayout.Label("Please contact the mod developer to update this mod for the latest version.");
                };
            } else {
                entry.OnToggle = instance.OnToggle;
                entry.OnUpdate = instance.OnUpdate;
                entry.OnShowGUI = instance.OnShowGUI;
                entry.OnGUI = instance.OnGUI;
                entry.OnSaveGUI = instance.OnSaveGUI;
            }

            return true;
        }

        public bool OnToggle(ModEntry entry, bool toggle) {
            if(toggle) {
                set = new Setting();
                patchMethod(entry, targetMethod);
                renderer = new TextRenderer(() => set);
                P_scrEnableIfBeta.OnAwake = renderer.OnBetaAwake;
            } else {
                ModSettings.Save(set, entry);
                harmony?.UnpatchAll(entry.Info.Id);
                renderer.RestoreOriginal();
                Clear();
            }
            return true;
        }

        public void OnUpdate(ModEntry entry, float dt) {
            renderer.Update(Time.unscaledDeltaTime);
        }

        public void OnShowGUI(ModEntry entry) {
            if(ui == null) {
                ui = new UIController(set, renderer);
                ui.OnSettingChanged = () => ModSettings.Save(set, entry);
            }   
        }

        public void OnGUI(ModEntry entry) {
            ui.Draw();
        }

        public void OnSaveGUI(ModEntry entry) {
            ModSettings.Save(set, entry);
        }
    }
}