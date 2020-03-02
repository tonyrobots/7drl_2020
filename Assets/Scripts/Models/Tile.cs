using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tile 
{
    int _x;
    int _y;
    Map _map;
    public enum TileType {
        FLOOR,
        WALL
    }
    TileType _type;

    Action<Tile> cbTileChanged;    

    bool isRevealed = false;
    bool isVisible = false;

    
    // accessors and such
    public int Y { get => _y; set => _y = value; }
    public int X { get => _x; set => _x = value; }
    public TileType Type { get => _type; set {
         _type = value;
        if (cbTileChanged != null) cbTileChanged(this);     
         } 
    }
    public Map Map { get => _map; set => _map = value; }

    public bool IsRevealed { get => isRevealed; set => isRevealed = value; }
    public bool IsVisible { get => isVisible; set {
            isVisible = value;
            if (value) isRevealed = true;
            if ( cbTileChanged != null) cbTileChanged(this);
        }
    }

    public Tile (Map map, int x, int y, TileType type=TileType.WALL) {
        this.Map = map;
        this.X = x;
        this.Y = y;
        this.Type = type;
    }

    public bool IsBetween(int x1, int x2, int y1, int y2) // this isn't used
    {
        return ((x1 < this.X && this.X < x2) || (y1 < this.Y && this.Y < y2));
    }

    public bool IsPassable(){
        return (_type != TileType.WALL);
    }

    public void Dig() {
        _type = TileType.FLOOR;
    }

    public override string ToString() {
        return ("Tile (" + X + ", " + Y +")");
    }

    public void RegisterTileChangedCallback(Action<Tile> callback) {
        cbTileChanged += callback;
    }

    public void UnregisterTileChangedCallback(Action<Tile> callback)
    {
        cbTileChanged -= callback;
    }

}
