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

using UnityEngine;

namespace Resources.Code.Test {
    public static class TextureGenerator {
        // Height Map Colors
        public static readonly Color DeepColor = new Color(15 / 255f, 30 / 255f, 80 / 255f, 1);
        public static readonly Color ShallowColor = new Color(15 / 255f, 40 / 255f, 90 / 255f, 1);
        private static readonly Color SandColor = new Color(198 / 255f, 190 / 255f, 31 / 255f, 1);
        private static readonly Color GrassColor = new Color(50 / 255f, 220 / 255f, 20 / 255f, 1);
        private static readonly Color ForestColor = new Color(16 / 255f, 160 / 255f, 0, 1);
        private static readonly Color RockColor = new Color(0.5f, 0.5f, 0.5f, 1);
        private static readonly Color SnowColor = new Color(1, 1, 1, 1);

        // Height Map Colors
        private static readonly Color Coldest = new Color(0, 1, 1, 1);
        private static readonly Color Colder = new Color(170 / 255f, 1, 1, 1);
        private static readonly Color Cold = new Color(0, 229 / 255f, 133 / 255f, 1);
        private static readonly Color Warm = new Color(1, 1, 100 / 255f, 1);
        private static readonly Color Warmer = new Color(1, 100 / 255f, 0, 1);
        private static readonly Color Warmest = new Color(241 / 255f, 12 / 255f, 0, 1);

        // Moisture map
        private static readonly Color Driest = new Color(255 / 255f, 139 / 255f, 17 / 255f, 1);
        private static readonly Color Dryer = new Color(245 / 255f, 245 / 255f, 23 / 255f, 1);
        private static readonly Color Dry = new Color(80 / 255f, 255 / 255f, 0 / 255f, 1);
        private static readonly Color Wet = new Color(85 / 255f, 255 / 255f, 255 / 255f, 1);
        private static readonly Color Wetter = new Color(20 / 255f, 70 / 255f, 255 / 255f, 1);
        private static readonly Color Wettest = new Color(0 / 255f, 0 / 255f, 100 / 255f, 1);

        public static Texture2D GetHeightMapTexture(int width, int height, Tile[,] tiles) {
            var texture = new Texture2D(width, height);
            var pixels = new Color[width * height];

            for (var x = 0; x < width; x++) {
                for (var y = 0; y < height; y++) {
                    pixels[x + y * width] = tiles[x, y].HeightType switch {
                        HeightType.DeepWater => DeepColor,
                        HeightType.ShallowWater => ShallowColor,
                        HeightType.Sand => SandColor,
                        HeightType.Grass => GrassColor,
                        HeightType.Forest => ForestColor,
                        HeightType.Rock => RockColor,
                        HeightType.Snow => SnowColor,
                        _ => pixels[x + y * width]
                    };
                }
            }

            texture.SetPixels(pixels);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.Apply();
            return texture;
        }

        public static Texture2D GetHeatMapTexture(int width, int height, Tile[,] tiles) {
            var texture = new Texture2D(width, height);
            var pixels = new Color[width * height];

            for (var x = 0; x < width; x++) {
                for (var y = 0; y < height; y++) {
                    pixels[x + y * width] = tiles[x, y].HeatType switch {
                        HeatType.Coldest => Coldest,
                        HeatType.Colder => Colder,
                        HeatType.Cold => Cold,
                        HeatType.Warm => Warm,
                        HeatType.Warmer => Warmer,
                        HeatType.Warmest => Warmest,
                        _ => pixels[x + y * width]
                    };
                }
            }

            texture.SetPixels(pixels);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.Apply();
            return texture;
        }

        public static Texture2D GetMoistureMapTexture(int width, int height, Tile[,] tiles) {
            var texture = new Texture2D(width, height);
            var pixels = new Color[width * height];

            for (var x = 0; x < width; x++) {
                for (var y = 0; y < height; y++) {
                    Tile t = tiles[x, y];

                    pixels[x + y * width] = t.MoistureType switch {
                        MoistureType.Driest => Driest,
                        MoistureType.Dryer => Dryer,
                        MoistureType.Dry => Dry,
                        MoistureType.Wet => Wet,
                        MoistureType.Wetter => Wetter,
                        _ => Wettest
                    };
                }
            }

            texture.SetPixels(pixels);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.Apply();
            return texture;
        }

        public static readonly Color Ice = Color.white;
        public static readonly Color Desert = new Color(238 / 255f, 218 / 255f, 130 / 255f, 1);
        public static readonly Color Savanna = new Color(177 / 255f, 209 / 255f, 110 / 255f, 1);
        public static readonly Color TropicalRainforest = new Color(66 / 255f, 123 / 255f, 25 / 255f, 1);
        public static readonly Color Tundra = new Color(96 / 255f, 131 / 255f, 112 / 255f, 1);
        public static readonly Color TemperateRainforest = new Color(29 / 255f, 73 / 255f, 40 / 255f, 1);
        public static readonly Color Grassland = new Color(164 / 255f, 225 / 255f, 99 / 255f, 1);
        public static readonly Color SeasonalForest = new Color(73 / 255f, 100 / 255f, 35 / 255f, 1);
        public static readonly Color BorealForest = new Color(95 / 255f, 115 / 255f, 62 / 255f, 1);
        public static readonly Color Woodland = new Color(139 / 255f, 175 / 255f, 90 / 255f, 1);

        public static Texture2D GetBiomeMapTexture(int width, int height, Tile[,] tiles) {
            var texture = new Texture2D(width, height);
            var pixels = new Color[width * height];

            for (var x = 0; x < width; x++) {
                for (var y = 0; y < height; y++) {
                    BiomeType value = tiles[x, y].BiomeType;

                    pixels[x + y * width] = tiles[x, y].HeightType switch {
                        // Water tiles
                        HeightType.DeepWater => DeepColor,
                        HeightType.ShallowWater => ShallowColor,
                        _ => value switch {
                            BiomeType.Ice => Ice,
                            BiomeType.BorealForest => BorealForest,
                            BiomeType.Desert => Desert,
                            BiomeType.Grassland => Grassland,
                            BiomeType.SeasonalForest => SeasonalForest,
                            BiomeType.Tundra => Tundra,
                            BiomeType.Savanna => Savanna,
                            BiomeType.TemperateRainforest => TemperateRainforest,
                            BiomeType.TropicalRainforest => TropicalRainforest,
                            BiomeType.Woodland => Woodland,
                            _ => pixels[x + y * width]
                        }
                    };

                    /*// add a outline
                    if (tiles[x, y].HeightType < HeightType.Shore) continue;

                    if (tiles[x, y].BiomeBitmask != 15)
                        pixels[x + y * width] = Color.Lerp(pixels[x + y * width], Color.black, 0.35f);*/
                }
            }

            texture.SetPixels(pixels);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.Apply();
            return texture;
        }
    }
}
