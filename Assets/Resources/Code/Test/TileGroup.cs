using System.Collections.Generic;

namespace Resources.Code.Test {
    public enum TileGroupType {
        Water,
        Land
    }

    public class TileGroup {
        public TileGroupType Type;
        public readonly List<Tile> Tiles;

        public TileGroup() {
            Tiles = new List<Tile>();
        }
    }
}
