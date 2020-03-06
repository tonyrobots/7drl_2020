using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Entity 
{
    private Tile tile;
    private string symbol;
    private Color color;
    private Map map;
    private string name;

    bool isVisible = false;
    public bool isPassable = false;
    public bool isCarryable = false;

    protected Action<Entity> cbEntityChanged;

    public Tile Tile { get => tile;  set => tile = value;} 
    public string Symbol { get => symbol; set => symbol = value; }
    public Color Color { get => color; set => color = value; }
    public Map Map { get => map; set => map = value; }
    public string Name { get => name; set => name = value; }
    public bool IsVisible { get => isVisible; set { 
        isVisible = value;
        if (cbEntityChanged != null) cbEntityChanged(this); // call callbacks        
        }
    }


    public void RegisterEntityChangedCallback(Action<Entity> callback)
    {
        cbEntityChanged += callback;
    }

    public void UnregisterEntityChangedCallback(Action<Entity> callback)
    {
        cbEntityChanged -= callback;
    }

    public virtual void DoTurn() {
    }

    public void PlaceAtTile(Tile tile)
    {
        // if it's the player, let's do an FOV refresh
        if (this.GetType() == typeof(Player)) {
            Map.Game.Player.fovHelper.FOV(tile);
        }

        // if we are moving from a previous tile, exit that one
        if (Tile != null) {
            Tile.Exit(this);
        } else {
            // or else this is a new entity, add it to the map's list of entities
            tile.Map.AddEntity(this);
        }
        // now set the tile, enter the tile, set visibility, etc
        Tile = tile;
        Map = tile.Map;
        Tile.Enter(this);
        IsVisible = tile.IsVisible;
    }

    public void RemoveFromMap(){
        if (Tile != null)
        {
            Tile.Exit(this);
        }
        Map.RemoveEntity(this);
        Tile=null;
        IsVisible = false;
        // Map = null;
    }

    public virtual void DropItem(Item i)
    {
        // note this doesn't remove from inventory, it just puts the item next to or under the dropper
        i.CarriedBy = null;
        i.PlaceAtTile(Tile.GetRandomAdjacentEmptyTile());
    }



}
