using UnityEngine;

using TMPro;

namespace Resources.Code {
    public class FPSCounter : MonoBehaviour {
        [SerializeField]
        private TextMeshProUGUI fpsText;
        
        private float pollingTime = 0.5f;
        private float time;
        private int frameCount;
        
        private void Update() {
            time += Time.deltaTime;
            frameCount++;

            if (!(time >= pollingTime)) return;
            
            var frameRate = Mathf.RoundToInt(frameCount / time);
            fpsText.text = frameRate + " FPS";
            time -= pollingTime;
            frameCount = 0;
        }
    }
}
