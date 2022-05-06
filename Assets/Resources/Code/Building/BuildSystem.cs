using System;
using Resources.Code.Resources.Code.Building;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

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
        
        public void SetObject(GameObject obj) {
            if (obj == null) return;

            buildable = obj;
            buildablePreview = Instantiate(obj);
            var draggable = buildablePreview.AddComponent<DraggableObject>();
            draggable.buildSystem = this;
            draggable.ready = true;
        }

        public static Vector3 GetPosition() {
            var pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            pos.x = RoundTo(pos.x, 0.16f);
            pos.y = RoundTo(pos.y, 0.16f);
            pos.z = 0;
            return pos;
        }
        
        private static float RoundTo(float value, float multipleOf) {
            return Mathf.Round(value/multipleOf) * multipleOf;
        }

        public bool Build() {
            var obj = buildablePreview;
            // divide position bu 0.16f to get the grid position
            var pos = grid.WorldToCell(obj.transform.position);
            pos.z = 1;
            
            // get the size of the buildable's sprite
            var sprite = obj.GetComponent<SpriteRenderer>().sprite;
            var size = new Vector2Int(sprite.texture.width, sprite.texture.height);
            
            // divide the size by 16f to get the grid size
            size.x = Mathf.RoundToInt(size.x / 16f);
            size.y = Mathf.RoundToInt(size.y / 16f);
            
            // fill the tilemap with positions and size
            int startX, startY, endX, endY;

            if (pos.x < 0) {
                startX = Mathf.FloorToInt(pos.x - size.x / 2f) - 1;
                endX = Mathf.CeilToInt(pos.x + size.x / 2f) - 1;
            } else {
                startX = Mathf.FloorToInt(pos.x - size.x / 2f);
                endX = Mathf.FloorToInt(pos.x + size.x / 2f);
            }

            if (pos.y < 0) {
                startY = Mathf.CeilToInt(pos.y - size.y / 2f);
                endY = Mathf.FloorToInt(pos.y + size.y / 2f);
            } else {
                startY = Mathf.FloorToInt(pos.y - size.y / 2f);
                endY = Mathf.FloorToInt(pos.y + size.y / 2f);
            }
            
            // check that all positions are valid
            if (!CheckValidPositions(new Vector2Int(startX, startY), new Vector2Int(endX - startX, endY - startY))) return false;

            tilemap.BoxFill(pos, whiteTile, startX, startY, endX, endY);
            
            buildablePreview = Instantiate(buildable);
            var draggable = buildablePreview.AddComponent<DraggableObject>();
            draggable.buildSystem = this;
            StartCoroutine(draggable.Wait());
            
            return true;
        }
        
        // check that all positions are valid
        private bool CheckValidPositions(Vector2Int pos, Vector2Int size) {
            for (var x = pos.x; x < pos.x + size.x; x++) {
                for (var y = pos.y; y < pos.y + size.y; y++) {
                    if (tilemap.HasTile(new Vector3Int(x, y, 1))) return false;
                }
            }

            return true;
        }
    }
}
