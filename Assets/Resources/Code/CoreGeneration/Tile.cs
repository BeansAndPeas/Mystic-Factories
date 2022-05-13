/*
    MIT License

    Copyright (c) [2017] jgallant.com 

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
 */

namespace Resources.Code.Test {
    public enum HeightType {
        DeepWater = 1,
        ShallowWater = 2,
        Shore = 3,
        Sand = 4,
        Grass = 5,
        Forest = 6,
        Rock = 7,
        Snow = 8
    }

    public enum HeatType {
        Coldest = 0,
        Colder = 1,
        Cold = 2,
        Warm = 3,
        Warmer = 4,
        Warmest = 5
    }

    public enum MoistureType {
        Wettest = 5,
        Wetter = 4,
        Wet = 3,
        Dry = 2,
        Dryer = 1,
        Driest = 0
    }

    public enum BiomeType {
        Desert,
        Savanna,
        TropicalRainforest,
        Grassland,
        Woodland,
        SeasonalForest,
        TemperateRainforest,
        BorealForest,
        Tundra,
        Ice
    }

    public class Tile {
        public HeightType HeightType;
        public HeatType HeatType;
        public MoistureType MoistureType;
        public BiomeType BiomeType;

        public float HeightValue { get; set; }
        public int X, Y;
        public int BiomeBitmask;

        public Tile Left;
        public Tile Right;
        public Tile Top;
        public Tile Bottom;

        public bool Collidable;
        public bool FloodFilled;
        
        public void UpdateBiomeBitmask() {
            var count = 0;

            if (Collidable && Top.BiomeType == BiomeType)
                count += 1;
            if (Collidable && Right.BiomeType == BiomeType)
                count += 2;
            if (Collidable && Bottom.BiomeType == BiomeType)
                count += 4;
            if (Collidable && Left.BiomeType == BiomeType)
                count += 8;

            BiomeBitmask = count;
        }
    }
}
