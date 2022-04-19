using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace Resources.Code.Building {
    public class BuildSystem : MonoBehaviour {
        public static BuildSystem current;
        
        public GridLayout gridLayout;
        private Grid grid;
        
        [SerializeField]
        private Tilemap mainTilemap;
        [SerializeField]
        private TileBase whiteTile;
        
        public PlaceableObject objectToPlace;

        private void Awake() {
            current = this;
            grid = gridLayout.gameObject.GetComponent<Grid>();
        }

        private static readonly Vector3 zero = new Vector3(0, 0, 10);

        public static Vector3 GetMouseWorldPosition() {
            Vector3 vector = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            return Physics2D.OverlapPoint(vector) ? vector : zero;
        }

        public Vector3 SnapCoordinateToGrid(Vector3 position) {
            Vector3Int cellPos = gridLayout.WorldToCell(position);
            position = grid.GetCellCenterWorld(cellPos);
            return position;
        }

        private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap) {
            var array = new TileBase[area.size.x * area.size.y * area.size.z];
            int counter = 0;

            foreach (var position in area.allPositionsWithin) {
                var pos = new Vector3Int(position.x, position.y, position.z);
                array[counter++] = tilemap.GetTile(pos);
            }

            return array;
        }

        public void Initialize(GameObject prefab) {
            Vector3 position = SnapCoordinateToGrid(Vector3.zero);
            Debug.Log(position);
            
            GameObject obj = Instantiate(prefab, position, Quaternion.identity);
            objectToPlace = obj.GetComponent<PlaceableObject>();
            obj.AddComponent<ObjectDrag>();
        }

        public bool CanBePlaced(PlaceableObject obj) {
            var area = new BoundsInt {
                                         position = gridLayout.WorldToCell(obj.GetStartPosition()),
                                         size = new Vector3Int(obj.Size.x + 1, obj.Size.y + 1, 1)
                                     };

            TileBase[] baseArray = GetTilesBlock(area, mainTilemap);
            return baseArray.All(tile => tile != whiteTile);
        }

        public void TakeArea(Vector3Int start, Vector3Int size) {
            mainTilemap.BoxFill(start, whiteTile, start.x, start.y, start.x + size.x, start.y + size.y);
        }
    }
}
