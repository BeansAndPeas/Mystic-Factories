using UnityEngine;
using UnityEngine.InputSystem;

namespace Resources.Code.UI {
    public class MouseHandler : MonoBehaviour, IMouse {
        private Vector3 offset;
        private PlayerInput input;
        private InputAction drag;
        private IMouse currentDragger;

        private void Start() {
            input = Camera.main.GetComponent<PlayerInput>();
            InputActionMap map = input.currentActionMap;
            
            InputAction click = map.FindAction("Click", true);
            click.started += OnClickStarted;
            click.canceled += OnClickCancelled;

            drag = map.FindAction("Drag", true);
        }

        private void OnClickStarted(InputAction.CallbackContext ctx) {
            RaycastHit hit;

            switch (Mouse.current.clickCount.ReadValue()) {
                case 1:
                    currentDragger = this;

                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit)) {
                        IMouse dragger = hit.collider.gameObject.GetComponent<IMouse>();

                        if (dragger != null) {
                            currentDragger = dragger;
                        }
                    }

                    currentDragger.OnMouseDown(ctx);
                    drag.performed += currentDragger.OnMouseDrag;

                    break;
                case 2:
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit)) {
                        IDoubleClick doubleClick = hit.collider.gameObject.GetComponent<IDoubleClick>();
                        doubleClick?.OnDoubleClick();
                    }

                    break;
            }
        }

        public void OnClickCancelled(InputAction.CallbackContext ctx) {
            drag.performed -= currentDragger.OnMouseDrag;
            currentDragger.OnMouseUp(ctx);
        }
        
        public void OnMouseDown(InputAction.CallbackContext ctx) {}
        public void OnMouseDrag(InputAction.CallbackContext ctx) {}
        public void OnMouseUp(InputAction.CallbackContext ctx) {}
    }
}
