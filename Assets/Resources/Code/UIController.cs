using UnityEngine;
using UnityEngine.UI;

namespace Resources.Code {
    public class UIController : MonoBehaviour {
        [SerializeField] 
        private RawImage minimap;
        [SerializeField]
        private Camera minimapCamera;
        [SerializeField]
        private GameObject resourceBar;
        [SerializeField]
        private Transform resourceBarContent;
        [SerializeField]
        private GameObject resourceBarPrefab;
        
        private void Start() {
            for (var i = 0; i < 32; i++) {
                Instantiate(resourceBarPrefab, resourceBarContent);
            }
        }
    }
}
