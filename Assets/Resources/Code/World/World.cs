using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Resources.Code.Resources.Code.Testing;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Resources.Code {
    public class World : MonoBehaviour {
        private readonly Dictionary<TilePos, GameObject> tiles = new Dictionary<TilePos, GameObject>();
        private readonly Dictionary<Color, Tile> TileColorCache = new Dictionary<Color, Tile>();
        private readonly Dictionary<Tile, Sprite> TileSpriteCache = new Dictionary<Tile, Sprite>();
        
        [SerializeField]
        private MeshRenderer biomeMapRenderer;
        [SerializeField]
        private GameObject tilePrefab;
        [SerializeField]
        private Tilemap tilemap;
        [SerializeField]
        private TileBase whiteTile;
        [SerializeField]
        private GameObject orePrefab;
        [SerializeField]
        private MeshTest meshTest;
        
        internal IEnumerator GenerateWorld(Image loadingBar) {
            var texture = biomeMapRenderer.material.mainTexture as Texture2D;
            var widthScaled = (int) ((texture.width / 2f) * 0.16f);
            var heightScaled = (int) ((texture.height / 2f) * 0.16f);

            float complete = 0;
            for (var x = 0; x < texture.width; x++) {
                for (var y = 0; y < texture.height; y++) {
                    if (y % 64 == 0)
                        yield return new WaitForSeconds(0.001f);

                    Color color = texture.GetPixel(x, y);
                    if (!TileColorCache.ContainsKey(color)) {
                        var t = TileList.Tiles.First(tile => tile.Color == color);
                        TileColorCache.Add(color, t);
                    }

                    Tile tile = TileColorCache[color];
                    GameObject tileObj = Instantiate(tilePrefab, transform);
                    tileObj.transform.position = new Vector2(RoundTo(x * .16f, 0.16f) - widthScaled, RoundTo(y * .16f, 0.16f) - heightScaled);
                    
                    if (!TileSpriteCache.ContainsKey(tile)) {
                        TileSpriteCache.Add(tile, UnityEngine.Resources.Load<Sprite>("Textures/" + tile.Sprite));
                    }
                    
                    tileObj.GetComponent<SpriteRenderer>().sprite = TileSpriteCache[tile];
                    
                    tileObj.name = tile.Name;
                    tiles.Add(new TilePos(x, y), tileObj);
                    loadingBar.fillAmount = Mathf.Lerp(0.2f, 0.8f, complete++ / (texture.width * texture.height));
                }
            }

            biomeMapRenderer.gameObject.SetActive(false);
            
            StartCoroutine(GenerateOres(loadingBar, widthScaled, heightScaled));
        }

        private IEnumerator GenerateOres(Image loadingBar, int width, int height) {
            // get a random number between 1/8th of the tile count and 1/4th of the tile count
            var oreCount = Random.Range(tiles.Count / 64, tiles.Count / 48);
            
            int counter = 1;
            
            // loop through the orecount
            for (var attempt = 0; attempt < oreCount; attempt++) {
                yield return new WaitForSeconds(0.01f);
                
                // get a random position from the tiles dictionary's keys
                var position = tiles.Keys.ElementAt(Random.Range(0, tiles.Keys.Count));
                var tile = tiles[position];
            
                Vector3Int tilePos = tilemap.WorldToCell(new Vector3(RoundTo(position.X * .16f, 0.16f) - width, RoundTo(position.Y * .16f, 0.16f) - height, 1));
                print(tilePos);
                
                // check if the tilemap has a tile at the position
                if (tilemap.HasTile(new Vector3Int(tilePos.x, tilePos.y, 1)) || tile.GetComponent<SpriteRenderer>().sprite.name != "woodland") {
                    continue;
                }
                
                // set ore at position
                tilemap.SetTile(new Vector3Int(tilePos.x, tilePos.y, 1), whiteTile);
                
                // create ore object
                var ore = Instantiate(orePrefab, transform);
                ore.transform.position = new Vector3(RoundTo(position.X * .16f, 0.16f) - width, RoundTo(position.Y * .16f, 0.16f) - height, 1);
                ore.name = "Ore_" + counter++;
                loadingBar.fillAmount = Mathf.Lerp(0.8f, 1.0f, (float) attempt / oreCount);
            }
            
            loadingBar.fillAmount = 1.0f;
            yield return new WaitForSeconds(1f);
            meshTest.Init(tiles.Keys.ToArray());
            UIController.FinishedGenerating = true;
        }

        public GameObject GetTile(int x, int y) {
            return (from keyValuePair in tiles
                let key = keyValuePair.Key
                where key.X == x && key.Y == y
                select keyValuePair.Value).FirstOrDefault();
        }

        public GameObject GetTile(TilePos pos) {
            return GetTile(pos.X, pos.Y);
        }

        public GameObject GetTile(Vector2 pos) => GetTile((int) pos.x, (int) pos.y);
        
        private static float RoundTo(float value, float multipleOf) {
            return Mathf.Round(value/multipleOf) * multipleOf;
        }
    }
}