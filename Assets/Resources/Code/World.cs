using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Resources.Code {
    public class World : MonoBehaviour {
        private readonly Dictionary<TilePos, GameObject> tiles = new Dictionary<TilePos, GameObject>();
        [SerializeField]
        private MeshRenderer biomeMapRenderer;
        [SerializeField]
        private GameObject tilePrefab;
        private readonly Dictionary<Color, Tile> TileColorCache = new Dictionary<Color, Tile>();
        private readonly Dictionary<Tile, Sprite> TileSpriteCache = new Dictionary<Tile, Sprite>();

        private void Start() => StartCoroutine(GenerateWorld());

        private IEnumerator GenerateWorld() {
            yield return new WaitForSeconds(10);

            Texture2D texture = biomeMapRenderer.material.mainTexture as Texture2D;
            int widthScaled = (int) ((texture.width / 2f) * 0.16f);
            int heightScaled = (int) ((texture.height / 2f) * 0.16f);

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
                    tileObj.transform.position = new Vector2((x * .16f) - widthScaled, y * .16f - heightScaled);
                    
                    if (!TileSpriteCache.ContainsKey(tile)) {
                        TileSpriteCache.Add(tile, UnityEngine.Resources.Load<Sprite>("Textures/" + tile.Sprite));
                    }
                    tileObj.GetComponent<SpriteRenderer>().sprite = TileSpriteCache[tile];
                    
                    tileObj.name = tile.Name;
                    tiles.Add(new TilePos(x, y), tileObj);
                }
            }

            biomeMapRenderer.gameObject.SetActive(false);
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
    }
}