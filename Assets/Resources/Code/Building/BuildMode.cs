using System;
using System.Collections.Generic;
using Resources.Code.Building;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Resources.Code.UI {
    public class BuildMode : MonoBehaviour {
        [SerializeField]
        private Button buildMode;
        [SerializeField]
        private GameObject buildPanel;
        [SerializeField]
        private List<GameObject> structures = new List<GameObject>();
        [SerializeField]
        private BuildSystem buildSystem;

        private int currentIndex;
        private bool isEnabled;

        public void Enable() {
            buildMode.gameObject.SetActive(false);
            buildPanel.SetActive(true);
            buildSystem.SetObject(structures[currentIndex]);
            CameraMovement.MasterInput.Building.Enable();
            isEnabled = true;
        }

        public void Disable() {
            buildMode.gameObject.SetActive(true);
            buildPanel.SetActive(false);
            CameraMovement.MasterInput.Building.Disable();
            isEnabled = false;
        }

        private void Update() {
            if (!isEnabled) 
                return;
            
            float scrollAmount = Mouse.current.scroll.y.ReadValue();
            if (scrollAmount > 0) {
                GameObject next = GetNext();
                if (next == buildSystem.GetObject()) 
                    return;
                
                buildSystem.SetObject(next);
            } else if (scrollAmount < 0) {
                GameObject previous = GetPrevious();
                if (previous == buildSystem.GetObject()) 
                    return;
                
                buildSystem.SetObject(previous);
            }
        }

        private GameObject GetNext() {
            if (currentIndex >= structures.Count - 1) {
                currentIndex = 0;
            } else {
                currentIndex++;
            }

            return structures[currentIndex];
        }

        private GameObject GetPrevious() {
            if (currentIndex <= 0) {
                currentIndex = structures.Count - 1;
            } else {
                currentIndex--;
            }

            return structures[currentIndex];
        }
    }
}
