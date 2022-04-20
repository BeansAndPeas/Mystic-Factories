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
            BoxCollider2D box = gameObject.GetComponent<BoxCollider2D>();
            vertices = new Vector2[4];
            
            var size = box.size;
            vertices[0] = new Vector2(-size.x, -size.y) * 0.5f;
            vertices[1] = new Vector2(size.x, -size.y) * 0.5f;
            vertices[2] = new Vector2(size.x, size.y) * 0.5f;
            vertices[3] = new Vector2(-size.x, size.y) * 0.5f;
        }

        private void CalculateSizeInCells() {
            var vertices = new Vector2Int[this.vertices.Length];
            for (int index = 0; index < vertices.Length; index++) {
                Vector2 worldPos = transform.TransformPoint(this.vertices[index]);
                Vector3Int pos = BuildSystem.Current.gridLayout.WorldToCell(worldPos);
                vertices[index] = new Vector2Int(pos.x, pos.y);
            }

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
