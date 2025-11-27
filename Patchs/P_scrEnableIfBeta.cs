using HarmonyLib;
using System;
using TMPro;

namespace ChangeBuildText.Patchs {
    internal class P_scrEnableIfBeta {
        public static Action<TMP_Text> OnAwake;

        [HarmonyPatch(typeof(scrEnableIfBeta), "Awake")]
        public class Patch_Awake {
            public static void Postfix(scrEnableIfBeta __instance) {
                TMP_Text txt = __instance.GetComponent<TMP_Text>();
                OnAwake?.Invoke(txt);
            }
        }
    }
}
