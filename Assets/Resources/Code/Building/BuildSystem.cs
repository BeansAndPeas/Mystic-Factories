using Resources.Code.Resources.Code.Building;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Resources.Code.Building {
    public class BuildSystem : MonoBehaviour {
        public GameObject buildable;
        
        [SerializeField]
        private Grid grid;
        [SerializeField]
        private Tilemap tilemap;
        [SerializeField]
        private TileBase whiteTile;
        
        public void SetObject(GameObject obj) {
            if (buildable == null) return;
            
            buildable = Instantiate(obj);
            var draggable = buildable.AddComponent<DraggableObject>();
            draggable.buildSystem = this;
        }

        public void Build() {
            // divide position bu 0.16f to get the grid position
            var pos = grid.WorldToCell(buildable.transform.position);
            pos.x = Mathf.RoundToInt(pos.x / 0.16f);
            pos.y = Mathf.RoundToInt(pos.y / 0.16f);
            pos.z = 0;
            
            // get the size of the buildable's sprite
            var sprite = buildable.GetComponent<SpriteRenderer>().sprite;
            var size = new Vector2Int(sprite.texture.width, sprite.texture.height);
            
            // divide the size by 16f to get the grid size
            size.x = Mathf.RoundToInt(size.x / 16f);
            size.y = Mathf.RoundToInt(size.y / 16f);
            
            // check that all positions are valid
            if (!CheckValidPositions(new Vector2Int(pos.x, pos.y), size)) return;
            
            // fill the tilemap with positions and size
            tilemap.BoxFill(pos, whiteTile, pos.x, pos.y, pos.x + size.x, pos.y + size.y);
            
            // clone the buildable
            var obj = Instantiate(buildable, transform.parent);
            // set object position to the grid position
            obj.transform.position = new Vector3(pos.x + size.x / 2, pos.y + size.y / 2, 0);
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
