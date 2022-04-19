using System;
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

        public void Enable() {
            buildMode.gameObject.SetActive(false);
            buildPanel.SetActive(true);
            BuildSystem.current.Initialize(testPrefab);
            Debug.Log("hi");
        }

        public void Disable() {
            buildMode.gameObject.SetActive(true);
            buildPanel.SetActive(false);
        }
    }
}
