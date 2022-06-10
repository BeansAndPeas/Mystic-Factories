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
        private float objectHeight;
        private GameObject item;
        
        [SerializeField]
        private GameObject itemPrefab;
        
        private void Awake() {
            counter = GameObject.Find("Count").GetComponent<TextMeshProUGUI>();
            objectHeight = transform.localScale.y * 0.16f;
        }
        
        private void Start() => StartCoroutine(HandleItemDrop());

        private IEnumerator HandleItemDrop() {
            while (true) {
                if (GetComponent<DraggableObject>() || HasItem() || !Mouse.current.leftButton.wasReleasedThisFrame) {
                    yield return new WaitForSeconds(0.001f);
                    continue;
                }
                
                GameObject hit = RaycastUtils.FromMouse();
                if (!gameObject.Equals(hit)) {
                    yield return new WaitForSeconds(0.001f);
                    continue;
                }

                int count = int.Parse(counter.text);
                if (count <= 0) {
                    yield return new WaitForSeconds(0.001f);
                    continue;
                }
                
                counter.SetText(count - 1 + "");
                SetItem(Instantiate(itemPrefab, transform.position, Quaternion.identity, transform));
                yield return new WaitUntil(() => !HasItem());
            }
        }

        private void Update() {
            if (!HasItem()) return;

            Vector3 pos = item.transform.position;
            pos.y += 0.1f * Time.deltaTime;

            Transform tform = transform;
            if (!(pos.y > tform.position.y + objectHeight / 2f)) {
                item.transform.position = pos;
                return;
            }
            
            Vector3 up = tform.position + new Vector3(0, objectHeight / 2f + 0.05f, 0);
            GameObject hit = RaycastUtils.AtPosition(up);
            if (!hit) 
                return;

            var conveyor = hit.GetComponent<Conveyor>();
            var test = hit.GetComponent<Test>();
            if (conveyor && !conveyor.HasItem() && !conveyor.GetComponent<DraggableObject>()) {
                conveyor.SetItem(item);
                SetItem(null);
            } else if (test) {
                if (test.GiveItem(item)) {
                    SetItem(null);
                }
            }
        }

        public bool HasItem() => item != null;

        public void SetItem(GameObject obj) => item = obj;
    }
}
