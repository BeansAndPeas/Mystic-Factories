using System;
using System.Collections;
using Resources.Code.Objects;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = System.Diagnostics.Debug;
using Random = System.Random;

namespace Resources.Code.Objects {
    public class MineableOre : MonoBehaviour {
        [SerializeField]
        private GameObject resource;
        public Vector3Int tilemapPos;
        private readonly Random rand = new Random();
        
        private World world;
        private int health;
        private float timer;
        private const float Time = 0.2f;
        private Vector3 startPos;
        private Vector3 randomPos;
        public float delayBetweenShakes = 0f;
        public float distance = 0.01f;

        private void Start() {
            health = resource.GetComponent<ResourceItem>().maxHealth;
            world = FindObjectOfType<World>();
            startPos = transform.position;
        }

        private IEnumerator Shake() {
            timer = 0f;
 
            while (timer < Time) {
                timer += UnityEngine.Time.deltaTime;
                randomPos = startPos + (UnityEngine.Random.insideUnitSphere * distance);
                transform.position = randomPos;
 
                if (delayBetweenShakes > 0f) {
                    yield return new WaitForSeconds(delayBetweenShakes);
                } else {
                    yield return null;
                }
            }
 
            transform.position = startPos;
        }

        private void Update() {
            if (!Mouse.current.leftButton.wasReleasedThisFrame) return;

            Debug.Assert(Camera.main != null, "Camera.main != null");
            Vector3 point = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero);
            if (!hit) return;
            
            GameObject objectHit = hit.transform.gameObject;
            if (objectHit != gameObject) return;

            StartCoroutine(Shake());
            if (--health > 0) return;

            Vector3 pos = objectHit.transform.position;
            
            Destroy(objectHit);
            world.GetTilemap().SetTile(tilemapPos, null);

            ResourceItem itemRes = resource.GetComponent<ResourceItem>();
            for (var index = 0; index < UnityEngine.Random.Range(itemRes.minSpawn, itemRes.maxSpawn); index++) {
                GameObject item = Instantiate(resource);
                item.transform.position = pos;
                item.GetComponent<Rigidbody2D>().AddForce(new Vector2(UnityEngine.Random.Range(-itemRes.spawnVelocity, itemRes.spawnVelocity), UnityEngine.Random.Range(-itemRes.spawnVelocity, itemRes.spawnVelocity)));
                item.GetComponent<ResourceItem>().world = world;
            }
        }
    }
}