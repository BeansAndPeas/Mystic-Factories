using System.Collections.Generic;
using Resources.Code.Test;

namespace Resources.Code {
    public static class TileList {
        public static readonly List<Tile> Tiles = new List<Tile>();

        private static Tile Register(Tile.Builder builder) {
            var tile = new Tile(builder);
            Tiles.Add(tile);
            return tile;
        }

        public static readonly Tile Grassland = Register(new Tile.Builder("grassland", TextureGenerator.Grassland, "Grassland"));
        public static readonly Tile Ice = Register(new Tile.Builder("ice", TextureGenerator.Ice, "Ice"));
        public static readonly Tile Savanna = Register(new Tile.Builder("savanna", TextureGenerator.Savanna, "Savanna"));
        public static readonly Tile TropicalRainforest = Register(new Tile.Builder("tropical_rainforest", TextureGenerator.TropicalRainforest, "Tropical Rainforest"));
        public static readonly Tile Tundra = Register(new Tile.Builder("tundra", TextureGenerator.Tundra, "Tundra"));
        public static readonly Tile TemperateRainforest = Register(new Tile.Builder("temperate_rainforest", TextureGenerator.TemperateRainforest, "Temperate Rainforest"));
        public static readonly Tile SeasonalForest = Register(new Tile.Builder("seasonal_forest", TextureGenerator.SeasonalForest, "Seasonal Forest"));
        public static readonly Tile BorealForest = Register(new Tile.Builder("boreal_forest", TextureGenerator.BorealForest, "Boreal Forest"));
        public static readonly Tile Woodland = Register(new Tile.Builder("woodland", TextureGenerator.Woodland, "Woodland"));
        public static readonly Tile Desert = Register(new Tile.Builder("desert", TextureGenerator.Desert, "Desert"));
        public static readonly Tile ShallowWater = Register(new Tile.Builder("shallow_water", TextureGenerator.ShallowColor, "Shallow Water"));
        public static readonly Tile DeepWater = Register(new Tile.Builder("deep_water", TextureGenerator.DeepColor, "Deep Water"));
    }
}