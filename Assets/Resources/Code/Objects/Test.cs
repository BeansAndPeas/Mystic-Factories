using System;
using Resources.Code.Resources.Code.Building;
using Resources.Code.Resources.Code.Utility;
using UnityEngine;

namespace Resources.Code.Objects {
    public class Test : MonoBehaviour {
        private float objectHeight;
        
        private GameObject currentlyProcessing;
        private GameObject currentOutput;
        private int progress = 0;
        private const int MAX_PROGRESS = 10;

        [SerializeField]
        private GameObject ingotPrefab;

        private void Awake() => objectHeight = transform.localScale.y * 0.16f;

        public bool CanAccept() {
            return !currentlyProcessing && !currentOutput;
        }
        
        public bool GiveItem(GameObject item) {
            if (!CanAccept()) return false;
            
            currentlyProcessing = item;
            currentlyProcessing.SetActive(false);
            currentOutput = null;
            return true;
        }

        private void Update() {
            if (currentOutput) {
                Vector2 up = transform.position + new Vector3(0, objectHeight / 2f + 0.05f, 0);
                GameObject hit = RaycastUtils.AtPosition(up);
                    
                if (!hit) 
                    return;

                Conveyor conveyor = hit.GetComponent<Conveyor>();
                if (conveyor && !conveyor.HasItem() && !conveyor.GetComponent<DraggableObject>()) {
                    conveyor.SetItem(Instantiate(currentOutput, hit.transform.position, Quaternion.identity, hit.transform));
                    currentOutput = null;
                } else {
                    //TODO: Handle other machines
                }
            }

            if (!currentlyProcessing || currentOutput) return;

            if (progress++ < MAX_PROGRESS) return;
            currentOutput = ingotPrefab;
            Destroy(currentlyProcessing);
            currentlyProcessing = null;
        }
    }
}
