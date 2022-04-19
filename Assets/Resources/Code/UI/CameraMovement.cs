using UnityEngine;
using UnityEngine.InputSystem;

namespace Resources.Code {
    public class CameraMovement : MonoBehaviour {
        [SerializeField]
        private PlayerInput input;
        public static MasterInput MasterInput;

        private void Awake() {
            MasterInput =  new MasterInput();
            MasterInput.Camera.Enable();
        }

        private void Update() {
            var movement = MasterInput.Camera.Movement.ReadValue<Vector2>();
            transform.position += new Vector3(movement.x, movement.y, 0) * 2.5f * Time.deltaTime;
        }
    }
}
