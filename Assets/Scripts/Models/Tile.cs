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

    public Action<Tile> cbTileChanged;    

    bool isExplored = false;
    bool isVisible = false;

    public List<Entity> entities;

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
        entities = new List<Entity>();
    }


    public bool IsPassable(){
        foreach (Entity e in entities) {
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
        entities.Add(e);
    }

    public void Exit(Entity e) {
        // remove from list of entities on this tile
        entities.Remove(e);
    }

    public Actor GetActorOnTile() {
        foreach (Entity e in entities)
        {
            if (!e.isPassable) // This may have false positives!
            {
                return e as Actor; // this seems like some funky hoodoo
            } 
        }
        return null;
    }

    public Item GetItemOnTile() {
        foreach (Entity e in entities)
        {
            // if (e.isCarryable)  // only works for carryable items. which is all of them, for now, but...
            // {
                return e as Item; // this seems like some funky hoodoo
            // }
        }
        return null;
    }

    public bool IsAdjacentTo(Tile otherTile) {
        if (Map.Game.allowDiagonalMovement) {
            return (Mathf.Abs(X-otherTile.X) <=1) && (Mathf.Abs(Y-otherTile.Y) <= 1);
        
        } else {
            return (Map.GetManhattanDistanceBetweenTiles(this, otherTile) == 1);
        }
    }


    public List<Tile> GetAdjacentEmptyTiles() {
        List<Tile> emptyTiles = new List<Tile>();
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y<2; y++) {
                Tile t = Map.GetTile(X+x, Y+y);            
                if ((t.entities.Count == 0) && t.IsPassable() ) {
                    emptyTiles.Add(t);
                }
            }
        }
        if (emptyTiles.Count == 0) emptyTiles.Add(Map.GetTile(X,Y));
        return emptyTiles;
    }

    public List<Tile> GetAdjacentPassableTiles() // can combine this with the previous one, no time right now!
    {
        List<Tile> passableTiles = new List<Tile>();
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                Tile t = Map.GetTile(X + x, Y + y);
                if (t.IsPassable())
                {
                    passableTiles.Add(t);
                }
            }
        }
        return passableTiles;
    }

    public Tile GetRandomAdjacentEmptyTile() {
        List<Tile> emptyTiles = GetAdjacentEmptyTiles();
        return emptyTiles[UnityEngine.Random.Range(0,emptyTiles.Count)];
    }

    public void RegisterTileChangedCallback(Action<Tile> callback) {
        cbTileChanged += callback;
    }

    public void UnregisterTileChangedCallback(Action<Tile> callback)
    {
        cbTileChanged -= callback;
    }

}
