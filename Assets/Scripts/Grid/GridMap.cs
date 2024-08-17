using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridMap {

    private const int MOVE_STRAIGHT_COST = 10;
    
    public static GridMap Instance { get; private set; }
    
    private Grid<GridMapObject> grid;
    private List<GridMapObject> openList;
    private HashSet<GridMapObject> closedList;

    public event EventHandler OnLoaded;
    
    // Constructor
    
    public GridMap(int width, int height, float cellSize, bool showDebug) {
        Instance = this;
        grid = new Grid<GridMapObject>(width, height, cellSize, Vector3.zero, showDebug, null, ((g, x, y) => new GridMapObject(g, x, y)));
    }
    
    // Visuals
    
    public void SetGridMapVisual(GridMapVisual gridMapVisual) {
        gridMapVisual.SetGrid(this, grid);
    }
    
    public void SetNodeType(Vector3 worldPosition, GridMapObject.NodeType nodeType) {
        GridMapObject tilemapObject = grid.GetGridObject(worldPosition);
        if (tilemapObject != null) {
            tilemapObject.SetNodeType(nodeType);
        }
    }

    //Pathfinding
    
    public List<GridMapObject> FindPath(int startX, int startY, int endX, int endY) {
        //Debug.Log("Finding path..");
        GridMapObject startNode = grid.GetGridObject(startX, startY);
        GridMapObject endNode = grid.GetGridObject(endX, endY);
        
        openList = new List<GridMapObject> { startNode };
        closedList = new HashSet<GridMapObject>();

        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                GridMapObject gridMapObject = grid.GetGridObject(x, y);
                gridMapObject.gCost = int.MaxValue;
                gridMapObject.CalculateFCost();
                gridMapObject.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0) {
            GridMapObject currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode) {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (GridMapObject neighbourNode in GetNeighbourList(currentNode)) {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.IsWalkable) {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost) {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode)) {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        
        //Out of nodes of the openList;
        return null;
    }

    private List<GridMapObject> GetNeighbourList(GridMapObject currentNode) {
        List<GridMapObject> neighbourList = new List<GridMapObject>();
        if (currentNode.x - 1 >= 0) //Left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
        
        if (currentNode.x + 1 < grid.GetWidth()) //Right
            neighbourList.Add(GetNode(currentNode.x + 1 , currentNode.y));
        
        if (currentNode.y - 1 >= 0) //Down
            neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        
        if (currentNode.y + 1 < grid.GetHeight()) //Up
            neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighbourList;
    }

    private List<GridMapObject> CalculatePath(GridMapObject endNode) {
        List<GridMapObject> path = new List<GridMapObject>();
        path.Add(endNode);
        GridMapObject currentNode = endNode;
        while (currentNode.cameFromNode != null) {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(GridMapObject a, GridMapObject b) {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        // Apply a penalty for diagonal moves to encourage straight-line movement
        int diagonalSteps = Mathf.Min(xDistance, yDistance);
        int diagonalPenalty = 5;

        return MOVE_STRAIGHT_COST * (xDistance + yDistance) + diagonalPenalty * diagonalSteps;
    }

    private GridMapObject GetLowestFCostNode(List<GridMapObject> pathNodeList) {
        GridMapObject lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++) {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost) {
                lowestFCostNode = pathNodeList[i];
            }
        }

        return lowestFCostNode;
    }

    public Grid<GridMapObject> GetGrid() {
        return grid;
    }

    public GridMapObject GetNode(int x, int y) {
        return grid.GetGridObject(x, y);
    }

    //Save and Load
    
    public void Save() {
        List<GridMapObject.SaveObject> pathNodeSaveObjectList = new List<GridMapObject.SaveObject>();
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                GridMapObject gridMapObject = grid.GetGridObject(x, y);
                pathNodeSaveObjectList.Add(gridMapObject.Save());
            }
        }

        SaveObject saveObject = new SaveObject { gridMapSaveObjectArray = pathNodeSaveObjectList.ToArray() };

        Scene scene = SceneManager.GetActiveScene();
        SaveSystem.SaveObject(scene.name ,saveObject, true);
    }

    public class SaveObject {
        public GridMapObject.SaveObject[] gridMapSaveObjectArray;
    }

    public void Load(LevelDataSO levelDataSOData) {
        SaveObject saveObject = SaveSystem.LoadObject<SaveObject>(levelDataSOData.GetFileName());
        foreach (GridMapObject.SaveObject gridMapSaveObject in saveObject.gridMapSaveObjectArray) {
            GridMapObject gridMapObject = grid?.GetGridObject(gridMapSaveObject.x, gridMapSaveObject.y);
            gridMapObject?.Load(gridMapSaveObject);
        }
        
        // Update the visual representation for each grid object to reflect the loaded state
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                grid.TriggerGridObjectChanged(x, y); // Trigger the grid object changed event to update visuals
            }
        }
        OnLoaded?.Invoke(this, EventArgs.Empty);
    }
}