using System;
using UnityEngine;

public class GridMapObject {

    private Grid<GridMapObject> grid;
    public int x, y;

    public int gCost;
    public int hCost;
    public int fCost;

    public bool IsWalkable { get; private set; }
    public bool IsBuildable { get; private set; }
    public GridMapObject cameFromNode;
    
    public enum NodeType {
        None,
        PermanentModule,
        BuiltModule,
        Vortex,
        Core,
    }
    
    private NodeType nodeType = NodeType.None;
    private Turret builtTurret;
    
    public GridMapObject(Grid<GridMapObject> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
        IsWalkable = true;
        IsBuildable = true;
    }
    
    public void SetNodeType(NodeType nodeType) {
        this.nodeType = nodeType;
        switch (nodeType) {
            case NodeType.None:
                IsWalkable = true;
                IsBuildable = true;
                break;
            case NodeType.Core:
                IsWalkable = true;
                IsBuildable = false;
                break;
            case NodeType.Vortex:
                IsWalkable = true;
                IsBuildable = false;
                break;
            case NodeType.BuiltModule:
                IsWalkable = false;
                IsBuildable = false;
                break;
            case NodeType.PermanentModule:
                IsWalkable = false;
                IsBuildable = false;
                break;
            default:
                Debug.LogError("NodeType invalid!");
                break;
        }
        grid.TriggerGridObjectChanged(x, y);
    }
    
    public NodeType GetNodeType() {
        return nodeType;
    }

    public Turret GetBuiltTurret() {
        return builtTurret;
    }
    
    public void SetBuiltTurret(Turret turret) => builtTurret = turret;

    public void CalculateFCost() {
        fCost = gCost + hCost;
    }

    public void SetIsWalkable(bool isWalkable) {
        IsWalkable = isWalkable;
    }

    public void SetIsBuildable(bool isBuildable) {
        IsBuildable = isBuildable;
    }

    public override string ToString() {
        switch (nodeType) {
            case NodeType.Core:
                return $"CORE\nCnBld: {IsBuildable}";
            case NodeType.Vortex:
                return "VORTEX";
            case NodeType.BuiltModule:
                return $"BltMod\nCnBld: {IsBuildable}";
            case NodeType.PermanentModule:
                return $"PermMod\nCnBld: {IsBuildable}";
            case NodeType.None:
                return $"Empty\nCnBld: {IsBuildable}";
            default:
                return "Invalid type";
        }
    }

    [Serializable]
    public class SaveObject {
        public NodeType nodeType;
        public int x, y;
        public Turret turret;
    }

    public SaveObject Save() {
        return new SaveObject {
            nodeType = nodeType,
            x = x,
            y = y,
            turret = builtTurret,
        };
    }

    public void Load(SaveObject saveObject) {
        nodeType = saveObject.nodeType;
        builtTurret = saveObject.turret;
        SetNodeType(nodeType);
    }
}
