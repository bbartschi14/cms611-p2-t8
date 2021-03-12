using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Heatmap : MonoBehaviour
{
    public Grid grid;
    private Mesh mesh;
    public int maxDistance;
    public Color goalColor;
    public Color weakColor;
    public bool autoUpdate;
    private Texture2D texture;
    private int textureWidth = 32;

    public void GenerateHeatmap(Grid grid)
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateTexture(textureWidth);
        GetComponent<MeshRenderer>().sharedMaterial.mainTexture = texture;
        this.grid = grid;
        UpdateHeatMapVisual();
    }

    private void CreateTexture(int width)
    {
        texture = new Texture2D(width, 1, TextureFormat.ARGB32, false);
        for (int i = 0; i < width; i++)
        {
            Color col = Color.Lerp(weakColor, goalColor, (float) i / (width - 1));
            //Debug.Log("Color " + col + " value: " + (float) i / (width - 1));
            texture.SetPixel(i, 1, col);
        }

        texture.Apply();
    }

    private void UpdateHeatMapVisual() {
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();
                Vector2 gridValueUV = new Vector2(grid.GetGoalProximity(x, y, maxDistance), 0f);
                float resampledX = ResampleUV(gridValueUV.x);
                Vector2 clampedUV = new Vector2(resampledX, 0f);
                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, 
                                          grid.GetWorldPosition(x, y) + quadSize * .5f, 
                                          0f, 
                                          quadSize, 
                                          clampedUV, clampedUV);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    private float ResampleUV(float x)
    {
        x *= (textureWidth-1);
        int newX = Mathf.FloorToInt(x);
        float floatX = (float) newX / (textureWidth);
        floatX += .5f / (textureWidth);
        return floatX;
    }
}
