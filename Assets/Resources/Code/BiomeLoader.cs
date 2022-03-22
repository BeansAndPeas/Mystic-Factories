using System.Collections.Generic;
using UnityEngine;

namespace Resources.Code {
    public class BiomeLoader { }

    public static class BiomeRegistry {
        public static readonly List<Biome> Biomes = new List<Biome>();

        public static Biome Register(string name, float temperature, float wetness, Color color) {
            var biome = new Biome {
                                      Name = name,
                                      Temperature = temperature,
                                      Wetness = wetness,
                                      Color = color
                                  };
            Biomes.Add(biome);
            return biome;
        }
    }

    public struct Biome {
        internal string Name;
        internal float Temperature;
        internal float Wetness;
        internal Color Color;
    }
}
