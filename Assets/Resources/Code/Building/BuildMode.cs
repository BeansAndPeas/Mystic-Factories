using Resources.Code.Building;
using UnityEngine;
using UnityEngine.UI;

namespace Resources.Code.UI {
    public class BuildMode : MonoBehaviour {
        [SerializeField]
        private Button buildMode;
        [SerializeField]
        private GameObject buildPanel;
        [SerializeField]
        private GameObject testPrefab;
        [SerializeField]
        private BuildSystem buildSystem;

        public void Enable() {
            buildMode.gameObject.SetActive(false);
            buildPanel.SetActive(true);
            buildSystem.SetObject(testPrefab);
            CameraMovement.MasterInput.Building.Enable();
        }

        public void Disable() {
            buildMode.gameObject.SetActive(true);
            buildPanel.SetActive(false);
            CameraMovement.MasterInput.Building.Disable();
        }
    }
}
