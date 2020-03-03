using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tile 
{
    int _x;
    int _y;
    Map _map;
    public enum TileTypes {
        FLOOR,
        WALL
    }
    TileTypes _type;

    Action<Tile> cbTileChanged;    

    bool isExplored = false;
    bool isVisible = false;

    public List<Entity> _entities;

    
    // accessors and such
    public int Y { get => _y; set => _y = value; }
    public int X { get => _x; set => _x = value; }
    public TileTypes Type { get => _type; set {
         _type = value;
         // call callbacks on tile type change
        if (cbTileChanged != null) cbTileChanged(this);     
         } 
    }
    public Map Map { get => _map; set => _map = value; }

    public bool IsExplored { get => isExplored; set => isExplored = value; }

    public bool IsVisible { get => isVisible; set {
            isVisible = value;
            if (value) isExplored = true;
            // call callbacks because tile visibility values change

            if ( cbTileChanged != null) cbTileChanged(this);
        }
    }

    public Tile (Map map, int x, int y, TileTypes type=TileTypes.WALL) {
        this.Map = map;
        this.X = x;
        this.Y = y;
        this.Type = type;
        _entities = new List<Entity>();
    }


    public bool IsPassable(){
        foreach (Entity e in _entities) {
            if (!e.isPassable) {
                return false;
            } 
        }
        return (_type != TileTypes.WALL);

    }

    public void Dig() {
        _type = TileTypes.FLOOR;
        Map.FloorTiles.Add(this);

    }

    public override string ToString() {
        return ("Tile (" + X + ", " + Y +")");
    }

    public void Enter(Entity e){
        // add to list of entities on this tile
        _entities.Add(e);

    }

    public void Exit(Entity e) {
        // remove from list of entities on this tile
        _entities.Remove(e);
    }

    public Actor GetActorOnTile() {
        foreach (Entity e in _entities)
        {
            if (!e.isPassable)
            {
                return e as Actor; // this seems like some funky hoodoo
            } 
        }
        return null;
    }

    public void RegisterTileChangedCallback(Action<Tile> callback) {
        cbTileChanged += callback;
    }

    public void UnregisterTileChangedCallback(Action<Tile> callback)
    {
        cbTileChanged -= callback;
    }

}
