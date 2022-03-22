using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Resources.Code {
    public class World : MonoBehaviour {
        private readonly Dictionary<TilePos, Tile> tiles = new Dictionary<TilePos, Tile>();

        private void Generate(TilePos pos) {
            
        }

        private void Update() {
            
        }

        public Tile GetTile(int x, int y) {
            return (from keyValuePair in tiles
                let key = keyValuePair.Key
                where key.X == x && key.Y == y
                select keyValuePair.Value).FirstOrDefault();
        }

        public Tile GetTile(TilePos pos) {
            return GetTile(pos.X, pos.Y);
        }

        public Tile GetTile(Vector2 pos) => GetTile((int) pos.x, (int) pos.y);
    }
}
