using System;
using Resources.Code.Building;
using UnityEngine;

namespace Resources.Code.Resources.Code.Building {
    public class DraggableObject : MonoBehaviour {
        public BuildSystem buildSystem;
        private void Start() => CameraMovement.MasterInput.Building.Enable();

        private void Update() {
            if (buildSystem.buildable) {
                
            }
            
            if(CameraMovement.MasterInput.Building.Place.ReadValue<float>() > 0) {
                buildSystem.Build();
            }
        }
    }
}
