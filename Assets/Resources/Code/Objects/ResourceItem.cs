using UnityEngine;

namespace Resources.Code {
    public class ResourceItem : MonoBehaviour {
        [SerializeField]
        private string resourceName;

        public void SetPosition(Vector3 pos) {
            transform.position = pos;
        }
    }
}
