using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Resources.Code.Resources.Code.Testing {
    
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class MeshTest : MonoBehaviour {
        private Mesh finalMesh;
        
        public void Init(TilePos[] positions) {
            StartCoroutine(CreateMesh(positions));
        }

        private void Update() {
            if (finalMesh == null) return;
            GetComponent<MeshFilter>().mesh = finalMesh;
            
            transform.rotation = Quaternion.Euler(0, 180, 0);
            finalMesh = null;
        }

        private IEnumerator CreateMesh(IReadOnlyCollection<TilePos> positions) {
            int vertexCount = positions.Count * 4;
            int triangleCount = positions.Count * 6;
            
            var mesh = new Mesh();
            mesh.name = "TestMesh";
            
            mesh.vertices = new Vector3[vertexCount];
            mesh.triangles = new int[triangleCount];
            mesh.uv = new Vector2[vertexCount];
            mesh.colors = new Color[vertexCount];
            
            CombineInstance[] combine = new CombineInstance[positions.Count];

            int meshIndex = 0;
            // for every position in the list, create a quad and add it to the mesh
            foreach (var position in positions) {
                // create the tile mesh
                var tile = CreateTile("TestTile");
                combine[meshIndex++] = new CombineInstance {
                                                               mesh = tile,
                                                               transform = Matrix4x4.TRS(new Vector3(position.X, position.Y, 1), Quaternion.identity, Vector3.one)
                                                           };
                yield return new WaitForSeconds(0.01f);
            }
            
            mesh.CombineMeshes(combine, true, true, false);

            finalMesh = mesh;
        }

        private static Mesh CreateTile(string name) {
            return new Mesh {
                                     vertices = new[] {
                                                          new Vector3(-0.5f, -0.5f, 0),
                                                          new Vector3(0.5f, -0.5f, 0),
                                                          new Vector3(0.5f, 0.5f, 0),
                                                          new Vector3(-0.5f, 0.5f, 0)
                                                      },
                                     triangles = new[] {
                                                           0, 1, 2,
                                                           2, 3, 0
                                                       },
                                     normals = new[] {
                                                         Vector3.back, Vector3.back, 
                                                         Vector3.back, Vector3.back
                                                     },
                                     name = name
                                 };
        }
    }
}
