using System;
using System.Collections;
using Resources.Code.Resources.Code.Utility;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Debug = System.Diagnostics.Debug;

namespace Resources.Code.Objects {
    public class ResourceItem : MonoBehaviour {
        public string resourceName;
        [Min(1)]
        public int maxHealth;
        [Min(0.01f)]
        public float spawnVelocity;
        [Min(0)]
        public int minSpawn;
        [Min(0)]
        public int maxSpawn;
        public World world;
        
        private TextMeshProUGUI counter;

        private void Awake() => counter = GameObject.Find("Count").GetComponent<TextMeshProUGUI>();

        private void Start() => StartCoroutine(HandlePickup());

        private IEnumerator HandlePickup() {
            while(true) {
                GameObject hit = RaycastUtils.FromMouse();
                if (!hit || hit != gameObject) {
                    yield return new WaitForSeconds(0.5f);
                    continue;
                }

                Destroy(gameObject);

                counter.SetText(int.Parse(counter.text) + 1 + "");
                break;
            }

            yield return null;
        }
    }
}
