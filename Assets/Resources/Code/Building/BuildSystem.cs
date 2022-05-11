using Resources.Code.Resources.Code.Building;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using static UnityEngine.Mathf;

namespace Resources.Code.Building {
    public class BuildSystem : MonoBehaviour {
        private GameObject buildable;
        private GameObject buildablePreview;
        
        [SerializeField]
        private Grid grid;
        [SerializeField]
        private Tilemap tilemap;
        [SerializeField]
        private TileBase whiteTile;

        public GameObject GetObject() => buildable;

        public void SetObject(GameObject obj) {
            if (obj == null) return;

            Destroy(buildablePreview);
            
            buildable = obj;
            buildablePreview = Instantiate(obj);
            var draggable = buildablePreview.AddComponent<DraggableObject>();
            draggable.buildSystem = this;
            draggable.ready = true;
        }

        public static Vector3 GetPosition() {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            pos.x = RoundTo(pos.x, 0.16f);
            pos.y = RoundTo(pos.y, 0.16f);
            pos.z = 0;
            return pos;
        }
        
        private static float RoundTo(float value, float multipleOf) => Round(value/multipleOf) * multipleOf;

        public bool Build() {
            GameObject obj = buildablePreview;
            // divide position bu 0.16f to get the grid position
            Vector3Int pos = grid.WorldToCell(obj.transform.position);
            pos.z = 1;
            
            // get the size of the buildable's sprite
            Sprite sprite = obj.GetComponent<SpriteRenderer>().sprite;
            var size = new Vector2Int(sprite.texture.width, sprite.texture.height);
            
            // divide the size by 16f to get the grid size
            size.x = RoundToInt(size.x / 16f);
            size.y = RoundToInt(size.y / 16f);
            
            // setup bounds
            var startX = FloorToInt(pos.x - size.x / 2);
            var startY = CeilToInt(pos.y - size.y / 2);
            var endX = pos.x < 0 ? CeilToInt(pos.x + size.x / 2) : FloorToInt(pos.x + size.x / 2);
            var endY = pos.y < 0 ? CeilToInt(pos.y + size.y / 2) : FloorToInt(pos.y + size.y / 2);
            
            // check that all positions are valid
            if (!CheckValidPositions(new Vector3Int(startX, startY, 1), new Vector3Int(endX, endY, 1))) return false;

            for (var x = startX; x < endX; x++) {
                for (var y = startY; y < endY; y++) {
                    tilemap.SetTile(new Vector3Int(x, y, 1), whiteTile);
                }
            }
            
            buildablePreview = Instantiate(buildable);
            var draggable = buildablePreview.AddComponent<DraggableObject>();
            draggable.buildSystem = this;
            StartCoroutine(draggable.Wait());
            
            return true;
        }
        
        // check that all positions are valid
        private bool CheckValidPositions(Vector3Int start, Vector3Int end) {
            print(start + "\t" + end);
            
            for (var x = start.x; x < end.x; x++) {
                for (var y = start.y; y < end.y; y++) {
                    if (tilemap.HasTile(new Vector3Int(x, y, 1))) return false;
                }
            }

            return true;
        }
    }
}
