using System.Collections.Generic;

namespace Resources.Code {
    public class BiomeList {
        private static readonly List<Biome> Biomes = new List<Biome>();
        
        private static Biome Register(Biome.Builder builder) {
            var biome = new Biome(builder);
            Biomes.Add(biome);
            return biome;
        }
    }
}