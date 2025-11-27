using System;
using UnityEngine;

namespace ChangeBuildText.Core {
    public class UIController {
        private Setting set;
        private TextRenderer renderer;

        public Action OnSettingChanged;

        public UIController(Setting set, TextRenderer renderer) {
            this.set = set;
            this.renderer = renderer;
        }

        public void Draw() {
            bool newCustom = GUILayout.Toggle(set.customText, "Custom Text");
            if(newCustom != set.customText) {
                set.customText = newCustom;
                OnSettingChanged?.Invoke();
                renderer.ApplyCustomOrOriginal();
            }

            if(set.customText) {
                string newText = GUILayout.TextField(set.text);
                if(newText != set.text) {
                    set.text = newText;
                    OnSettingChanged?.Invoke();
                    renderer.ApplyCustom();
                }
            }

            bool newRainbow = GUILayout.Toggle(set.rainbow, "Rainbow");
            if(newRainbow != set.rainbow) {
                set.rainbow = newRainbow;
                OnSettingChanged?.Invoke();
                if(!newRainbow) {
                    renderer.ApplyCustomOrOriginal();
                }
            }

            if(set.rainbow) {
                GUILayout.Label($"Rainbow Speed: {set.rainbowSpeed:0.00}");
                float newSpeed = GUILayout.HorizontalSlider(set.rainbowSpeed, 0f, 100f);
                if(newSpeed != set.rainbowSpeed) {
                    set.rainbowSpeed = newSpeed;
                    OnSettingChanged?.Invoke();
                }

                GUILayout.Label($"Rainbow Gap: {set.rainbowGap:0.00}");
                float newGap = GUILayout.HorizontalSlider(set.rainbowGap, 0f, 1f);
                if(newGap != set.rainbowGap) {
                    set.rainbowGap = newGap;
                    OnSettingChanged?.Invoke();
                }
            }
        }
    }
}