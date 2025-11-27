using System;
using System.Text;
using TMPro;
using UnityEngine;

namespace ChangeBuildText.Core {
    public class TextRenderer {
        private TMP_Text betaText;
        private string originalText;
        private readonly Func<Setting> setGetter;

        private float hue;

        public TextRenderer(Func<Setting> setGetter) {
            this.setGetter = setGetter;
        }

        public void OnBetaAwake(TMP_Text txt) {
            betaText = txt;
            if(betaText == null) {
                return;
            }

            if(originalText == null) {
                originalText = betaText.text;
            }

            var s = setGetter();
            if(s.customText) {
                betaText.text = s.text;
            }
        }

        public void ApplyCustom() {
            if(betaText == null) {
                return;
            }

            var s = setGetter();
            betaText.text = s.text;
        }
        public void RestoreOriginal() {
            if(betaText == null) {
                return;
            }

            betaText.text = originalText;
        }

        public void ApplyCustomOrOriginal() {
            if(betaText == null) {
                return;
            }

            var s = setGetter();
            betaText.text = s.customText ? s.text : originalText;
        }

        public void Update(float dt) {
            var s = setGetter();
            if(betaText == null) {
                return;
            }

            if(!s.rainbow) {
                return;
            }

            string text = s.customText ? s.text : originalText;
            if(string.IsNullOrEmpty(text)) {
                return;
            }

            hue += dt * s.rainbowSpeed * 0.1f;
            if(hue > 1f) {
                hue -= 1f;
            }

            var sb = new StringBuilder();

            for(int i = 0; i < text.Length; i++) {
                float h = (hue + s.rainbowGap * i) % 1f;
                Color c = Color.HSVToRGB(h, 1f, 1f);
                sb.Append($"<color=#{ColorUtility.ToHtmlStringRGB(c)}>{text[i]}</color>");
            }

            betaText.text = sb.ToString();
        }
    }
}
