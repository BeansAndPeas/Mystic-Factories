using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

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

        private void Update() {
            
        }
    }
}
