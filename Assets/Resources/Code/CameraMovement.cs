using UnityEngine;
using UnityEngine.InputSystem;

namespace Resources.Code {
    public class CameraMovement : MonoBehaviour {
        [SerializeField]
        private PlayerInput input;
        private MasterInput masterInput;

        private void Awake() {
            masterInput =  new MasterInput();
            masterInput.Camera.Enable();
        }

        private void Update() {
            var movement = masterInput.Camera.Movement.ReadValue<Vector2>();
            transform.position += new Vector3(movement.x, movement.y, 0) * 2.5f * Time.deltaTime;
        }
    }
}
