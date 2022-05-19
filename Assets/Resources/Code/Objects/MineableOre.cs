using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = System.Diagnostics.Debug;

namespace Resources.Code {
    public class MineableOre : MonoBehaviour {
        [SerializeField]
        private int maxHealth;
        [SerializeField]
        private GameObject resource;
        
        private World world;
        private int health;

        private void Start() {
            health = maxHealth;
            world = FindObjectOfType<World>();
        }

        private void Update() {
            if (!Mouse.current.leftButton.wasReleasedThisFrame) return;

            Debug.Assert(Camera.main != null, "Camera.main != null");
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit)) return;
            
            GameObject objectHit = hit.transform.gameObject;
            if (objectHit != gameObject) return;

            if (--health > 0) return;
            
            Destroy(gameObject);
            world.GetTilemap().SetTile(world.GetTilemap().WorldToCell(transform.position), null);
            Instantiate(resource);
        }
    }
}
