using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Tilemap : MonoBehaviour
{
    private Grid grid;
    private Mesh mesh;
    
    private Texture2D texture;

    public void GenerateTilemap(Grid grid)
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateTexture();
        GetComponent<MeshRenderer>().sharedMaterial.mainTexture = texture;
        
        this.grid = grid;
        UpdateTileMapVisual();
    }

    private void CreateTexture()
    {
        texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        texture.SetPixel(0, 0, Color.grey);
        texture.Apply();
    }

    private void UpdateTileMapVisual() {
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();
                Vector2 gridValueUV = new Vector2(0f, 0f);
                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, 
                                          grid.GetWorldPosition(x, y) + quadSize * .5f, 
                                          0f, 
                                          quadSize, 
                                          gridValueUV, gridValueUV);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}
