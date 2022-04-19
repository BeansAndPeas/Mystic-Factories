using Resources.Code.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Resources.Code.Building {
    public class ObjectDrag : MonoBehaviour, IMouse {
        private Vector3 offset;

        public void OnMouseDown(InputAction.CallbackContext ctx) {
            offset = transform.position - BuildSystem.GetMouseWorldPosition();
        }
        public void OnMouseDrag(InputAction.CallbackContext ctx) {}

        private void Update() {
            Vector3 pos = BuildSystem.GetMouseWorldPosition() + offset;
            transform.position = BuildSystem.current.SnapCoordinateToGrid(pos);

            //TODO: Add input key for placing
            PlaceableObject placeable = gameObject.GetComponent<PlaceableObject>();
            if (BuildSystem.current.CanBePlaced(placeable)) {
                placeable.Place();
                Vector3Int start = BuildSystem.current.gridLayout.WorldToCell(placeable.GetStartPosition());
                BuildSystem.current.TakeArea(start, placeable.Size);
            } else {
                Destroy(gameObject);
            } 
        }

        public void OnMouseUp(InputAction.CallbackContext ctx) {}
    }
}
