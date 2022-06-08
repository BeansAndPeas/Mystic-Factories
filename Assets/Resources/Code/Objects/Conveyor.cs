using System;
using System.Collections;
using Resources.Code.Resources.Code.Building;
using Resources.Code.Resources.Code.Utility;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Resources.Code.Objects {
    public class Conveyor : MonoBehaviour {
        private TextMeshProUGUI counter;
        private void Awake() => counter = GameObject.Find("Count").GetComponent<TextMeshProUGUI>();
        private void Start() => StartCoroutine(HandleItemDrop());

        [SerializeField]
        private GameObject itemPrefab;
        private GameObject item;
        
        private IEnumerator HandleItemDrop() {
            while (true) {
                if (GetComponent<DraggableObject>() || HasItem()) {
                    yield return new WaitForSeconds(0.5f);
                    continue;
                }

                GameObject hit = RaycastUtils.FromMouse();
                if (!hit || hit != gameObject) {
                    yield return new WaitForSeconds(0.5f);
                    continue;
                }

                int count = int.Parse(counter.text);
                if (count <= 0) {
                    yield return new WaitForSeconds(0.25f);
                    continue;
                }
                
                counter.SetText(count - 1 + "");
                SetItem(Instantiate(itemPrefab));
                item.transform.position = transform.position;
                break;
            }

            yield return null;
        }

        private void Update() {
            if (!HasItem()) return;

            Vector3 pos = item.transform.position;
            pos.y += 0.1f;

            Transform tform = transform;
            if (!(pos.y > tform.position.y + (tform.localScale.y / 2))) {
                item.transform.position = pos;
                return;
            }
            
            Vector3 up = tform.position + new Vector3(0, tform.localScale.y, 0);
            GameObject hit = RaycastUtils.AtPosition(up);
            if (!hit) 
                return;

            var conveyor = hit.GetComponent<Conveyor>();
            var test = hit.GetComponent<Test>();
            if (conveyor && !conveyor.HasItem()) {
                conveyor.SetItem(item);
                SetItem(null);
            } else if (test) {
                
            }
        }

        public bool HasItem() => item;

        public void SetItem(GameObject obj) => item = obj;
    }
}
