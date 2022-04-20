using System;
using Resources.Code.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Resources.Code.Building {
    public class ObjectDrag : MonoBehaviour {
        private void Start() {
            CameraMovement.MasterInput.Building.Place.performed += Place;
        }

        private void Place(InputAction.CallbackContext ctx) {
            PlaceableObject placeable = BuildSystem.Current.objectToPlace;
            if (BuildSystem.Current.CanBePlaced(placeable)) {
                placeable.Place();
                Vector3Int start = BuildSystem.Current.gridLayout.WorldToCell(placeable.GetStartPosition());
                BuildSystem.Current.TakeArea(start, placeable.Size);
            } else {
                Destroy(gameObject);
            }
        }

        private void Update() {
            Vector3 pos = BuildSystem.GetMouseWorldPosition();
            transform.position = BuildSystem.Current.SnapCoordinateToGrid(pos);
        }
    }
}
