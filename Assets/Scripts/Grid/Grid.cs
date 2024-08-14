using System;
using CodeMonkey.Utils;
using UnityEngine;

public class Grid<TGridObject> {
    public event EventHandler<OnGridValueChangedEventArgs> OnGridObjectChanged;

    public class OnGridValueChangedEventArgs : EventArgs {
        public int x;
        public int y;
    }

    bool showDebug;
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridObject[,] gridArray;
    private TextMesh[,] debugTextArray;
    
    public Grid(int width, int height, float cellSize, Vector3 originPosition, bool showDebug, Transform debugTextParent, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        this.showDebug = showDebug;

        gridArray = new TGridObject[width, height];
        //Debug.Log( width + " " + height);

        for (int x = 0; x < gridArray.GetLength(0); x++) {
            for (int y = 0; y < gridArray.GetLength(1); y++) {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }

        if (showDebug) {
            debugTextArray = new TextMesh[width, height];
            
            for (int x = 0; x < gridArray.GetLength(0); x++) {
                for (int y = 0; y < gridArray.GetLength(1); y++) {
                    debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y]?.ToString(), debugTextParent, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 45, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                    //Debug.Log(x + ", " + y);

                    OnGridObjectChanged += (sender, eventArgs) => {
                        debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
                    };
                }
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    public void SetDebug(bool setDebug) {
        showDebug = setDebug;
    }

    public Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public int GetWidth() {
        return width;
    }

    public int GetHeight() {
        return height;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y) {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public void SetGridObject(int x, int y, TGridObject value) {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            gridArray[x, y] = value;
            if (showDebug)
                debugTextArray[x, y].text = gridArray[x, y].ToString();
            OnGridObjectChanged?.Invoke(this, new OnGridValueChangedEventArgs { x = x, y = y });
        }
    }

    public void TriggerGridObjectChanged(int x, int y) {
        OnGridObjectChanged?.Invoke(this, new OnGridValueChangedEventArgs { x = x, y = y });
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public TGridObject GetGridObject(int x, int y) {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            return gridArray[x, y];
        }

        // Return -1 for invalid value
        return default;
    }

    public TGridObject GetGridObject(Vector3 worldPosition) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    public float GetCellSize() {
        return cellSize;
    }

    public Bounds GetGridBounds() {
        // Calculate the size of the grid in world units
        Vector3 gridSize = new Vector3(width * cellSize, height * cellSize, 0f);

        // Calculate the center of the grid in world units
        Vector3 gridCenter = originPosition + gridSize * 0.5f;

        // Create and return the Bounds object
        return new Bounds(gridCenter, gridSize);
    }
}