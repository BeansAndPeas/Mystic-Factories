using System;
using System.Collections;
using System.Collections.Generic;
using Resources.Code.Building;
using UnityEngine;

namespace Resources.Code {
    public class PlaceableObject : MonoBehaviour {
        public bool Placed { get; set; }
        public Vector3Int Size { get; private set; }
        private Vector2[] vertices;

        private void GetColliderVertexPositionsLocal() {
            SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
            var rect = sprite.sprite.rect;
            print(rect);
            vertices = new Vector2[4];

            float width = rect.width;
            float height = rect.height;
        }

        private void CalculateSizeInCells() {
            var vertices = new Vector2Int[this.vertices.Length];
            for (int index = 0; index < vertices.Length; index++) {
                Vector2 worldPos = transform.TransformPoint(this.vertices[index]);
                Vector3Int pos = BuildSystem.Current.gridLayout.WorldToCell(worldPos);
                vertices[index] = new Vector2Int(pos.x, pos.y);
            }

            print(Math.Abs((vertices[0] - vertices[1]).x));
            print(Math.Abs((vertices[0] - vertices[3]).y));
            Size = new Vector3Int(Math.Abs((vertices[0] - vertices[1]).x), Math.Abs((vertices[0] - vertices[3]).y), 1);
        }

        public Vector2 GetStartPosition() {
            return transform.TransformPoint(vertices[0]);
        }

        private void Start() {
            GetColliderVertexPositionsLocal();
            CalculateSizeInCells();
        }

        public virtual void Place() {
            ObjectDrag drag = gameObject.GetComponent<ObjectDrag>();
            Destroy(drag);

            Placed = true;
        }
    }
}
