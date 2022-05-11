using System;
using System.Collections;
using Resources.Code.Building;
using UnityEngine;

namespace Resources.Code.Resources.Code.Building {
    public class DraggableObject : MonoBehaviour {
        public BuildSystem buildSystem;
        private void Start() => CameraMovement.MasterInput.Building.Enable();
        private bool built;
        public bool ready;

        public IEnumerator Wait() {
            yield return new WaitForSeconds(1f);
            ready = true;
        }

        private void Update() {
            if (built) return;
            
            transform.position = BuildSystem.GetPosition();
            
            if (!ready || !(CameraMovement.MasterInput.Building.Place.ReadValue<float>() > 0)) return;

            built = buildSystem.Build();
        }
    }
}
