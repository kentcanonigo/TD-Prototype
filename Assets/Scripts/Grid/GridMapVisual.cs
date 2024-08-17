using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GridMapVisual : MonoBehaviour {
    
    [Serializable]
    public struct TilemapSpriteUV {
        [FormerlySerializedAs("nodeSprite")] [FormerlySerializedAs("tilemapSprite")] public GridMapObject.NodeType nodeType;
        public Vector2Int uv00Pixels;
        public Vector2Int uv11Pixels;
    }

    private struct UVCoords {
        public Vector2 uv00;
        public Vector2 uv11;
    }

    [SerializeField] private TilemapSpriteUV[] tilemapSpriteUVArray;
    
    private Grid<GridMapObject> grid;
    private Mesh mesh;
    private bool updateMesh;
    private Dictionary<GridMapObject.NodeType, UVCoords> uvCoordsDictionary;

    private void Awake() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Texture texture = GetComponent<MeshRenderer>().material.mainTexture;
        float textureWidth = texture.width;
        float textureHeight = texture.height;
        
        uvCoordsDictionary = new Dictionary<GridMapObject.NodeType, UVCoords>();
        foreach (TilemapSpriteUV tilemapSpriteUV in tilemapSpriteUVArray) {
            uvCoordsDictionary[tilemapSpriteUV.nodeType] = new UVCoords {
                uv00 = new Vector2(tilemapSpriteUV.uv00Pixels.x / textureWidth, tilemapSpriteUV.uv00Pixels.y / textureHeight),
                uv11 = new Vector2(tilemapSpriteUV.uv11Pixels.x / textureWidth, tilemapSpriteUV.uv11Pixels.y / textureHeight),
            };
        }
    }

    public void SetGrid(GridMap gridMap, Grid<GridMapObject> grid) {
        this.grid = grid;
        UpdateTilemapVisual();
        
        grid.OnGridObjectChanged += Grid_OnGridObjectChanged;
        gridMap.OnLoaded += GridMap_OnLoaded;
    }

    private void GridMap_OnLoaded(object sender, EventArgs e) {
        updateMesh = true;
    }

    private void Grid_OnGridObjectChanged(object sender, Grid<GridMapObject>.OnGridValueChangedEventArgs e) {
        //Debug.Log("Grid_GridOnValueChanged");
        updateMesh = true;
    }

    private void LateUpdate() {
        if (updateMesh) {
            updateMesh = false;
            UpdateTilemapVisual();
        }
    }

    private void UpdateTilemapVisual() {
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();

                GridMapObject gridObject = grid.GetGridObject(x, y);
                GridMapObject.NodeType nodeType = gridObject.GetNodeType();
                Vector2 gridUV00, gridUV11;
                if (nodeType is GridMapObject.NodeType.None or GridMapObject.NodeType.Core or GridMapObject.NodeType.Vortex) {
                    gridUV00 = Vector2.zero;
                    gridUV11 = Vector2.zero;
                    quadSize = Vector3.zero;
                } else {
                    UVCoords uvCoords = uvCoordsDictionary[nodeType];
                    gridUV00 = uvCoords.uv00;
                    gridUV11 = uvCoords.uv11;
                }
                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + quadSize * 0.5f, 0f, quadSize, gridUV00, gridUV11);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}
