using static UnityModManagerNet.UnityModManager;

namespace ChangeBuildText {
    public class Setting : ModSettings {
        public bool customText = true;
        public string text = "Custom Text";
        public bool rainbow = false;
        public float rainbowSpeed = 1.8f;
        public float rainbowGap = 0.98f;
    }
}
