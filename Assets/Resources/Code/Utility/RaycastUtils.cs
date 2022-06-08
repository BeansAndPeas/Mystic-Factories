using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Resources.Code.Resources.Code.Utility {
    public static class RaycastUtils {
        [CanBeNull]
        public static GameObject FromMouse() {
            Debug.Assert(Camera.main != null, "Camera.main != null");
            Vector3 point = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero);
            if (!hit) 
                return null;
                
            GameObject objectHit = hit.transform.gameObject;
            return objectHit;
        }

        [CanBeNull]
        public static GameObject AtPosition(Vector2 pos) {
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if (!hit) 
                return null;
                
            GameObject objectHit = hit.transform.gameObject;
            return objectHit;
        }
    }
}
