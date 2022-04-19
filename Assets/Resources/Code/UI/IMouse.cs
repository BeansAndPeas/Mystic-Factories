using UnityEngine.InputSystem;

namespace Resources.Code.UI {
    public interface IMouse {
        void OnMouseDown(InputAction.CallbackContext ctx);
        void OnMouseDrag(InputAction.CallbackContext ctx);
        void OnMouseUp(InputAction.CallbackContext ctx);
    }
}
