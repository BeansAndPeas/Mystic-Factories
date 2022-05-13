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

using System;
using System.Collections;
using System.Collections.Generic;
using AccidentalNoise;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Resources.Code.Test {
    public class Generator : MonoBehaviour {
        private int seed;

        // Adjustable variables for Unity Inspector
        [Header("Generator Values")]
        [SerializeField]
        private int width = 512;
        [SerializeField]
        private int height = 512;

        [Header("Height Map")]
        [SerializeField]
        private int terrainOctaves = 6;
        [SerializeField]
        private double terrainFrequency = 1.25;
        [SerializeField]
        private float deepWater = 0.2f;
        [SerializeField]
        private float shallowWater = 0.4f;
        [SerializeField]
        private float sand = 0.5f;
        [SerializeField]
        private float grass = 0.7f;
        [SerializeField]
        private float forest = 0.8f;
        [SerializeField]
        private float rock = 0.9f;

        [Header("Heat Map")]
        [SerializeField]
        private int heatOctaves = 4;
        [SerializeField]
        private double heatFrequency = 3.0;
        [SerializeField]
        private float coldestValue = 0.05f;
        [SerializeField]
        private float colderValue = 0.18f;
        [SerializeField]
        private float coldValue = 0.4f;
        [SerializeField]
        private float warmValue = 0.6f;
        [SerializeField]
        private float warmerValue = 0.8f;

        [Header("Moisture Map")]
        [SerializeField]
        private int moistureOctaves = 4;
        [SerializeField]
        private double moistureFrequency = 3.0;
        [SerializeField]
        private float dryerValue = 0.27f;
        [SerializeField]
        private float dryValue = 0.4f;
        [SerializeField]
        private float wetValue = 0.6f;
        [SerializeField]
        private float wetterValue = 0.8f;
        [SerializeField]
        private float wettestValue = 0.9f;

        // private variables
        private ImplicitFractal heightMap;
        private ImplicitCombiner heatMap;
        private ImplicitFractal moistureMap;

        private MapData heightData;
        private MapData heatData;
        private MapData moistureData;

        private Tile[,] tiles;

        // Our texture output GameObject
        [SerializeField]
        private MeshRenderer heightMapRenderer;
        [SerializeField]
        private MeshRenderer heatMapRenderer;
        [SerializeField]
        private MeshRenderer moistureMapRenderer;
        [SerializeField]
        private MeshRenderer biomeMapRenderer;

        protected readonly BiomeType[,] BiomeTable = {
                                                         //COLDEST        //COLDER          //COLD                  //HOT         //HOTTER            //HOTTEST
                                                         {BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland, BiomeType.Desert, BiomeType.Desert, BiomeType.Desert}, //DRIEST
                                                         {BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland, BiomeType.Desert, BiomeType.Desert, BiomeType.Desert}, //DRYER
                                                         {BiomeType.Ice, BiomeType.Tundra, BiomeType.Woodland, BiomeType.Woodland, BiomeType.Savanna, BiomeType.Savanna}, //DRY
                                                         {BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.Woodland, BiomeType.Savanna, BiomeType.Savanna}, //WET
                                                         {BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.SeasonalForest, BiomeType.TropicalRainforest, BiomeType.TropicalRainforest}, //WETTER
                                                         {BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.TemperateRainforest, BiomeType.TropicalRainforest, BiomeType.TropicalRainforest} //WETTEST
                                                     };

        private void Start() {
            seed = Random.Range(0, int.MaxValue);
        }

        public IEnumerator GenerateWorld(Image loadingBar, World world) {
            Initialize();
            loadingBar.fillAmount = 0.07f;
            yield return new WaitForSeconds(0.5f);
            GetData();
            loadingBar.fillAmount = 0.08f;
            yield return new WaitForSeconds(0.5f);
            LoadTiles();
            loadingBar.fillAmount = 0.09f;
            yield return new WaitForSeconds(0.5f);
            UpdateNeighbors();
            loadingBar.fillAmount = 0.1f;
            yield return new WaitForSeconds(0.5f);
            FloodFill();
            loadingBar.fillAmount = 0.12f;
            yield return new WaitForSeconds(0.5f);
            GenerateBiomeMap();
            loadingBar.fillAmount = 0.13f;
            yield return new WaitForSeconds(0.5f);
            UpdateBiomeBitmask();
            loadingBar.fillAmount = 0.1475f;
            yield return new WaitForSeconds(0.5f);
            heightMapRenderer.materials[0].mainTexture = TextureGenerator.GetHeightMapTexture(width, height, tiles);
            loadingBar.fillAmount = 0.16f;
            yield return new WaitForSeconds(0.5f);
            heatMapRenderer.materials[0].mainTexture = TextureGenerator.GetHeatMapTexture(width, height, tiles);
            loadingBar.fillAmount = 0.17f;
            yield return new WaitForSeconds(0.5f);
            moistureMapRenderer.materials[0].mainTexture = TextureGenerator.GetMoistureMapTexture(width, height, tiles);
            loadingBar.fillAmount = 0.18f;
            yield return new WaitForSeconds(0.5f);
            biomeMapRenderer.materials[0].mainTexture = TextureGenerator.GetBiomeMapTexture(width, height, tiles);
            loadingBar.fillAmount = 0.2f;

            StartCoroutine(world.GenerateWorld(loadingBar));
        }

        private void Initialize() {
            // Initialize the HeightMap Generator
            heightMap = new ImplicitFractal(FractalType.MULTI,
                BasisType.SIMPLEX,
                InterpolationType.QUINTIC,
                terrainOctaves,
                terrainFrequency,
                seed);


            // Initialize the Heat map
            var gradient = new ImplicitGradient(1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            var heatFractal = new ImplicitFractal(FractalType.MULTI,
                BasisType.SIMPLEX,
                InterpolationType.QUINTIC,
                heatOctaves,
                heatFrequency,
                seed);

            heatMap = new ImplicitCombiner(CombinerType.MULTIPLY);
            heatMap.AddSource(gradient);
            heatMap.AddSource(heatFractal);

            // moisture map
            moistureMap = new ImplicitFractal(FractalType.MULTI,
                BasisType.SIMPLEX,
                InterpolationType.QUINTIC,
                moistureOctaves,
                moistureFrequency,
                seed);
        }

        // Extract data from a noise module
        private void GetData() {
            heightData = new MapData(width, height);
            heatData = new MapData(width, height);
            moistureData = new MapData(width, height);

            // loop through each x,y point - get height value
            for (var x = 0; x < width; x++) {
                for (var y = 0; y < height; y++) {
                    // WRAP ON BOTH AXIS
                    // Noise range
                    const float dx = 2;
                    const float dy = 2;

                    // Sample noise at smaller intervals
                    float s = x / (float) width;
                    float t = y / (float) height;

                    // Calculate our 4D coordinates
                    float nx = 0 + Mathf.Cos(s * 2 * Mathf.PI) * dx / (2 * Mathf.PI);
                    float ny = 0 + Mathf.Cos(t * 2 * Mathf.PI) * dy / (2 * Mathf.PI);
                    float nz = 0 + Mathf.Sin(s * 2 * Mathf.PI) * dx / (2 * Mathf.PI);
                    float nw = 0 + Mathf.Sin(t * 2 * Mathf.PI) * dy / (2 * Mathf.PI);


                    float heightValue = (float) heightMap.Get(nx, ny, nz, nw);
                    float heatValue = (float) heatMap.Get(nx, ny, nz, nw);
                    float moistureValue = (float) moistureMap.Get(nx, ny, nz, nw);

                    // keep track of the max and min values found
                    if (heightValue > heightData.Max) heightData.Max = heightValue;
                    if (heightValue < heightData.Min) heightData.Min = heightValue;

                    if (heatValue > heatData.Max) heatData.Max = heatValue;
                    if (heatValue < heatData.Min) heatData.Min = heatValue;

                    if (moistureValue > moistureData.Max) moistureData.Max = moistureValue;
                    if (moistureValue < moistureData.Min) moistureData.Min = moistureValue;

                    heightData.Data[x, y] = heightValue;
                    heatData.Data[x, y] = heatValue;
                    moistureData.Data[x, y] = moistureValue;
                }
            }
        }

        // Build a Tile array from our data
        private void LoadTiles() {
            tiles = new Tile[width, height];

            for (var x = 0; x < width; x++) {
                for (var y = 0; y < height; y++) {
                    Tile t = new Tile {
                                          X = x,
                                          Y = y
                                      };

                    // set heightmap value
                    float heightValue = heightData.Data[x, y];
                    heightValue = (heightValue - heightData.Min) / (heightData.Max - heightData.Min);
                    t.HeightValue = heightValue;


                    if (heightValue < deepWater) {
                        t.HeightType = HeightType.DeepWater;
                        t.Collidable = false;
                    }
                    else if (heightValue < shallowWater) {
                        t.HeightType = HeightType.ShallowWater;
                        t.Collidable = false;
                    }
                    else if (heightValue < sand) {
                        t.HeightType = HeightType.Sand;
                        t.Collidable = true;
                    }
                    else if (heightValue < grass) {
                        t.HeightType = HeightType.Grass;
                        t.Collidable = true;
                    }
                    else if (heightValue < forest) {
                        t.HeightType = HeightType.Forest;
                        t.Collidable = true;
                    }
                    else if (heightValue < rock) {
                        t.HeightType = HeightType.Rock;
                        t.Collidable = true;
                    }
                    else {
                        t.HeightType = HeightType.Snow;
                        t.Collidable = true;
                    }


                    switch (t.HeightType) {
                        // adjust moisture based on height
                        case HeightType.DeepWater:
                            moistureData.Data[t.X, t.Y] += 8f * t.HeightValue;
                            break;
                        case HeightType.ShallowWater:
                            moistureData.Data[t.X, t.Y] += 3f * t.HeightValue;
                            break;
                        case HeightType.Shore:
                            moistureData.Data[t.X, t.Y] += 1f * t.HeightValue;
                            break;
                        case HeightType.Sand:
                            moistureData.Data[t.X, t.Y] += 0.2f * t.HeightValue;
                            break;
                        case HeightType.Grass:
                            break;
                        case HeightType.Forest:
                            break;
                        case HeightType.Rock:
                            break;
                        case HeightType.Snow:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    // Moisture Map Analyze	
                    float moistureValue = moistureData.Data[x, y];
                    moistureValue = (moistureValue - moistureData.Min) / (moistureData.Max - moistureData.Min);

                    // set moisture type
                    if (moistureValue < dryerValue) t.MoistureType = MoistureType.Driest;
                    else if (moistureValue < dryValue) t.MoistureType = MoistureType.Dryer;
                    else if (moistureValue < wetValue) t.MoistureType = MoistureType.Dry;
                    else if (moistureValue < wetterValue) t.MoistureType = MoistureType.Wet;
                    else if (moistureValue < wettestValue) t.MoistureType = MoistureType.Wetter;
                    else t.MoistureType = MoistureType.Wettest;


                    switch (t.HeightType) {
                        // Adjust Heat Map based on Height - Higher == colder
                        case HeightType.Forest:
                            heatData.Data[t.X, t.Y] -= 0.1f * t.HeightValue;
                            break;
                        case HeightType.Rock:
                            heatData.Data[t.X, t.Y] -= 0.25f * t.HeightValue;
                            break;
                        case HeightType.Snow:
                            heatData.Data[t.X, t.Y] -= 0.4f * t.HeightValue;
                            break;
                        case HeightType.DeepWater:
                            break;
                        case HeightType.ShallowWater:
                            break;
                        case HeightType.Shore:
                            break;
                        case HeightType.Sand:
                            break;
                        case HeightType.Grass:
                            break;
                        default:
                            heatData.Data[t.X, t.Y] += 0.01f * t.HeightValue;
                            break;
                    }

                    // Set heat value
                    float heatValue = heatData.Data[x, y];
                    heatValue = (heatValue - heatData.Min) / (heatData.Max - heatData.Min);

                    // set heat type
                    if (heatValue < coldestValue) t.HeatType = HeatType.Coldest;
                    else if (heatValue < colderValue) t.HeatType = HeatType.Colder;
                    else if (heatValue < coldValue) t.HeatType = HeatType.Cold;
                    else if (heatValue < warmValue) t.HeatType = HeatType.Warm;
                    else if (heatValue < warmerValue) t.HeatType = HeatType.Warmer;
                    else t.HeatType = HeatType.Warmest;

                    tiles[x, y] = t;
                }
            }
        }

        private void UpdateNeighbors() {
            for (var x = 0; x < width; x++) {
                for (var y = 0; y < height; y++) {
                    Tile t = tiles[x, y];

                    t.Top = GetTop(t);
                    t.Bottom = GetBottom(t);
                    t.Left = GetLeft(t);
                    t.Right = GetRight(t);
                }
            }
        }

        private void UpdateBiomeBitmask() {
            for (var x = 0; x < width; x++) {
                for (var y = 0; y < height; y++) {
                    tiles[x, y].UpdateBiomeBitmask();
                }
            }
        }

        public BiomeType GetBiomeType(Tile tile) {
            return BiomeTable[(int) tile.MoistureType, (int) tile.HeatType];
        }

        private void GenerateBiomeMap() {
            for (var x = 0; x < width; x++) {
                for (var y = 0; y < height; y++) {
                    if (!tiles[x, y].Collidable) continue;

                    Tile t = tiles[x, y];
                    t.BiomeType = GetBiomeType(t);
                }
            }
        }

        private void FloodFill() {
            // Use a stack instead of recursion
            var stack = new Stack<Tile>();

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    Tile t = tiles[x, y];

                    // Tile already flood filled, skip
                    if (t.FloodFilled) continue;

                    // Land
                    if (t.Collidable) {
                        var group = new TileGroup {
                                                      Type = TileGroupType.Land
                                                  };
                        stack.Push(t);

                        while (stack.Count > 0) {
                            FloodFill(stack.Pop(), ref group, ref stack);
                        }
                    }

                    // Water
                    else {
                        var group = new TileGroup {
                                                      Type = TileGroupType.Water
                                                  };
                        stack.Push(t);

                        while (stack.Count > 0) {
                            FloodFill(stack.Pop(), ref group, ref stack);
                        }
                    }
                }
            }
        }

        private void FloodFill(Tile tile, ref TileGroup tileGroup, ref Stack<Tile> stack) {
            // Validate
            if (tile.FloodFilled)
                return;
            if (tileGroup.Type == TileGroupType.Land && !tile.Collidable)
                return;
            if (tileGroup.Type == TileGroupType.Water && tile.Collidable)
                return;

            // Add to TileGroup
            tileGroup.Tiles.Add(tile);
            tile.FloodFilled = true;

            // FloodFill into neighbors
            Tile floodTile = GetTop(tile);
            if (!floodTile.FloodFilled && tile.Collidable == floodTile.Collidable)
                stack.Push(floodTile);
            floodTile = GetBottom(tile);
            if (!floodTile.FloodFilled && tile.Collidable == floodTile.Collidable)
                stack.Push(floodTile);
            floodTile = GetLeft(tile);
            if (!floodTile.FloodFilled && tile.Collidable == floodTile.Collidable)
                stack.Push(floodTile);
            floodTile = GetRight(tile);
            if (!floodTile.FloodFilled && tile.Collidable == floodTile.Collidable)
                stack.Push(floodTile);
        }

        private Tile GetTop(Tile t) {
            return tiles[t.X, MathHelper.Mod(t.Y - 1, height)];
        }

        private Tile GetBottom(Tile t) {
            return tiles[t.X, MathHelper.Mod(t.Y + 1, height)];
        }

        private Tile GetLeft(Tile t) {
            return tiles[MathHelper.Mod(t.X - 1, width), t.Y];
        }

        private Tile GetRight(Tile t) {
            return tiles[MathHelper.Mod(t.X + 1, width), t.Y];
        }
    }
}
